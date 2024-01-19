using MediatR;
using Persistence;

namespace Application.Orders
{
    public class Take
    {
        public class Command : IRequest
        {
            public int OrderId { get; set; }
            public string CourierID { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;  
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var order = await _context.Orders.FindAsync(request.OrderId);

                //if(order is not null && order.Status == 1 && request.CourierID != 0)
                if(order is not null && order.Status == 1)
                {
                    order.CourierId = request.CourierID;
                    //order.CourierID = request.CourierID;
                    order.Status = 3;
                    order.lastTimestamp = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}