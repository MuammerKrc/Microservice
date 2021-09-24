using Course.Client1.Models;
using Course.Client1.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Controllers
{
    public class AuthController : Controller
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult Signin()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signin(SigninInput signinInput)
        {
            if(!ModelState.IsValid)
            {
                return View(signinInput);
            }
            var result =await _identityService.SignIn(signinInput);
            if(!result.IsSuccess)
            {
                if(result.Errors.Any())
                {
                    result.Errors.ForEach(x => ModelState.AddModelError("", x));
                }
            }
            return result.IsSuccess ? RedirectToAction("Index", "Home") : View(signinInput);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await _identityService.RevokeRefreshToken();
            return RedirectToAction("Index", "Home");
        }
    }
}
