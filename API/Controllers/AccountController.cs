using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        public UserManager<IdentityUser> UserManager { get; }
        public AccountController(UserManager<IdentityUser> userManager)
        {
            this.UserManager = userManager;
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await UserManager.FindByEmailAsync(loginDTO.Email);

            if (user == null) return Unauthorized();

            var result = await UserManager.CheckPasswordAsync(user, loginDTO.Password);

            if (result)
            {
                return new UserDTO
                {
                    Token = "this will be a token",
                };
            }
            
            return Unauthorized();
        }
    }
}