using API.Extensions;
using Application.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliverOrderController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> HandleRequest(int id)
        {
            var courierEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var courierName = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            int res = await Mediator.Send(new Deliver.Command{OrderId = id, CourierEmail = courierEmail});

            switch (res)
            {
                case 0:
                    return Ok($"Order {id} delivered by {courierName}");
                case 1:
                    return BadRequest($"Order {id} doesn't exist in the database");
                case 2:
                    return BadRequest($"Order {id} status doesn't allow for delivery");
                case 3:
                    return BadRequest($"Order {id} belongs to (was taken by) a different courier");
                default:
                    return BadRequest($"Other error");
            }
        }
    }
}