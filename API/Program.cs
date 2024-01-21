using API.Extensions;
using Application.Mapping;
using Application.Offers;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddTransient<IEmailSender, EmailSender>();

services.AddControllers();
services.AddControllers(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddIdentityServices(builder.Configuration); // add Google auth here (doesnt work)

services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetOffer.Handler).Assembly));

services.AddCors(opt => {
    opt.AddPolicy("CorsPolicy", policy => {
        policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3001");
    });
});

//services.AddDefaultIdentity<AppUser>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization(); // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

app.MapControllers();

using var scope = app.Services.CreateScope();
var services1 = scope.ServiceProvider;

try
{
    var context = services1.GetRequiredService<DataContext>();
    var userManager = services1.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();
    Console.WriteLine("After migration---------------------------------------------------------------------------");
    await Seed.SeedMain(context, userManager);
    //await Seed.SeedMain(context);
    Mapping._context = context;
    Mapping.Configure();
}
catch (Exception ex)
{
    var logger = services1.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "an error occured during migration");
    throw;
}

app.Run();
