using Application.Orders;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CannotDeliverOrderController : BaseApiController
    {
        [HttpPost("{id}")]
        public async Task<IActionResult> HandleRequest(int id)
        {
            int res = await Mediator.Send(new CannotDeliver.Query{OrderId = id});
            if (res == 0) return Ok($"Cannot deliver order {id} (request OK)");
            else if(res == 1) return BadRequest("This order is not present in the database");
            else if(res == 2) return BadRequest("This order has a status that doesn't allow for 'Cannot Deliver' (but is present in the database)");
            else return BadRequest("Unknown Error");
        }
    }
}