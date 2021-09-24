using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.ConfigurationOptions.Abstract
{
    public interface IServiceApiSettings
    {
        public string BaseUrl { get; set; }
        public string PhotoStockUrl { get; set; }
        public ServicePath CatalogService { get; set; }
        public ServicePath PhotoService { get; set; }
    }
}
