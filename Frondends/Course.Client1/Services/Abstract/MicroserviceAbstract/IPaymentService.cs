using Course.Client1.Models.PaymentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Services.Abstract.MicroserviceAbstract
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInput paymentInput);

    }
}
