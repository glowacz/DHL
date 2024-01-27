using API.Extensions;
using Application.Orders;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CannotDeliverOrderController : BaseApiController
    {
        [HttpPost("{id}")]
        public async Task<IActionResult> HandleRequest(int id, [FromBody] CannotDeliverDTO data)
        // public async Task<IActionResult> HandleRequest(int id)
        {
            var courierEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var courierName = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            int res = await Mediator.Send(new CannotDeliver.Query{OrderId = id, 
                Name = data.Name, Reason = data.Reason,
                CourierEmail = courierEmail});

            switch (res)
            {
                case 0:
                    return Ok($"Order {id} cannot be delivered by {courierName} (request OK)");
                case 1:
                    return BadRequest($"Order {id} doesn't exist in the database");
                case 2:
                    return BadRequest($"Order {id} status doesn't allow for 'Cannot Deliver'");
                case 3:
                    return BadRequest($"Order {id} belongs to (was taken by) a different courier");
                default:
                    return BadRequest($"Other error");
            }
        }
    }
}