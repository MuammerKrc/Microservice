using Course.Services.Discount.Models;
using Course.Services.Discount.Services;
using Course.Shared.ControllerHelper;
using Course.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : CustomBaseController
    {
        private readonly IDiscountService _discountService;

        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountController(IDiscountService discountService, ISharedIdentityService sharedIdentityService)
        {
            _discountService = discountService;
            _sharedIdentityService = sharedIdentityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return QQReturnObject(await _discountService.GetAllAsync());
        }

        //api/discounts/4
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _discountService.GetByIdAsync(id);

            return QQReturnObject(discount);
        }

        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)

        {
            var userId = _sharedIdentityService.GetUserId;

            var discount = await _discountService.GetCodeByUserId(code, userId);

            return QQReturnObject(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Save(DiscountItem discount)
        {
            return QQReturnObject(await _discountService.SaveAsync(discount));
        }

        [HttpPut]
        public async Task<IActionResult> Update(DiscountItem discount)
        {
            return QQReturnObject(await _discountService.UpdateAsync(discount));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return QQReturnObject(await _discountService.DeleteAsync(id));
        }
    }
}
