using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChefConnect.Models;
using Microsoft.AspNetCore.Identity;
using ChefConnect.Entities;
using ChefConnect.Services;
using ChefConnect.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Azure;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChefConnect.Controllers
{
    [Authorize(Roles = "Chef")]
    public class ChefController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private HelperServices _helperServices = new HelperServices();
        private ChefConnectDbContext _chefConnectDbContext;

        public ChefController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ChefConnectDbContext chefConnectDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _chefConnectDbContext = chefConnectDbContext;
        }

        [AllowAnonymous]
        [HttpGet("/Register/AsChef")]
        public async Task<IActionResult> ChefRegister()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("/Register/AsChef")]
        public async Task<IActionResult> ChefRegister(RegisterChefViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                if (_userManager.FindByNameAsync(registerViewModel.UserName).Result != null)
                {
                    ModelState.AddModelError("UserName", "Username is already registered.");
                }
                if (_userManager.FindByEmailAsync(registerViewModel.Email).Result != null)
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                }
                if (!_helperServices.IsValidAge(registerViewModel.DateOfBirth))
                {
                    ModelState.AddModelError("DateOfBirth", "You must be 18 years or more to register.");
                }
                if (!isUniquePhoneNumber(registerViewModel.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Phone number is already registered.");
                }
                if (!_helperServices.IsPhoneNumberValid(registerViewModel.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Please enter a valid Canadian phone number.");
                }
                if (ModelState.ErrorCount == 0)
                {
                    var user = new AppUser { UserName = registerViewModel.UserName, Name = registerViewModel.Name };
                    var result = await _userManager.CreateAsync(user, registerViewModel.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Chef");
                        user.Email = registerViewModel.Email;
                        user.PhoneNumber = registerViewModel.PhoneNumber;
                        user.DateOfBirth = registerViewModel.DateOfBirth;
                        await _userManager.UpdateAsync(user);
                        await _signInManager.SignInAsync(user, false);
                        var message = $"\nHi,\n\nThanks for getting started with ChefConnect!\n\nWe need a little more information to complete your registration, including a confirmation of your email address.\n\nClick below to confirm your email address:\n\nhttps://localhost:7042/{registerViewModel.UserName}/Email-Verification-Success\n\nIf you have problems, please paste the above URL into your web browser.";
                        _helperServices.SendEmailAsync(registerViewModel.Email, "Email Verification", message);
                        if (!user.EmailConfirmed)
                        {
                            TempData["ConfirmEmailMessage"] = $"An email verification is sent to you. Please confirm your email there.";
                        }

                        return RedirectToAction("ChefProfile", new { username = registerViewModel.UserName });


                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("Password", error.Description);
                        }
                        return View();
                    }
                }
                else
                {
                    return View();
                }

            }
            else
            {
                return View();
            }
        }

        [HttpGet("/{username}/Chef-Profile")]
        public async Task<IActionResult> ChefProfile(string username)
        {
            ChefViewModel model = new ChefViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                chefRecipes = await _chefConnectDbContext.ChefRecipes.Include(r => r.RecipeCuisine).Include(r => r.Reviews).Where(r => r.ChefId == _userManager.FindByNameAsync(username).Result.Id).ToListAsync(),
                otherChefRecipes = await _chefConnectDbContext.ChefRecipes.Include(r => r.RecipeCuisine).Include(r => r.Chef).Include(r => r.Reviews).Where(r => r.ChefId != _userManager.FindByNameAsync(username).Result.Id).ToListAsync()
            };

            return View(model);
        }


        //Get method to take the review from the chef profile and navigate to the admin page
        [HttpGet("{id}/Review-Reported")]
        public async Task<IActionResult> GetReportedReviews(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var review = await _chefConnectDbContext.Reviews.Include(r => r.Customer).Where(r => r.ReviewsId == id).FirstOrDefaultAsync();
            review.Status = Reviews.ReviewStatus.Reported;
            _chefConnectDbContext.Reviews.Update(review);
            _chefConnectDbContext.SaveChanges();


          

            return RedirectToAction("GetChefReviewsPage", new {username = user.UserName});
        }


        [HttpGet("/{username}/Reviews")]
        public async Task<IActionResult> GetChefReviewsPage(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            ChefViewModel model = new ChefViewModel()
            {
                ChefReviews = await _chefConnectDbContext.Reviews.Include(r => r.Customer).Include(r => r.ChefRecipe).Where(r => r.Status == Reviews.ReviewStatus.Clean).Where(r => r.ChefId == user.Id).ToListAsync()
            };

            return View("MyReviews", model);
        }



        [HttpGet("/{username}/My-Bookings")]
        public async Task<IActionResult> GetMyBookingsPage(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            ChefViewModel model = new ChefViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                UpComingOrders = new List<OrderRecipes>(),
                PastOrders = new List<OrderRecipes>()
                //UpComingOrders = await _chefConnectDbContext.OrderDetails.Include(o => o.Customer).Include(o => o.Address).Include(o => o.PaymentMethod).Include(o => o.OrderRecipes).ThenInclude(or => or.ChefRecipes).Where(o => o.OrderRecipes.Any(or => or.ChefRecipes.ChefId == user.Id)).Where(o => o.Status == OrderDetails.OrderStatus.Pending).ToListAsync(),
                //PastOrders = await _chefConnectDbContext.OrderDetails.Include(o => o.Customer).Include(o => o.Address).Include(o => o.PaymentMethod).Include(o => o.OrderRecipes).ThenInclude(or => or.ChefRecipes).Where(o => o.OrderRecipes.Any(or => or.ChefRecipes.ChefId == user.Id)).Where(o => o.Status == OrderDetails.OrderStatus.Confirmed).ToListAsync()
            };
            return View("MyBookings",model);
        }

        [HttpGet("/{username}/Upcoming-Bookings")]
        public async Task<IActionResult> GetUpcomingBookings(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            ChefViewModel model = new ChefViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                UpComingOrders = await _chefConnectDbContext.OrderRecipes.Include(r => r.TimeSlot).Include(r => r.OrderDetails).ThenInclude(od => od.Customer).Include(r => r.OrderDetails).ThenInclude(od => od.Address).Include(o => o.ChefRecipes).ThenInclude(r => r.Chef).Where(r => r.ChefRecipes.ChefId == user.Id).Where(r => r.OrderDate > DateTime.Now).ToListAsync(),
                PastOrders = new List<OrderRecipes>()
                
            };
            return View("MyBookings", model);
        }

        [HttpGet("/{username}/Past-Bookings")]
        public async Task<IActionResult> GetPastBookings(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            ChefViewModel model = new ChefViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                UpComingOrders = new List<OrderRecipes>(),
                PastOrders = await _chefConnectDbContext.OrderRecipes.Include(r => r.TimeSlot).Include(r => r.OrderDetails).ThenInclude(od => od.Customer).Include(r => r.OrderDetails).ThenInclude(od => od.Address).Include(o => o.ChefRecipes).ThenInclude(r => r.Chef).Where(r => r.ChefRecipes.ChefId == user.Id).Where(r => r.OrderDate < DateTime.Now).ToListAsync()

            };
            return View("MyBookings", model);
        }

        [HttpGet("/{orderid}/{recipeid}/Cancel-Booking")]
        public async Task<IActionResult> CancelUpcomingBooking(int orderid, int recipeid)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var orderToDelete = await _chefConnectDbContext.OrderRecipes.Where(or => or.OrderDetailsId == orderid).Where(or => or.ChefRecipesId == recipeid).FirstOrDefaultAsync();
            _chefConnectDbContext.OrderRecipes.Remove(orderToDelete);
            _chefConnectDbContext.SaveChanges();

            return RedirectToAction("GetUpcomingBookings", new { username = user.UserName });
        }

        [HttpGet("/{username}/My-Recipes-Cuisines")]
        public async Task<IActionResult> GetMyRecipesAndCuisinesPage(string username)
        {
            var User = await _userManager.FindByNameAsync(username);
            ChefViewModel model = new ChefViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                chefRecipes = await _chefConnectDbContext.ChefRecipes.Include(r => r.RecipeCuisine).Where(r => r.ChefId == User.Id).ToListAsync(),
                chefCuisines = await _chefConnectDbContext.ChefCuisines.Include(cc => cc.Cuisine).Where(r => r.ChefId == User.Id).ToListAsync(),
                allCuisines = await _chefConnectDbContext.Cuisines.ToListAsync()
            };
            return View("MyRecipesAndCuisines", model);
        }

        [HttpGet("/{username}/Add-Recipes")]
        public async Task<IActionResult> GetAddRecipesPage(string username)
        {
            ChefViewModel model = new ChefViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                allCuisines = await _chefConnectDbContext.Cuisines.ToListAsync(),
                NewRecipe = new ChefRecipes()
            };
            return View("AddRecipes", model);
        }

        [HttpPost("/New-Recipe-Added")]
        public async Task<IActionResult> AddNewRecipe(ChefViewModel model)
        {
            if (Request.Form.Files.Count == 0)
            {
                TempData["imageerror"] = "Please select a picture for your recipe.";
                return RedirectToAction("GetAddRecipesPage", new { username = User.Identity.Name });
            }
            else
            {
                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        model.NewRecipe.RecipeImage = dataStream.ToArray();
                    }
                }

                _chefConnectDbContext.ChefRecipes.Add(model.NewRecipe);
                _chefConnectDbContext.SaveChanges();

                return RedirectToAction("GetMyRecipesAndCuisinesPage", new { username = User.Identity.Name });
            }
        }

        [HttpGet("/{username}/Recipe-Details/{id}")]
        public async Task<IActionResult> GetRecipeDetailsPage(string username, int id)
        {
            ChefViewModel model = new ChefViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                ActiveChefRecipe = await _chefConnectDbContext.ChefRecipes.Include(r => r.Chef).Include(r => r.RecipeCuisine).Where(r => r.ChefRecipesId == id).FirstOrDefaultAsync()
            };

            return View("RecipeDetails", model);
        }

        [HttpGet("/{username}/Edit-Recipe/{id}")]
        public async Task<IActionResult> GetEditRecipesPage(string username, int id)
        {

            ChefViewModel model = new ChefViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                allCuisines = await _chefConnectDbContext.Cuisines.ToListAsync(),
                ActiveChefRecipe = await _chefConnectDbContext.ChefRecipes.Include(r => r.RecipeCuisine).Where(r => r.ChefRecipesId == id).FirstOrDefaultAsync()
            };
            return View("EditRecipes", model);
        }

        [HttpPost("/Recipe-Edit-Success")]
        public async Task<IActionResult> EditRecipe(ChefViewModel model)
        {
            Console.WriteLine(model.ActiveChefRecipe.RecipeImage.ToString());
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    model.ActiveChefRecipe.RecipeImage = dataStream.ToArray();
                }
                _chefConnectDbContext.ChefRecipes.Update(model.ActiveChefRecipe);
                _chefConnectDbContext.SaveChanges();
            }
            else
            {
                _chefConnectDbContext.ChefRecipes.Update(model.ActiveChefRecipe);
                _chefConnectDbContext.SaveChanges();
            }

            return RedirectToAction("GetRecipeDetailsPage", new { username = User.Identity.Name, id = model.ActiveChefRecipe.ChefRecipesId });
        }

        [HttpGet("/Delete-Recipe/{id}")]
        public async Task<IActionResult> DeleteChefRecipe(int id)
        {
            var recipeToDelete = await _chefConnectDbContext.ChefRecipes.Include(r => r.RecipeCuisine).Where(r => r.ChefRecipesId == id).FirstOrDefaultAsync();

            _chefConnectDbContext.ChefRecipes.Remove(recipeToDelete);
            _chefConnectDbContext.SaveChanges();

            return RedirectToAction("GetMyRecipesAndCuisinesPage", new { username = User.Identity.Name });
        }

        //Get method to navigate to the chef account settings page
        [HttpGet("/{username}/Account-Settings")]
        public async Task<IActionResult> GetChefAccountSettings(string username)
        {
            ChefViewModel model = new ChefViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username)

            };
            Console.WriteLine(model.ActiveUser.UserName);
            Console.WriteLine(model.ActiveUser.Name);

            return View("EditProfile", model);
        }



        //Method to edit chef profile details
        [HttpPost()]
        public async Task<IActionResult> EditChefAccountDetails(ChefViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.ActiveUser.UserName);
                user.Name = model.ActiveUser.Name;
                user.PhoneNumber = model.ActiveUser.PhoneNumber;
                user.UserName = model.ActiveUser.UserName;
                user.Email = model.ActiveUser.Email;
                //user.DateOfBirth = model.ActiveUser.DateOfBirth;
                await _userManager.UpdateAsync(user);


                return RedirectToAction("ChefProfile", new { username = model.ActiveUser.UserName });
            }
            else
            {
                return RedirectToAction("ChefProfile", model);
            }
        }


        // Check this method
        [HttpPost()]
        public async Task<IActionResult> AddCuisineForChefProfile(ChefViewModel model)
        {

            List<ChefCuisines> chefCuisineList = await _chefConnectDbContext.ChefCuisines.Include(cc => cc.Cuisine).Where(c => c.ChefId == model.NewChefCuisine.ChefId).ToListAsync();
            if (isnewChefCuisine(chefCuisineList, model.NewChefCuisine))
            {
                _chefConnectDbContext.ChefCuisines.Add(model.NewChefCuisine);
                _chefConnectDbContext.SaveChanges();
                return RedirectToAction("GetMyRecipesAndCuisinesPage", new { username = User.Identity.Name });

            }
            else
            {
                TempData["error"] = "Cuisine already added to your profile";
                return RedirectToAction("GetMyRecipesAndCuisinesPage", new { username = User.Identity.Name });
            }


        }

        public bool isnewChefCuisine(List<ChefCuisines> ChefCuisine, ChefCuisines item)
        {
            foreach (var it in ChefCuisine)
            {
                if (it.CuisineId == item.CuisineId && it.ChefId == item.ChefId)
                {
                    return false;
                }
            }
            return true;
        }

        //Method to remove the cuisine from the chef profile
        [HttpGet()]
        public async Task<IActionResult> RemoveCuisineFromChefProfile(int cuisineId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var cuisineToDelete = await _chefConnectDbContext.ChefCuisines.Include(cc => cc.Cuisine).Where(c => c.ChefId == user.Id).Where(c => c.CuisineId == cuisineId).FirstOrDefaultAsync();

            _chefConnectDbContext.ChefCuisines.Remove(cuisineToDelete);
            _chefConnectDbContext.SaveChanges();

            return RedirectToAction("GetMyRecipesAndCuisinesPage", new { username = User.Identity.Name });
        }

        

        [HttpGet("{id}/{username}/Add-Recipe")]
        public async Task<IActionResult> AddRecipeToChefProfile(int id, string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var recipe = await _chefConnectDbContext.ChefRecipes.Where(r => r.ChefRecipesId == id).FirstOrDefaultAsync();

            ChefRecipes newRecipe = new ChefRecipes()
            {
                RecipeName = recipe.RecipeName,
                RecipeDescription = recipe.RecipeDescription,
                RecipeImage = recipe.RecipeImage,
                NumberOfPeople = recipe.NumberOfPeople,
                Price = recipe.Price,
                PricePerExtraPerson = recipe.PricePerExtraPerson,
                ChefId = user.Id,
                CuisineId = recipe.CuisineId
            };

            _chefConnectDbContext.ChefRecipes.Add(newRecipe);
            _chefConnectDbContext.SaveChanges();

            return RedirectToAction("GetMyRecipesAndCuisinesPage", new { username = user.UserName });
        }


        public bool isUniquePhoneNumber(string phone)
        {
            var allUsers = _userManager.Users;

            foreach (var user in allUsers)
            {
                if (user.PhoneNumber == phone)
                {
                    return false;
                }
            }

            return true;
        }

    }

}

