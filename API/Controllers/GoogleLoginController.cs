using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Services;
using Domain;
using Domain.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    // [Route("api/[controller]")]
    [Route("Auth")]
    public class GoogleLoginController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration config;

        public GoogleLoginController(IConfiguration config)
        {
            this.config = config;
            _httpClient = new HttpClient();
        }

        [AllowAnonymous]
        //[HttpGet("login-google")] // Zmiana endpointu dla logowania z Google
        [HttpGet("login-google")] // Zmiana endpointu dla logowania z Google
        public async Task<IActionResult> LoginWithGoogle()
        {
            var props = new AuthenticationProperties { RedirectUri = "Auth/google-callback" };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            //var response = await  _httpClient.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var response = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (response.Principal == null) return BadRequest();

            var name = response.Principal.FindFirstValue(ClaimTypes.Name);
            var givenName = response.Principal.FindFirstValue(ClaimTypes.GivenName);
            var email = response.Principal.FindFirstValue(ClaimTypes.Email);

            return Redirect("https://localhost:3001");
        }
        [Authorize]
        [HttpGet("logout-google")]
        public async Task<IActionResult> LogoutFromGoogle()
        {
            await HttpContext.SignOutAsync();
            //  var callbackUrl = Url.Action("GoogleLogoutCallback", "Auth", values: null, protocol: Request.Scheme);
            //  var logoutUrl = $"https://accounts.google.com/o/oauth2/logout?client_id=484929956573-66ordm7uo4ug3i1oerd7p4992idsq2mh.apps.googleusercontent.com&redirect_uri={callbackUrl}";

            return Redirect("https://localhost:3001");
        }

        [HttpGet("get-current-user")]
        public async Task<ActionResult<string>> GetCurrentUser()
        {
            var user = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            return user;
        }
    }
}