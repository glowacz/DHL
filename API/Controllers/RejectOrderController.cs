using API.Extensions;
using Application.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RejectOrderController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> AcceptOrder(int id)
        {
            var name = HttpContext.GetUserName();

            int res = await Mediator.Send(new Reject.Command{OrderId = id});
            
            switch (res)
            {
                case 0:
                    return Ok($"Order {id} rejected by office worker ({name})");
                case 1:
                    return BadRequest($"Order {id} doesn't exist in the database");
                case 2:
                    return BadRequest($"Order {id} status doesn't allow for rejecting");
                default:
                    return BadRequest($"Other error");
            }
        }
    }
}