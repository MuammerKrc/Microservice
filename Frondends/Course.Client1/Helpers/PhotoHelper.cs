using Course.Client1.ConfigurationOptions.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Helpers
{
    public class PhotoHelper
    {
        private readonly IServiceApiSettings _serviceApiSettings;

        public PhotoHelper(IServiceApiSettings serviceApiSettings)
        {
            _serviceApiSettings = serviceApiSettings;
        }

        public string GetPhotoStockUri(string photoUri)
        {
            return $"{_serviceApiSettings.PhotoStockUrl}/photos/{photoUri}";
        }
    }
}
