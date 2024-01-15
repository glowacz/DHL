using Application.Orders;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    // [Route("Order/GetStatus")]
    [Route("api/[controller]")]
    public class GetOrderStatusController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> GetStatus(int id)
        {
            return await Mediator.Send(new Status.Query{OrderId = id});
        }
    }
}