using API.Extensions;
using Application.Orders;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    // [Route("api/[controller]")]
    public class TakeOrderController : BaseApiController
    {
        //[HttpPost("TakeOrder/{id}")]
        [HttpGet("TakeOrder/{id}")]
        //public async Task<IActionResult> TakeOrder(int id, [FromBody] CourierActionDTO body)
        public async Task<IActionResult> TakeOrder(int id)
        {
            //var courierId = HttpContext.GetUserId();
            var courierEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var courierName = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            //if(courierId == string.Empty) return Unauthorized();
            //await Mediator.Send(new Take.Command{OrderId = id, CourierID = body.CourierId});
            int res = await Mediator.Send(new Take.Command { OrderId = id, CourierEmail = courierEmail});

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