using API.Extensions;
using Application.Orders;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    // [Route("api/[controller]")]
    public class TakeOrderController : BaseApiController
    {
        // [HttpPost("{id}")]
        [HttpPost("TakeOrder/{id}")]
        public async Task<IActionResult> TakeOrder(int id, [FromBody] CourierActionDTO body)
        {
            var courierId = HttpContext.GetUserId();
            if(courierId == string.Empty) return Unauthorized();
            //await Mediator.Send(new Take.Command{OrderId = id, CourierID = body.CourierId});
            await Mediator.Send(new Take.Command { OrderId = id, CourierID = courierId});
            return Ok($"Order {id} taken by courier {body.CourierId}");
        }
    }
}