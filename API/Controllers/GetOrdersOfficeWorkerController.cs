using Application.Orders;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetOrdersOfficeWorkerController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetOrders(int id)
        {
            return await Mediator.Send(new GetOrdersOfficeWorker.Query{});
        }
    }
}