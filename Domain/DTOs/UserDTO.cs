using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class UserDTO
    {
        // public string Email { get; set; }
        // public string Password { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
    }
}