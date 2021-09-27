using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Shared.Messages
{
    public class CreateOrderMessageCommand
    {
        public CreateOrderMessageCommand()
        {
            OrderItemDtos = new List<OrderItemMessageCommand>();
        }

        public string BuyerId { get; set; }
        public List<OrderItemMessageCommand> OrderItemDtos { get; set; }
        public int Id { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Line { get; set; }
    }
}
