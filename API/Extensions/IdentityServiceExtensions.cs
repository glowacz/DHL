using System.Text;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services,
            IConfiguration config)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super secret keysuper secret keysuper secret keysuper secret key"));

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                //options.LoginPath = "/account/google-login";
                options.LoginPath = "/Auth/login-google";
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    }
                };
            })
           .AddGoogle( options =>
           {
               options.ClientId = config["Authentication:Google:ClientId"];
               options.ClientSecret = config["Authentication:Google:ClientSecret"];
               //   options.Events.OnRedirectToAuthorizationEndpoint
               options.Events.OnRedirectToAuthorizationEndpoint = context =>
               {
                   context.Response.Redirect(context.RedirectUri + "&prompt=consent");
                   return Task.CompletedTask;
               };
               options.Events.OnRemoteFailure = context =>
               {
                   context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Ustawienie statusu 401 (brak autoryzacji)
                   return Task.CompletedTask;
               };

           });

            return services;
        }
    }
}