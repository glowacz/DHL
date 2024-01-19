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
            await Mediator.Send(new Pickup.Command{OrderId = id, CourierId = courierId});
            return Ok($"Order {id} picked up");
        }
    }
}