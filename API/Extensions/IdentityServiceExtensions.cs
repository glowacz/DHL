using System.Text;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication;
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //ValidateIssuerSigningKey = true,
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                })
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = config["Authentication:Google:ClientId"];
                    googleOptions.ClientSecret = config["Authentication:Google:ClientSecret"];
                }); ;

            //services.AddDefaultIdentity<AppUser>(opt => opt.SignIn.RequireConfirmedAccount = true)
            
            services.AddDefaultIdentity<AppUser>(opt =>
            {
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 5;
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<DataContext>();

            //services.AddIdentityCore<AppUser>(opt =>
            //{
            //    opt.Password.RequireNonAlphanumeric = false;
            //    opt.Password.RequireDigit = false;
            //    opt.Password.RequireUppercase = false;
            //    opt.Password.RequiredLength = 5;
            //    opt.User.RequireUniqueEmail = true;
            //})
            //.AddEntityFrameworkStores<DataContext>();

            // services.AddIdentityCore<AppUser>(opt => opt.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores< DataContext>();



            services.AddScoped<TokenService>();

            return services;
        }
    }
}