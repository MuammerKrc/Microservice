using Course.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        public string BuyerId { get; private set; }

        //owned type proporties
        public Address Address { get; private set; }

        //Backing Fields

        private List<OrderItem> _orderItems;

        public IReadOnlyList<OrderItem> OrderItems { get { return _orderItems; } }
        public Order()
        {
        }

        public Order(string buyerId, Address address)
        {
            _orderItems = new List<OrderItem>();
            CreatedDate = DateTime.Now;
            BuyerId = buyerId;
            Address = address;
        }

        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
        {
            var existProduct = _orderItems.Any(x => x.ProductId == productId);

            if (!existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);

                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice()
        {
            return _orderItems.Sum(x => x.Price); 
        }
        public decimal GetTotalPriceAgain => _orderItems.Sum(x => x.Price);
    }
}
