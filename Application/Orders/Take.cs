using MediatR;
using Persistence;

namespace Application.Orders
{
    public class Take
    {
        public class Command : IRequest<int>
        {
            public int OrderId { get; set; }
            public string CourierID { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;  
            }
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = await _context.Orders.FindAsync(request.OrderId);

                if (order == null) return 1;
                if (order.Status != 1) return 2;

                order.CourierId = request.CourierID;
                //order.CourierID = request.CourierID;
                order.Status = 3;
                order.lastTimestamp = DateTime.Now;

                //if (order is not null && order.Status == 1)
                //{
                //    order.CourierId = request.CourierID;
                //    //order.CourierID = request.CourierID;
                //    order.Status = 3;
                //    order.lastTimestamp = DateTime.Now;
                //}

                await _context.SaveChangesAsync();

                return 0;
            }
        }
    }
}