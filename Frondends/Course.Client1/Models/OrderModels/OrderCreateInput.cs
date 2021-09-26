using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Models.OrderModels
{
    public class OrderCreateInput
    {
        public OrderCreateInput()
        {
            OrderItemDtos = new List<OrderItemCreateInput>();
        }
        public string BuyerId { get; set; }
        public AddressCreateInput AddressDto { get; set; }
        public List<OrderItemCreateInput> OrderItemDtos { get; set; } 
    }
}
