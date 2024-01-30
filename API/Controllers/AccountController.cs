using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Extensions;
using API.Services;
using Domain;
using Domain.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    //[Route("Identity/[controller]")]
    [Route("[controller]"), AllowAnonymous]
    //[Route("account"), AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            this._userManager = userManager;

        }

        [AllowAnonymous]
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [AllowAnonymous]
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault()
                .Claims.Select(claim => new
                {
                    claim.Issuer,
                    claim.OriginalIssuer,
                    claim.Type,
                    claim.Value
                });
            return Redirect("https://localhost:3001");
            //return new JsonResult(claims);
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public async Task<ActionResult<UserDTO>> Login()
        {
            var courierId = HttpContext.GetUserSub();

            var user = await _userManager.FindByIdAsync(courierId);

            if (user == null) return Unauthorized();

            //var result = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            return CreateUserObject(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            // if(await _userManager.Users.AnyAsync(x => x.UserName == registerDTO.))

            var user = new AppUser
            {
                Name = registerDTO.Name,
                UserName = registerDTO.Name,
                Email = registerDTO.Email,
                Role = registerDTO.Role,
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            // return BadRequest("Problem registering user (possibly duplicate email)");
            return BadRequest(result.Errors);
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            return CreateUserObject(user);
        }

        private UserDTO CreateUserObject(AppUser user)
        {
            return new UserDTO
            {
                Name = user.Name,
                Role = user.Role,
                Token = _tokenService.CreateToken(user),
            };
        }
    }
}