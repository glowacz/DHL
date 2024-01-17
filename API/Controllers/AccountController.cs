using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using Domain;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        public UserManager<AppUser> UserManager { get; }
        private readonly TokenService _tokenService;
        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _tokenService = tokenService;
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
                    Token = _tokenService.CreateToken(user),
                    Name = user.Name
                };
            }
            
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            // if(await UserManager.Users.AnyAsync(x => x.UserName == registerDTO.))

            var user = new AppUser
            {
                  Name = registerDTO.Name,
                  UserName = registerDTO.Name,
                  Email = registerDTO.Email,
                  Role = registerDTO.Role,
            };

            var result = await UserManager.CreateAsync(user, registerDTO.Password);

            if(result.Succeeded)
            {
                return new UserDTO
                {
                    Name = user.Name,
                    Role = user.Role,
                    Token = _tokenService.CreateToken(user),
                };
            }

            return BadRequest("Problem registering user (possibly duplicate email)");
        }
    }
}