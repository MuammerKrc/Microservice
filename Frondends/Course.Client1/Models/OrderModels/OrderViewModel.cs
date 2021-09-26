using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Models.OrderModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        public string BuyerId { get; set; }
        private List<OrderItemViewModel> OrderItems { get; set; }
    }
}
