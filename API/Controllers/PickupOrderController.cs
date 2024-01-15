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
            await Mediator.Send(new Pickup.Command{OrderId = id});
            return Ok($"Order {id} picked up");
        }
    }
}