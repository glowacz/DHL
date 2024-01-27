using API.Extensions;
using Application.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcceptOrderController : BaseApiController
    {
        [HttpPost("{id}")]
        public async Task<IActionResult> AcceptOrder(int id)
        {
            //var name = HttpContext.GetUserName();
            var name = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            int res = await Mediator.Send(new Accept.Command{OrderId = id});

            switch (res)
            {
                case 0:
                    return Ok($"Order {id} accepted by office worker ({name})");
                case 1:
                    return BadRequest($"Order {id} doesn't exist in the database");
                case 2:
                    return BadRequest($"Order {id} status doesn't allow for accepting");
                default:
                    return BadRequest($"Other error");
            }
        }
    }
}