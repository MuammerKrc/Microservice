using Course.Client1.ConfigurationOptions.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.ConfigurationOptions
{
    public class ServiceApiSettings : IServiceApiSettings
    {
        public string BaseUrl { get; set; }
        public string PhotoStockUrl { get; set; }
        public string GatewayBaseUri { get; set; }
        public ServicePath CatalogService { get; set; }
        public ServicePath PhotoService { get; set; }
        public ServicePath BasketService { get; set; }
        public ServicePath DiscountService { get; set; }
        public ServicePath PaymentService { get; set; }
        public ServicePath OrderService { get; set; }



    }
    public class ServicePath
    {
        public string Path { get; set; }
    }
}
