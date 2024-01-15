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
            await Mediator.Send(new CannotDeliver.Command{OrderId = id});
            return Ok($"Cannot deliver order {id} (request OK)");
        }
    }
}