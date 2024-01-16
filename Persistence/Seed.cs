using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class Seed
    {

        public static async Task ClearData(DataContext context)
        {
            Console.WriteLine("Clearing data---------------------------------------------------------------------------");
            var _context = context;

            var allOrders = _context.Orders.ToList();
            _context.Orders.RemoveRange(allOrders);

            var allAddresses = _context.Addresses.ToList();
            _context.Addresses.RemoveRange(allAddresses);

            _context.SaveChanges();
        }
        public static async Task SeedData(DbContext context, UserManager<AppUser> userManager)
        {
            // foreach(var user in userManager.Users)
            // {
            //     await userManager.DeleteAsync(user);
            // }

            if(!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser{UserName = "Głowacz", Name="Głowacz", Email = "c1@test.com"},
                    new AppUser{UserName = "Pedro", Name="Pedro", Email = "c2@test.com"},
                };

                foreach(var user in users)
                {
                    await userManager.CreateAsync(user, "pswrd");
                }
            }

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
                        CourierID = 0
                    };

                    context.Set<Order>().Add(order);
                }

                context.SaveChanges();
            }
        }
    }
}
