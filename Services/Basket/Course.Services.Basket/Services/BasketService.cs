using Course.Services.Basket.Dtos;
using Course.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Course.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDatabase().KeyDeleteAsync(userId);
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("İşlem başarısız",500);
        }

        public async Task<Response<BasketDto>> GetBasket(string userId)
        {
            var existBasket=await _redisService.GetDatabase().StringGetAsync(userId);
            if(String.IsNullOrEmpty(existBasket))
            {
                return Response<BasketDto>.Fail("Basket not found", 404);
            }
            return Response<BasketDto>.Success(JsonSerializer.Deserialize<BasketDto>(existBasket), 200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDto basketDto)
        {
            var status = await _redisService.GetDatabase().StringSetAsync(basketDto.UserId, JsonSerializer.Serialize(basketDto));
            return status == true ? Response<bool>.Success(204) : Response<bool>.Fail("İşlem başarışız", 500);
        }
    }
}
