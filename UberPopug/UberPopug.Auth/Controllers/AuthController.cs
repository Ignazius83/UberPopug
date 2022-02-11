using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using UberPopug.Auth.Commands;
using UberPopug.Auth.Models;
using UberPopug.Auth.ViewModels;

namespace UberPopug.Auth.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;       

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,           
            IIdentityServerInteractionService interactionService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;           
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();

            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);

            if (string.IsNullOrEmpty(logoutRequest.PostLogoutRedirectUri))
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(logoutRequest.PostLogoutRedirectUri);
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            var externalProviders = await _signInManager.GetExternalAuthenticationSchemesAsync();           
            return View(new LoginViewModel { ReturnUrl = returnUrl, ExternalProviders = externalProviders });
           
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (result.Succeeded)
                return Redirect(model.ReturnUrl);
           
            return View(model);          
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {           
            return View( new RegisterViewModel { ReturnUrl  = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new User();
            user.UserName = model.Username;
            user.public_id = Guid.NewGuid().ToString("D");

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return View(model);

            //send Event            

            return Redirect(model.ReturnUrl);
        }
      
    } 
}
