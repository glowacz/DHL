using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class Seed
    {

        public static async Task ClearDatabase(DataContext context)
        {
            Console.WriteLine("Clearing data---------------------------------------------------------------------------");
            var _context = context;

            var allOrders = _context.Orders.ToList();
            _context.Orders.RemoveRange(allOrders);

            var allAddresses = _context.Addresses.ToList();
            _context.Addresses.RemoveRange(allAddresses);

            _context.SaveChanges();
        }

        //public static async Task ClearUsers(UserManager<IdentityUser> userManager)
        public static async Task ClearUsers(UserManager<AppUser> userManager)
        {
            // List<AppUser> users = new List<AppUser>(userManager.Users);
            List<AppUser> users = [.. userManager.Users];
            //List<IdentityUser> users = [.. userManager.Users];

            foreach(var user in users)
            {
                await userManager.DeleteAsync(user);
            }
        }

        //public static async Task SeedUsers(UserManager<IdentityUser> userManager)
        public static async Task SeedUsers(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser{UserName = "Glowacz", Name="Glowacz", Email = "c1@test.com",
                    Id = "105119251730054942410"},
                    new AppUser{UserName = "Pedro", Name="Pedro", Email = "c2@test.com"},
                    new AppUser{UserName = "Vlada", Name="Vlada", Email = "c3@test.com"},
                };

                foreach(var user in users)
                {
                    var res = await userManager.CreateAsync(user, "pswrd");
                    var cnt = userManager.Users.Count();
                    Console.WriteLine(cnt);
                }
            }
        }

        public static async Task SeedDatabase(DataContext context)
        {
            if (!context.Set<Order>().Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    var order = new Order
                    {
                        Width = 10 + i,
                        Height = 20 + i,
                        Weight = 5 + i,
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                        SourceAddress = new Address
                        {
                            StreetName = $"{i + 1} Main St",
                            StreetNo = i + 1,
                            ZipCode = $"{10000 + i}",
                            City = $"City {i + 1}"
                        },
                        DestinationAddress = new Address
                        {
                            StreetName = $"{i + 1} Second St",
                            StreetNo = i + 2,
                            ZipCode = $"{20000 + i}",
                            City = $"City {i + 2}"
                        },
                        Status = i % 3 == 0 ? 1 : 0,
                        Email = "pgenius100@gmail.com",
                        //CourierId = context.Users.First().Id,
                        CourierId = string.Empty
                        //CourierID = 0
                    };

                    context.Set<Order>().Add(order);
                }

                context.SaveChanges();
            }
        }
        //public static async Task SeedMain(DataContext context)
        public static async Task SeedMain(DataContext context, UserManager<AppUser> userManager)
        {
            await ClearUsers(userManager);
            await SeedUsers(userManager);

            await ClearDatabase(context);
            await SeedDatabase(context);
        }
    }
}
