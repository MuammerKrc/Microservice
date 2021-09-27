﻿using Course.Services.Order.Domain.OrderAggregate;
using Course.Services.Order.Infrastucture;
using Course.Shared.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.Consumer
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var newAddress = new Address(context.Message.Province, context.Message.District, context.Message.Street, context.Message.ZipCode, context.Message.Line);
            var order = new Order.Domain.OrderAggregate.Order(context.Message.BuyerId, newAddress);
            context.Message.OrderItemDtos.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.Price, "");
            });
            await _orderDbContext.Orders.AddAsync(order);
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
