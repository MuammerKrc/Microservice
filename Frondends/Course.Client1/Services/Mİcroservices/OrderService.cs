using Course.Client1.Models.OrderModels;
using Course.Client1.Models.PaymentModels;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using Course.Shared.Dtos;
using Course.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Client1.Services.Mİcroservices
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _client;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(
            HttpClient client,
            IPaymentService paymentService,
            IBasketService basketService,
            ISharedIdentityService sharedIdentityService)
        {
            _client = client;
            _paymentService = paymentService;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }


        //synchronicity
        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();
            if (basket == null)
                if (basket.BasketItems == null)
                    if (!basket.BasketItems.Any())
                        return new OrderCreatedViewModel() { IsSuccessful = false, Error = "Sepette ürün bulunamadı" };
            var paymentInfoInput = new PaymentInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                CVV = checkoutInfoInput.CVV,
                Expiration = checkoutInfoInput.Expiration,
                TotalPrice = basket.TotalPrice
            };
            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);
            if (!responsePayment)
            {
                return new OrderCreatedViewModel() { Error = "Ödeme alınamadı", IsSuccessful = false };
            }
            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                AddressDto = new AddressCreateInput()
                {
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Province = checkoutInfoInput.Province,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode
                }
            };
            basket.BasketItems.ForEach(i => orderCreateInput.OrderItemDtos.Add(new OrderItemCreateInput
            {
                ProductId = i.CourseId,
                Price = i.GetCurrentPrice,
                PictureUrl = "",
                ProductName = i.CourseName
            }));
            var response = await _client.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);
            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel() { IsSuccessful = false, Error = "Siprariş oluşamadı" };
            }
            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
            if (orderCreatedViewModel.Data != null)
            {
                if (orderCreatedViewModel.Data.OrderId != 0)
                {
                    return new OrderCreatedViewModel() { IsSuccessful = true, OrderId = orderCreatedViewModel.Data.OrderId };
                }
            }
            return new OrderCreatedViewModel() { IsSuccessful = false, Error = "Siprariş oluşamadı " };
        }
        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _client.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
            return response.Data;
        }
        //asynchronous
        public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();
            if (basket == null)
                if (basket.BasketItems == null)
                    if (!basket.BasketItems.Any())
                        return new OrderSuspendViewModel() {Error="Sepetinizde Ürün Bulunmamaktadır.",IsSuccessful=false };


            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                AddressDto = new AddressCreateInput()
                {
                    District = checkoutInfoInput.District,
                    Line = checkoutInfoInput.Line,
                    Province = checkoutInfoInput.Province,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode
                }
            };
            basket.BasketItems.ForEach(i => orderCreateInput.OrderItemDtos.Add(new OrderItemCreateInput
            {
                ProductId = i.CourseId,
                Price = i.GetCurrentPrice,
                PictureUrl = "",
                ProductName = i.CourseName
            }));


            var paymentInfoInput = new PaymentInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                CVV = checkoutInfoInput.CVV,
                Expiration = checkoutInfoInput.Expiration,
                TotalPrice = basket.TotalPrice,
                Order=orderCreateInput
            };

            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);
            if (!responsePayment)
            {
                return new OrderSuspendViewModel() { Error = "Ödeme Alınamadı", IsSuccessful = false };
            }
            await _basketService.Delete();
            return new OrderSuspendViewModel() { IsSuccessful = true };
        }
    }
}
