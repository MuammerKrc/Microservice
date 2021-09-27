using Course.Services.PaymentApi.Models;
using Course.Shared.ControllerHelper;
using Course.Shared.Dtos;
using Course.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.PaymentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public PaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDto paymentDto)
        {
            var sendEndpoint =await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:order-created-service"));
            var createOrderMessageCommand = new Shared.Messages.CreateOrderMessageCommand()
            {
                BuyerId = paymentDto.Order.BuyerId,
                Province = paymentDto.Order.AddressDto.Province,
                District = paymentDto.Order.AddressDto.District,
                Street = paymentDto.Order.AddressDto.Street,
                Line = paymentDto.Order.AddressDto.Street,
            };
            paymentDto.Order.OrderItemDtos.ForEach(i => createOrderMessageCommand.OrderItemDtos.Add(new Shared.Messages.OrderItemMessageCommand()
            {
                PictureUrl = "",
                Price = i.Price,
                ProductId = i.ProductId,
                ProductName = i.ProductName
            }));
            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);
            return QQReturnObject(Shared.Dtos.Response<NoContent>.Success(204));
        }
    }
}
