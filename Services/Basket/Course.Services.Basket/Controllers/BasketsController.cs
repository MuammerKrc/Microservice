using Course.Services.Basket.Dtos;
using Course.Services.Basket.Services;
using Course.Shared.ControllerHelper;
using Course.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : CustomBaseController
    {
        private readonly ISharedIdentityService _identityService;
        private readonly IBasketService _basketService;

        public BasketsController(ISharedIdentityService identityService, IBasketService basketService)
        {
            _identityService = identityService;
            _basketService = basketService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            var Claims = User.Claims;
            return QQReturnObject(await _basketService.GetBasket(_identityService.GetUserId));
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrUpdate(BasketDto basketDto)
        {
           return QQReturnObject(await _basketService.SaveOrUpdate(basketDto));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            return QQReturnObject(await _basketService.Delete(_identityService.GetUserId));
        }
    }
}
