using Course.Services.Catalog.Dtos;
using Course.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();

        Task<Response<CategoryDto>> GetById(string id);

        Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);
       
    }
}
