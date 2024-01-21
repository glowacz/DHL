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
            //var courierName = HttpContext.GetUserName();
            var courierName = "szymo";
            if(courierId == string.Empty) return Unauthorized();
            //await Mediator.Send(new Take.Command{OrderId = id, CourierID = body.CourierId});
            int res = await Mediator.Send(new Take.Command { OrderId = id, CourierID = courierId});

            switch (res)
            {
                case 0:
                    return Ok($"Order {id} taken by {courierName}");
                case 1:
                    return BadRequest($"Order {id} doesn't exist in the database");
                case 2:
                    return BadRequest($"Order {id} status doesn't allow for taking");
                default:
                    return BadRequest($"Other error");
            }
        }
    }
}