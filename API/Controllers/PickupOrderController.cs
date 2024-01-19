using API.Extensions;
using Application.Orders;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class PickupOrderController : BaseApiController
    {
        [HttpPost("PickupOrder/{id}")]
        public async Task<IActionResult> PickupOrder(int id)
        {
            var courierId = HttpContext.GetUserId();
            var courierName = HttpContext.GetUserName();
            int res = await Mediator.Send(new Pickup.Command{OrderId = id, CourierId = courierId});

            switch(res)
            {
                case 0:
                    return Ok($"Order {id} picked up by {courierName}");
                case 1:
                    return BadRequest($"Order {id} doesn't exist in the database");
                case 2:
                    return BadRequest($"Order {id} status doesn't allow for pickup");
                case 3:
                    return BadRequest($"Order {id} belongs to (was taken by) a different courier");
                default:
                    return BadRequest($"Other error");
            }
            
        }
    }
}