﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Basket.Settings
{
    public interface IRedisSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
