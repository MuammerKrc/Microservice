using Course.Services.Order.Application.Dtos;
using Course.Shared.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Commands
{
    public class CreateOrderCommand:IRequest<Response<CreatedOrderResponseDto>>
    {
        public string BuyerId { get; set; }
        public AddressDto AddressDto { get; set; }
        public List<OrderItemDto> OrderItemDtos { get; set; }
    }
}
