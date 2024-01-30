using MediatR;
using Persistence;

namespace Application.Orders
{
    public class Pickup
    {
        public class Command : IRequest<int>
        {
            public int OrderId { get; set; }
            public string CourierEmail { get; set; }
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
                if (order.Status != 3) return 2;
                if (order.CourierEmail != request.CourierEmail) return 3;

                order.Status = 4;
                order.lastTimestamp = DateTime.Now;
                await _context.SaveChangesAsync();
                return 0;
            }
        }
    }
}