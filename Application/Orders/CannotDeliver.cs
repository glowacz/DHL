using Domain;
using MediatR;
using Persistence;

namespace Application.Orders
{
    public class CannotDeliver
    {
        public class Query : IRequest<int>
        {
            public int OrderId { get; set; }
            public string Name { get; set; }
            public string Reason { get; set; }
        }

        public class Handler : IRequestHandler<Query, int>
        {
            private readonly DataContext _context;
            private readonly IEmailSender _emailSender;
            public Handler(DataContext context, IEmailSender emailSender)
            {
                _context = context;
                _emailSender = emailSender;
            }
            public async Task<int> Handle(Query request, CancellationToken cancellationToken)
            {
                var order = await _context.Orders.FindAsync(request.OrderId);
                
                if(order is null) return 1;
                if(!(order.Status == 3 || order.Status == 4)) return 2; 

                order.Status = 1; // now someone else can take it
                // var oldOrder = order.Adapt<OldOrder>();
                // _context.DeliveredOrders.Add(oldOrder);
                // _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                // string destEmail = "glowacki.pj@gmail.com",
                string destEmail = order.Email,
                subject = $"Cannot deliver - {request.Name}",
                message = $"I cannot deliver order {order.Id}.\n\nThe reason is:\n{request.Reason}\n\n{request.Name}";
                _emailSender.SendEmailAsync(destEmail, subject, message);

                return 0;
            }
        }
    }
}