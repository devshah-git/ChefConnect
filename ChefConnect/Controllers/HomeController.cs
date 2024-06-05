using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ChefConnect.Models;
using Microsoft.AspNetCore.Authorization;
using ChefConnect.Entities;
using Microsoft.AspNetCore.Identity;
using ChefConnect.Services;

namespace ChefConnect.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _signInManager;
    private readonly IWebHostEnvironment webHostEnvironment;
    private HelperServices HelperServices = new HelperServices();

    public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    
    public IActionResult Index()
    {
        if (_signInManager.IsSignedIn(User))
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
        return View();
    }

   
    [HttpGet("/Login")]
    public async Task<IActionResult> Login()
    {

        return View();
    }

    
    [HttpPost("/Login")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (ModelState.IsValid)
        {
            var userByUserName = await _userManager.FindByNameAsync(loginViewModel.UserName);
            var userByEmail = await _userManager.FindByEmailAsync(loginViewModel.UserName);

            if (userByEmail == null)
            {
                if (userByUserName == null)
                {
                    ViewBag.LogIn = false;
                    ModelState.AddModelError("", "Invalid username/password.");
                    return View(loginViewModel);
                }
                else
                {
                    var result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password,
                     isPersistent: false, lockoutOnFailure: false);
                    
                    if (result.Succeeded)
                    {
                        if (!userByUserName.EmailConfirmed)
                        {
                            TempData["ConfirmEmailMessage"] = $"An email verification is sent to you. Please confirm your email there.";
                        }
                        if (_userManager.IsInRoleAsync(userByUserName, "Chef").Result)
                        {
                            return RedirectToAction("ChefProfile", "Chef", new {username = userByUserName.UserName});
                        }
                        else if(_userManager.IsInRoleAsync(userByUserName, "Admin").Result)
                        {
                            return RedirectToAction("AdminHome", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("GetCustomerHome", "Customer", new { username = userByUserName.UserName });
                        }
                    }
                    else
                    {
                        ViewBag.LogIn = false;
                        ModelState.AddModelError("", "Invalid username/password.");
                        return View(loginViewModel);
                    }
                }
            }
            else
            {
                var result = await _signInManager.PasswordSignInAsync(userByEmail.UserName, loginViewModel.Password,
                     isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (_userManager.IsInRoleAsync(userByEmail, "Chef").Result)
                    {
                        return RedirectToAction("ChefProfile", "Chef", new { username = userByEmail.UserName });
                    } 
                    else
                    {
                        return RedirectToAction("GetCustomerHome", "Customer", new { username = userByEmail.UserName });
                    }
                }
                else
                {
                    ViewBag.LogIn = false;
                    ModelState.AddModelError("", "Invalid username/password.");
                    return View(loginViewModel);
                }
            }
        }
        else
        {
            return View();
        }
        
    }

    
    [HttpGet("/Register")]
    public async Task<IActionResult> SelectRegistration()
    {
        return View();
    }

    [Authorize(Roles = "Chef, Customer")]
    [HttpGet("/{username}/ResendVerification")]
    public async Task<IActionResult> ResendVerificationMail(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        var message = $"\nHi,\n\nThanks for getting started with ChefConnect!\n\nWe need a little more information to complete your registration, including a confirmation of your email address.\n\nClick below to confirm your email address:\n\nhttps://localhost:7042/{user.UserName}/Email-Verification-Success\n\nIf you have problems, please paste the above URL into your web browser.";
        HelperServices.SendEmailAsync(user.Email, "Email Verification", message);
        TempData["EmailReSentMessage"] = "A new verification mail has been sent your mail, please confirm your email there.";

        return RedirectToAction("ChefProfile","Chef",new { username = user.UserName });
    }

    [HttpGet("/{username}/Email-Verification-Success")]
    public async Task<IActionResult> EmailVerificationSuccess(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);
        
        return View();
    }

    [Authorize]
    [HttpGet("/Logout")]
    public async Task<IActionResult> Logout()
    {
     
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }

    [HttpGet("/Forgot-Password")]
    public async Task<IActionResult> GetForgotPasswordPage()
    {
        return View("ForgotPassword");
    }

    [HttpPost("/No-User-Found")]
    public async Task<IActionResult> SendForgotPasswordLink(IFormCollection form)
    {
        var user = await _userManager.FindByEmailAsync(form["emailinput"]);
        if (user != null)
        {
            var message = $"\nHi,\n\nPlease click on the link below to reset your password:\n\nhttps://localhost:7042/{user.UserName}/Reset-Password\n\nIf you have problems, please paste the above URL into your web browser.";
             HelperServices.SendEmailAsync(user.Email, "Password Reset", message);
            return RedirectToAction("Login");
        }
        else
        {
            ModelState.AddModelError("email", "No user was found with this email");
            return View("ForgotPassword");
        }
        
    }

    [HttpGet("/{username}/Reset-Password")]
    public async Task<IActionResult> GetResetPasswordPage(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        return View("ResetPassword", user);
    }

    [HttpPost("/Reset-Password-Error")]
    public async Task<IActionResult> ChangeUserPassword(IFormCollection form)
    {
        var password = form["password"];
        var confirmPassword = form["cnfrmpassword"];
        var user = await _userManager.FindByIdAsync(form["userid"]);
        if (password != confirmPassword)
        {
            TempData["resetPasswordError"] = "Password and Confirm Password should be same.";
            return View("ResetPassword", user);
        }
        else
        {
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, password);
            await _signInManager.SignOutAsync();
            return View("Login");
        }
    }
    
    public IActionResult Privacy()
    {
        return View();
    }

    
    public IActionResult Highlights()
    {
        return View();
    }

    
    public IActionResult About(string id)
    {

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

