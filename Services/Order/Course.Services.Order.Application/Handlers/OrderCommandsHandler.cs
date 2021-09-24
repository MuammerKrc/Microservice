using Course.Services.Order.Application.Commands;
using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Domain.OrderAggregate;
using Course.Services.Order.Infrastucture;
using Course.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Handlers
{
    public class OrderCommandsHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderResponseDto>>
    {
        private readonly OrderDbContext _context;

        public OrderCommandsHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderResponseDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var adress = new Address(request.AddressDto.Province, request.AddressDto.District, request.AddressDto.Street, request.AddressDto.ZipCode, request.AddressDto.Line);
            var order = new Order.Domain.OrderAggregate.Order(request.BuyerId, adress);
            request.OrderItemDtos.ForEach(i =>
            {
                order.AddOrderItem(i.PictureUrl, i.ProductName, i.Price, i.PictureUrl);
            });
            await _context.Orders.AddAsync(order);
            var result = await _context.SaveChangesAsync();

            return Response<CreatedOrderResponseDto>.Success(new CreatedOrderResponseDto { OrderId = order.Id }, 200);
        }
    }
}
