using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChefConnect.Data;
using ChefConnect.Entities;
using ChefConnect.Models;
using ChefConnect.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChefConnect.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private HelperServices _helperServices = new HelperServices();
        private ChefConnectDbContext _dbcontext;

        public CustomerController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ChefConnectDbContext dbcontext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbcontext = dbcontext;
        }

        [AllowAnonymous]
        [HttpGet("/Register/AsCustomer")]
        public async Task<IActionResult> CustomerRegister()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("/Register/AsCustomer")]
        public async Task<IActionResult> CustomerRegister(RegisterCustomerViewModel registerViewModel)
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

                        await _userManager.AddToRoleAsync(user, "Customer");
                        user.Email = registerViewModel.Email;
                        user.PhoneNumber = registerViewModel.PhoneNumber;
                        await _userManager.UpdateAsync(user);
                        await _signInManager.SignInAsync(user, false);
                        var message = $"\nHi,\n\nThanks for getting started with ChefConnect!\n\nWe need a little more information to complete your registration, including a confirmation of your email address.\n\nClick below to confirm your email address:\n\nhttps://localhost:7042/{registerViewModel.UserName}/Email-Verification-Success\n\nIf you have problems, please paste the above URL into your web browser.";
                        _helperServices.SendEmailAsync(registerViewModel.Email, "Email Verification", message);
                        if (!user.EmailConfirmed)
                        {
                            TempData["ConfirmEmailMessage"] = $"An email verification is sent to you. Please confirm your email there.";
                        }

                        return RedirectToAction("GetCustomerHome", new { username = registerViewModel.UserName });


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

        [HttpGet("/{username}/Home")]
        public async Task<IActionResult> GetCustomerHome(string username)
        {
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                AllRecipes = await _dbcontext.ChefRecipes.Include(r => r.RecipeCuisine).Include(r => r.Chef).Include(r => r.Reviews).ToListAsync(),
                FiveStarRecipeList = await _dbcontext.ChefRecipes.Include(r => r.RecipeCuisine).Include(r => r.Chef).Include(r=>r.Reviews).Where(r => r.Reviews.Average(o=>o.Ratings) >= 4).OrderByDescending(od=>od.Reviews.Average(o => o.Ratings) >= 4).ToListAsync()
            };
            //Console.WriteLine(model.FiveStarRecipeList.Count);
            return View("CustomerHome", model);
        }

        [HttpGet("/{username}/Search")]
        public async Task<IActionResult> GetSearchPage(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = user,
                CuisinesList = await _dbcontext.Cuisines.ToListAsync(),
                AllRecipes = new List<ChefRecipes>(),
                ChefsList = new List<AppUser>()
            };
            return View("CustomerSearch", model);
        }

        [HttpGet("/{name}/Recipes")]
        public async Task<IActionResult> GetRecipesFromCuisine(string name)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var recipes = await _dbcontext.ChefRecipes.Include(r => r.Chef).Include(r => r.RecipeCuisine).Where(r => r.RecipeCuisine.CuisineName == name).ToListAsync();
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = user,
                CuisinesList = await _dbcontext.Cuisines.ToListAsync(),
                AllRecipes = recipes,
                ChefsList = new List<AppUser>()
            };
            return View("CustomerSearch", model);
        }

        [HttpGet("/{username}/Customer-Profile")]
        public async Task<IActionResult> GetCustomerAccountSettings(string username)
        {
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username)
            };

            return View("AccountSettings", model);
        }

        [HttpPost()]
        public async Task<IActionResult> EditCustomerAccountDetails(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.ActiveUser.UserName);
                user.Name = model.ActiveUser.Name;
                user.PhoneNumber = model.ActiveUser.PhoneNumber;
                user.UserName = model.ActiveUser.UserName;
                user.Email = model.ActiveUser.Email;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("GetCustomerHome", new { username = model.ActiveUser.UserName });
            }
            else
            {
                return View("AccountSettings", model);
            }
        }

        [HttpGet("/{username}/Cart")]
        public async Task<IActionResult> GetCustomerCart(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            List<UserCartItem> _cartList = await _dbcontext.UserCartItems.Include(o => o.ChefRecipe).Where(o => o.CustomerId == user.Id).ToListAsync();

            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = user,
                cartList = _cartList,
                TimeSlots = await _dbcontext.TimeSlots.ToListAsync()

            };
            return View("CustomerCart", model);
        }

        //Get Method to navigate to All Addresses page
        [HttpGet("/{username}/All-Addresses")]
        public async Task<IActionResult> GetAllAddresses(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var addressList = await _dbcontext.Addresses.Where(a => a.CustomerId == user.Id).ToListAsync();
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = user,
                addressList = addressList
            };
            return View("CustomerManageAddresses", model);
        }

        //Get the customer to address page
        [HttpGet("/{username}/Address/{returnurl}")]
        public async Task<IActionResult> GetCustomerAddressPage(string username, string returnurl)
        {
            var user = await _userManager.FindByNameAsync(username);
            TempData["returnurl"] = returnurl;
            AddressViewModel model = new AddressViewModel()
            {
                Username = user.UserName,
                ReturnUrl = returnurl
            };

            return View("CustomerAddress", model);
        }


        //Post Method for Customer to add address
        [HttpPost()]
        public async Task<IActionResult> AddCustomerAddress(AddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                var returnurl = model.ReturnUrl;
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                Addresses address = new Addresses()
                {
                    Name = model.Name,
                    AptNumber = model.AptNumber,
                    StreetAddress = model.StreetAddress,
                    City = model.City,
                    Province = model.Province,
                    Country = model.Country,
                    PostalCode = model.PostalCode,
                    PhoneNumber = model.PhoneNumber,
                    CustomerId = user.Id
                };
                _dbcontext.Addresses.Add(address);
                _dbcontext.SaveChanges();
                if (returnurl == "manageaddress")
                {
                    return RedirectToAction("GetAllAddresses", new { username = user.UserName });
                }
                else
                {
                    return RedirectToAction("GetSecureCheckoutPage", new { username = user.UserName });
                }
                
            }
            else
            {
                return View("CustomerAddress", model);
            }
        }

        //Get method for customer to manage payment methods
        [HttpGet("/{username}/Manage-Payment")]
        public async Task<IActionResult> GetManagePaymentMethods(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var paymentMethods = await _dbcontext.PaymentMethods.Where(p => p.CustomerId == user.Id).ToListAsync();
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = user,
                PaymentMethodsList = paymentMethods
            };
            return View("CustomerManagePayment", model);
        }


        //Get Method for Customer to add payment method
        [HttpGet("/{username}/Add-Payment/{returnurl}")]
        public async Task<IActionResult> GetAddPaymentMethodPage(string username, string returnurl)
        {
            var user = await _userManager.FindByNameAsync(username);
            PaymentViewModel model = new PaymentViewModel()
            {
                Username = user.UserName,
                ReturnUrl = returnurl
            };
            TempData["returnurl"] = returnurl;
            return View("CustomerAddPayment", model);
        }


        //Add Customer payment method
        [HttpPost()]
        public async Task<IActionResult> AddCustomerPaymentMethod(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var returnurl = model.ReturnUrl;
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                PaymentMethods paymentMethod = new PaymentMethods()
                {
                    PaymentType = model.PaymentType,
                    NameOnCard = model.NameOnCard,
                    CardNumber = model.CardNumber,
                    CardCvv = model.CardCvv,
                    CardExpiry = model.CardExpiry,
                    CustomerId = user.Id
                };
                _dbcontext.PaymentMethods.Add(paymentMethod);
                _dbcontext.SaveChanges();
                if (returnurl == "managepayment")
                {
                    return RedirectToAction("GetManagePaymentMethods", new { username = user.UserName });
                }
                else
                {
                    return RedirectToAction("GetSecureCheckoutPage", new { username = user.UserName });
                }
            }
            else
            {
                return View("CustomerAddPayment", model);
            }
        }

        //Get Method for Customer to add to cart feature
        [HttpGet("/{username}/{id}/Cart")]
        public async Task<IActionResult> AddRecipeToCart(string username, int id)
        {

            var user = await _userManager.FindByNameAsync(username);
            var recipe = await _dbcontext.ChefRecipes.Include(r => r.RecipeCuisine).Where(r => r.ChefRecipesId == id).FirstOrDefaultAsync();
            List<UserCartItem> _cartList = await _dbcontext.UserCartItems.Include(o => o.ChefRecipe).Where(o => o.CustomerId == user.Id).ToListAsync();


            UserCartItem item = new UserCartItem()
            {

                RecipeId = recipe.ChefRecipesId,
                CustomerId = user.Id,
                TimeSlotId = 1
            };

            if (_cartList.Count == 0)
            {

                _dbcontext.UserCartItems.Add(item);
                _dbcontext.SaveChanges();
            }
            else if (isNewCartItem(_cartList, item))
            {
                _dbcontext.UserCartItems.Add(item);
                _dbcontext.SaveChanges();


            }

            return RedirectToAction("GetCustomerCart", new { username = user.UserName });
        }

        //Method to update checkout page with cart items
        [HttpPost()]
        public async Task<IActionResult> UpdateCartPage(IFormCollection form)
        {
            int id = int.Parse(form["itemId"]);
            var timeSlotId = form["timeSlotId"];
            var extraPeople = form["GuestQuantity"];
            var date = form["OrderDate"];
            Console.WriteLine(date);
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userCartItem = await _dbcontext.UserCartItems.Include(uc => uc.ChefRecipe).Where(o => o.UserCartItemId == id).FirstOrDefaultAsync();
            userCartItem.TimeSlotId = Convert.ToInt32(timeSlotId);
            userCartItem.OrderDate = Convert.ToDateTime(date);
            userCartItem.GuestQuantity = Convert.ToInt32(extraPeople);
            // userCartItem.RecipeTotal = (userCartItem.ChefRecipe.Price + ((userCartItem.GuestQuantity - userCartItem.ChefRecipe.NumberOfPeople) * userCartItem.ChefRecipe.PricePerExtraPerson));
            userCartItem.RecipeTotal = userCartItem.ChefRecipe.Price + (userCartItem.GuestQuantity * userCartItem.ChefRecipe.PricePerExtraPerson);
            _dbcontext.UserCartItems.Update(userCartItem);
            _dbcontext.SaveChanges();
            return RedirectToAction("GetCustomerCart", new { username = user.UserName });
        }

        [HttpGet()]
        public async Task<IActionResult> RemoveItemFromCart(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var cartItem = await _dbcontext.UserCartItems.Where(u => u.UserCartItemId == id).FirstOrDefaultAsync();

            _dbcontext.UserCartItems.Remove(cartItem);
            _dbcontext.SaveChanges();

            return RedirectToAction("GetCustomerCart", new { username = user.UserName });
        }

        //Navigate to Secure Checkout page
        [HttpGet("/{username}/Checkout")]
        public async Task<IActionResult> GetSecureCheckoutPage(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var cartlist = await _dbcontext.UserCartItems.Include(o => o.ChefRecipe).Where(o => o.CustomerId == user.Id).Where(ucr => ucr.GuestQuantity != null).ToListAsync();
            if (cartlist.Count == 0)
            {
                TempData["carterror"] = "Please save atleast one recipe to proceed to checkout";
                return RedirectToAction("GetCustomerCart", new { username = user.UserName });
            }
            else
            {
                CustomerViewModel model = new CustomerViewModel()
                {
                    ActiveUser = user,
                    cartList = cartlist,
                    addressList = await _dbcontext.Addresses.Where(a => a.CustomerId == user.Id).ToListAsync(),
                    PaymentMethodsList = await _dbcontext.PaymentMethods.Where(p => p.CustomerId == user.Id).ToListAsync()

                };
                return View("CustomerCheckout", model);
            }
           
        }

        [HttpPost("/Secure-Checkout")]
        public async Task<IActionResult> SecureCheckout(IFormCollection form)
        {

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var cartList = await _dbcontext.UserCartItems.Include(o => o.ChefRecipe).Where(o => o.GuestQuantity != null).Where(o => o.CustomerId == user.Id).ToListAsync();
            var subtotal = cartList.Sum(x => x.RecipeTotal);
            var tax = cartList.Sum(x => x.RecipeTotal) * 0.13;
            var charges = cartList.Sum(x => x.RecipeTotal) * 0.02;
            var total = subtotal + tax + charges;
            var order = new OrderDetails()
            {
                CustomerId = user.Id,
                OrderInstructions = form["orderinstructions"],
                OrderSubTotal = (double)subtotal,
                OrderTax = (double)tax,
                Charges = (double)charges,
                OrderTotal = (double)total,
                paymentMethodId = int.Parse(form["selectPayment"]),
                addressId = int.Parse(form["selectAddress"])
            };
            _dbcontext.OrderDetails.Add(order);
            _dbcontext.SaveChanges();

            foreach (var item in cartList)
            {
                var orderItem = new OrderRecipes()
                {
                    OrderDetailsId = order.OrderDetailsId,
                    ChefRecipesId = item.RecipeId,
                    GuestQuantity = (int)item.GuestQuantity,
                    TimeSlotId = (int)item.TimeSlotId,
                    OrderDate = (DateTime)item.OrderDate,
                    RecipeTotal = (double)item.RecipeTotal


                };
                _dbcontext.OrderRecipes.Add(orderItem);
                _dbcontext.UserCartItems.Remove(item);
                _dbcontext.SaveChanges();
            }


            return RedirectToAction("GetCustomerHome", new { username = user.UserName });



        }

        [HttpGet("/{username}/{id}/Add-Review")]
        public async Task<IActionResult> GetReviewsPage(string username, int id)
        {
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                ActiveRecipe = await _dbcontext.ChefRecipes.Include(r => r.RecipeCuisine).Where(r => r.ChefRecipesId == id).FirstOrDefaultAsync(),
                NewReview = new Reviews()
            };

            return View("CustomerAddReview", model);
        }

        [HttpPost()]
        public async Task<IActionResult> AddChefReview(CustomerViewModel model)
        {
            if (ModelState.IsValid)
            {
                _dbcontext.Reviews.Add(model.NewReview);
                _dbcontext.SaveChanges();
                return RedirectToAction("GetCustomerHome", new { username = User.Identity.Name });
            }
            else
            {
                return View("CustomerAddReview", model);
            }
        }


        //CUstomer search a recipe
        [HttpPost()]
        public async Task<IActionResult> GetSearchedRecipe(IFormCollection _form)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var searchString = _form["search"];


            var recipes = await _dbcontext.ChefRecipes.Include(r => r.RecipeCuisine).Include(r => r.Chef).Where(r => r.RecipeName.Contains(searchString)).ToListAsync();
            
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = user,
                CuisinesList = await _dbcontext.Cuisines.ToListAsync(),
                AllRecipes = recipes,
                
            };

            return View("CustomerSearch", model);
        }

        [HttpGet("/{username}/Orders")]
        public async Task<IActionResult> GetOrdersPage(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                UpComingOrders = new List<OrderRecipes>(),
                PastOrders = new List<OrderRecipes>()
            };
            return View("CustomerOrders", model);
        }

        [HttpGet("/{username}/Upcoming-Orders")]
        public async Task<IActionResult> GetUpcomingOrders(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                UpComingOrders = await _dbcontext.OrderRecipes.Include(r => r.TimeSlot).Include(r => r.OrderDetails).ThenInclude(od => od.Customer).Include(r => r.OrderDetails).ThenInclude(od => od.Address).Include(o => o.ChefRecipes).ThenInclude(r => r.Chef).Where(r => r.OrderDetails.CustomerId == user.Id).Where(r => r.OrderDate > DateTime.Now).ToListAsync(),
                PastOrders = new List<OrderRecipes>()

            };
            return View("CustomerOrders", model);
        }

        [HttpGet("/{username}/Past-Orders")]
        public async Task<IActionResult> GetPastOrders(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            CustomerViewModel model = new CustomerViewModel()
            {
                ActiveUser = await _userManager.FindByNameAsync(username),
                UpComingOrders = new List<OrderRecipes>(),
                PastOrders = await _dbcontext.OrderRecipes.Include(r => r.TimeSlot).Include(r => r.OrderDetails).ThenInclude(od => od.Customer).Include(r => r.OrderDetails).ThenInclude(od => od.Address).Include(o => o.ChefRecipes).ThenInclude(r => r.Chef).Where(r => r.OrderDetails.CustomerId == user.Id).Where(r => r.OrderDate < DateTime.Now).ToListAsync()

            };
            return View("CustomerOrders", model);
        }

        [HttpGet("/{orderid}/{recipeid}/Cancel-Booking")]
        public async Task<IActionResult> CancelUpcomingBooking(int orderid, int recipeid)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var orderToDelete = await _dbcontext.OrderRecipes.Where(or => or.OrderDetailsId == orderid).Where(or => or.ChefRecipesId == recipeid).FirstOrDefaultAsync();
            _dbcontext.OrderRecipes.Remove(orderToDelete);
            _dbcontext.SaveChanges();

            return RedirectToAction("GetUpcomingOrders", new { username = user.UserName });
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


        public bool isNewCartItem(List<UserCartItem> cartList, UserCartItem item)
        {
            foreach (var cartItem in cartList)
            {
                if (cartItem.RecipeId == item.RecipeId)
                {
                    return false;
                }
            }
            return true;
        }

    }
}

