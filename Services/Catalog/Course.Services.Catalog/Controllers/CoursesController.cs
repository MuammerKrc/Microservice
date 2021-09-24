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
    public class CoursesController : CustomBaseController
    {
        private readonly ICourseService _courseService;
        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return QQReturnObject(await _courseService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return QQReturnObject(await _courseService.GetByIdAsync(id));
        }
        [HttpGet]
        [Route("/api/[controller]/ByUserId/{userId}")]
        public async Task<IActionResult>GetByUserId(string userId)
        {
            return QQReturnObject(await _courseService.GetAllByUserIdAsync(userId));
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto courseDto)
        {
            return QQReturnObject(await _courseService.CreateCourseASync(courseDto));
        }
        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto courseUpdate)
        {
            return QQReturnObject(await _courseService.UpdateAsync(courseUpdate));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return QQReturnObject(await _courseService.DeleteAsync(id));
        }
    }
}
