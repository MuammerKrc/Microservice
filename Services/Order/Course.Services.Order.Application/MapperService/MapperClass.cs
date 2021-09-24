using AutoMapper;
using Course.Services.Order.Application.Dtos;
using Course.Services.Order.Domain.OrderAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Course.Services.Order.Application.MapperService
{
    public class MapperClass:Profile
    {
        public MapperClass()
        {
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<OrderDto, Order.Domain.OrderAggregate.Order>().ReverseMap();
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();
        }
    }
}
