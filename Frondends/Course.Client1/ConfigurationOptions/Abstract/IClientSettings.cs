using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.ConfigurationOptions.Abstract
{
    public interface IClientSettings
    {
        public Client WebClient { get; set; }
        public Client ForUser { get; set; }
    }
}
