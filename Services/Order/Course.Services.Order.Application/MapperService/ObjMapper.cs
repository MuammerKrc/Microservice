using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.MapperService
{
    public static class ObjMapper
    {
        private static readonly Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
          {
              var config = new MapperConfiguration(cfg =>
               {
                   cfg.AddProfile<MapperClass>();
               });
              return config.CreateMapper();
          });

        public static IMapper Mapper = lazy.Value;
    }
}
