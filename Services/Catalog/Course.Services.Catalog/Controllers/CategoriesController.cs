using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Services;
using Course.Shared.ControllerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : CustomBaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return QQReturnObject(await _categoryService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return QQReturnObject(await _categoryService.GetById(id));
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto category)
        {
            return QQReturnObject(await _categoryService.CreateAsync(category));
        }
    }
}
