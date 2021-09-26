using Course.Client1.Models.BasketModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Services.Abstract.MicroserviceAbstract
{
    public interface IBasketService
    {
        Task<bool> SaveOrUpdate(BasketViewModel basketModel);
        Task<BasketViewModel> Get();
        Task<bool> Delete();
        Task AddBasketItem(BasketItemViewModel basketItemView);
        Task<bool> RemoveBasketItems(string courseId);
        Task<bool> ApplyDiscount(string discountCode);
        Task<bool> CancelApplyDiscount();
    }
}
