using API.Extensions;
using Application.Orders;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetOrdersCourierController : BaseApiController
    {
        [HttpGet]
        //[HttpGet("{id}")]
        //public async Task<ActionResult<List<OrderDTO>>> GetOrders(int id)
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            var courierId = HttpContext.GetUserId();
            Console.WriteLine(courierId);

            return await Mediator.Send(new GetOrdersCourier.Query{ CourierId = courierId });
        }
    }
}