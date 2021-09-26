using Course.Client1.Models.BasketModels;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using Course.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Client1.Services.Mİcroservices
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;
        private readonly IDiscountService _discountService;
        public BasketService(HttpClient client, IDiscountService discountService)
        {
            _discountService = discountService;
            _client = client;
        }

        public async Task AddBasketItem(BasketItemViewModel basketItemView)
        {
            var response = await Get();
            if (response != null)
            {
                if (!response.BasketItems.Any(i => i.CourseId == basketItemView.CourseId))
                {
                    response.BasketItems.Add(basketItemView);
                }
            }
            else
            {
                response = new BasketViewModel();
                response.BasketItems = new List<BasketItemViewModel>();
                response.BasketItems.Add(basketItemView);
            }

            await SaveOrUpdate(response);
        }


        public async Task<bool> ApplyDiscount(string discountCode)
        {
            await CancelApplyDiscount();
            var basket = await Get();
            if (basket == null || basket.DiscountCode != null)
            {
                return false;
            }
            var hasDiscount = await _discountService.GetDiscount(discountCode);
            if (hasDiscount == null)
            {
                return false;
            }
            basket.DiscountRate = hasDiscount.Rate;
            basket.DiscountCode = hasDiscount.Code;
            return await SaveOrUpdate(basket);
        }

        public async Task<bool> CancelApplyDiscount()
        {
            var basket = await Get();
            if (basket == null || basket.DiscountCode == null)
            {
                return false;
            }
            basket.DiscountCode = null;
            return await SaveOrUpdate(basket);

        }

        public async Task<bool> Delete()
        {
            var result = await _client.DeleteAsync("Baskets");
            return result.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> Get()
        {
            var response = await _client.GetAsync("Baskets");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var data = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();

            
            return data.Data;
        }

        public async Task<bool> RemoveBasketItems(string courseId)
        {
            var basket = await Get();
            if (basket == null)
            {
                return false;
            }
            if (!basket.BasketItems.Any(i => i.CourseId == courseId))
            {
                return false;
            }
            var deleteBasketItem = basket.BasketItems.FirstOrDefault(i => i.CourseId == courseId);
            if (deleteBasketItem == null)
            {
                return false;
            }
            var deleteResult = basket.BasketItems.Remove(deleteBasketItem);
            if (!deleteResult)
            {
                return false;
            }
            if (!basket.BasketItems.Any())
            {
                basket.DiscountCode = null;
            }
            return await SaveOrUpdate(basket);
        }

        public async Task<bool> SaveOrUpdate(BasketViewModel basketModel)
        {
            var response = await _client.PostAsJsonAsync<BasketViewModel>("Baskets", basketModel);
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
    }
}
