using MediatR;
using Persistence;

namespace Application.Orders
{
    public class Accept
    {
        public class Command : IRequest<int>
        {
            public int OrderId { get; set; }
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
                if (order.Status != 0) return 2;
                
                order.Status = 1;
                await _context.SaveChangesAsync();
                return 0;
            }
        }
    }
}