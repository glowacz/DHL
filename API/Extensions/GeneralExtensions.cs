using Microsoft.AspNetCore.Authentication.Google;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            if(httpContext.User == null)
            {
                return string.Empty;
            }

            //return httpContext.User.Claims.Single(x => x.Type == "sub").Value;
            return httpContext.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public static string GetUserSub(this HttpContext httpContext)
        {
            string bearerToken = httpContext.Request.Headers["Authorization"].ToString();

            // Remove the "Bearer " prefix to get only the token
            if (bearerToken.StartsWith("Bearer "))
            {
                bearerToken = bearerToken.Substring("Bearer ".Length);
            }
            // Replace 'your_google_client_secret' with your actual Google client secret
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(bearerToken) as JwtSecurityToken;

            if (jsonToken == null)
            {
                throw new ArgumentException("Invalid Bearer token format");
            }

            // The 'sub' claim represents the subject (user ID)
            string subClaim = jsonToken.Subject;

            return subClaim;
        }

        public static string GetUserName(this HttpContext httpContext)
        {
            if (httpContext.User == null)
            {
                return string.Empty;
            }

            return httpContext.User.Claims.Single(x => x.Type == ClaimTypes.Name).Value;
        }
    }
}
