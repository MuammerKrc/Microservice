using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.PaymentApi.Models
{
    public class OrderDto
    {
        public OrderDto()
        {
            OrderItemDtos = new List<OrderItemDto>();
        }
        public string BuyerId { get; set; }
        public AdressDto AddressDto { get; set; }
        public List<OrderItemDto> OrderItemDtos { get; set; }

    }
}
