using MediatR;
using Persistence;
using Domain.DTOs;

namespace Application.Orders
{
    public class GetOrdersCourier
    {
        public class Query : IRequest<List<OrderDTO>>
        {
            public int CourierId { get; set; }
        }

        public class Handler : IRequestHandler<Query, List<OrderDTO>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;  
            }
            public async Task<List<OrderDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                //request.CourierId = GetCurrentUser();
                List<OrderDTO> res = new List<OrderDTO>();
                foreach(var order in _context.Orders)
                {
                    //if(order.Status == 1 || order.CourierID == request.CourierId)
                    if(order.Status == 1)
                        res.Add(await Mapping.Mapping.OrderToDTO(order));
                }

                return res;
            }
        }
    }
}