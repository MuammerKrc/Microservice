using Course.Client1.Models.PaymentModels;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Course.Client1.Services.Mİcroservices
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> ReceivePayment(PaymentInput paymentInput)
        {
            var response = await _httpClient.PostAsJsonAsync<PaymentInput>("payments", paymentInput);
            return response.IsSuccessStatusCode;
        }
    }
}
