using Course.Client1.Models.OrderModels;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Controllers
{
    public class OrderController:Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }
        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();
            ViewBag.basket = basket;
            return View(new CheckoutInfoInput());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput checkoutInfoInput)
        {
            //1. Path
            //var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);
            //if (!orderStatus.IsSuccessful)
            //{
            //    var basket = await _basketService.Get();
            //    ViewBag.basket = basket;
            //    ViewBag.error = orderStatus.Error;
            //    return View();
            //}
            //return RedirectToAction("SuccessfulCheckout", new { orderId = orderStatus.OrderId });

            //2.Path asynchronous
            var orderSuspend = await _orderService.SuspendOrder(checkoutInfoInput);
            if(!orderSuspend.IsSuccessful)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderSuspend.Error;
                return View();
            }
            return RedirectToAction("SuccessfulCheckout",new {orderId= 14 });
        }
        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
        public async Task<IActionResult> CheckoutHistory()
        {
            var result = await _orderService.GetOrder();
            return View(result);
        }
    }
}
