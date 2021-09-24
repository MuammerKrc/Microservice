using AutoMapper;
using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Mapping
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<CourseModel, CourseDto>().ReverseMap();
            CreateMap<CourseModel, CourseCreateDto>().ReverseMap();
            CreateMap<CourseModel, CourseUpdateDto>().ReverseMap();

            CreateMap<Feature, FeatureDto>().ReverseMap();

            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
