using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Application.MapperService;
using Course.Services.Order.Application.Queries;
using Course.Services.Order.Infrastucture;
using Course.Shared.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDto>>>
    {
        public readonly OrderDbContext _context;

        public GetOrdersByUserIdHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(i => i.OrderItems)
                                               .Where(x => x.BuyerId == request.UserId)
                                               .ToListAsync();
            if(!orders.Any())
            {
                return Response<List<OrderDto>>.Success(new List<OrderDto>(),200);
            }

            var orderDto = ObjMapper.Mapper.Map<List<OrderDto>>(orders);

            return Response<List<OrderDto>>.Success(orderDto, 200);
           
        }
    }
}
