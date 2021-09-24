using Course.Client1.ConfigurationOptions.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.ConfigurationOptions
{
    public class ClientSettings:IClientSettings
    {
        public Client WebClient { get; set; }
        public Client ForUser { get; set; }
    }
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
   
}
