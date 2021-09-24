using Course.Services.Discount.Models;
using Course.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Discount.Services
{
    public interface IDiscountService
    {
        Task<Response<List<DiscountItem>>> GetAllAsync();
        Task<Response<DiscountItem>> GetByIdAsync(int Id);
        Task<Response<NoContent>> SaveAsync(DiscountItem discountItem);
        Task<Response<NoContent>> UpdateAsync(DiscountItem discountItem);
        Task<Response<NoContent>> DeleteAsync(int id);
        Task<Response<DiscountItem>> GetCodeByUserId(string code, string userId);

    }
}
