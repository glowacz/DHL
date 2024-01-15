using Application.Orders;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetOrdersCourierController : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders(int id)
        {
            return await Mediator.Send(new GetOrdersCourier.Query{CourierId = id});
        }
    }
}