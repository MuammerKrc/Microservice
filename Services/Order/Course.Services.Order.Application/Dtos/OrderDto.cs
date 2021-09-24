using Course.Services.Order.Domain.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }

        public DateTime CreatedDate {get;set;}
        public string BuyerId {get;set;}

        public AddressDto Address {get;set;}


        private List<OrderItemDto> OrderItems { get; set; }

    }
}
