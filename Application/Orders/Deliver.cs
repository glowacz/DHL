using Domain;
using Mapster;
using MediatR;
using Persistence;

namespace Application.Orders
{
    public class Deliver
    {
        public class Command : IRequest<int>
        {
            public int OrderId { get; set; }
            public string CourierId { get; set; }
        }

        public class Handler : IRequestHandler<Command, int>
        {
            private readonly DataContext _context;
            private readonly IEmailSender _emailSender;
            public Handler(DataContext context, IEmailSender emailSender)
            {
                _context = context;
                _emailSender = emailSender;
            }
            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var order = await _context.Orders.FindAsync(request.OrderId);
                if (order == null) return 1;
                if (order.Status != 4) return 2;
                if (order.CourierId != request.CourierId) return 3;

                order.Status = 5; // useless ?
                order.lastTimestamp = DateTime.Now;
                var oldOrder = order.Adapt<OldOrder>();
                _context.DeliveredOrders.Add(oldOrder);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                string destEmail = order.Email,
                subject = $"Order delivered",
                message = $"Our courier has successfuly delivered your order :)";
                _emailSender.SendEmailAsync(destEmail, subject, message);
                
                return 0;
            }
        }
    }
}