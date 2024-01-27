using API.Extensions;
using Application.Orders;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class GetOrdersCourierController : BaseApiController
    {
        [HttpGet]
        //[AllowAnonymous]
        //[HttpGet("{id}")]
        //public async Task<ActionResult<List<OrderDTO>>> GetOrders(int id)
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            //var courierId = HttpContext.GetUserId();
            var courierEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            //var courierId = "105119251730054942410";
            //Console.WriteLine(courierId);

            return await Mediator.Send(new GetOrdersCourier.Query{ CourierEmail = courierEmail });
            //return await Mediator.Send(new GetOrdersCourier.Query{ CourierId = courierId });
        }
    }
}