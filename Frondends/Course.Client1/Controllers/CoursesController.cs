using Course.Client1.Models.CatalogModels;
using Course.Client1.Services.Abstract.MicroserviceAbstract;
using Course.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var response = await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId);
           
            return View(response);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.categorylist = new SelectList(categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput courseCreateInput,string Id)
        {
           if(!ModelState.IsValid)
            {
                var categories = await _catalogService.GetAllCategoryAsync();
                ViewBag.categorylist = new SelectList(categories, "Id", "Name");
                return View();
            }
            courseCreateInput.UserId = _sharedIdentityService.GetUserId;
            await _catalogService.CreateCourseAsync(courseCreateInput);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(string id)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            var course = await _catalogService.GetByCourseId(id);
            ViewBag.categorylist = new SelectList(categories, "Id", "Name",course.CategoryId);

            if (course!=null)
            {
                CourseUpdateInput courseUpdateInput = new CourseUpdateInput
                {
                    Id = course.Id,
                    Name = course.Name,
                    Price = course.Price,
                    Feature = course.Feature,
                    CategoryId = course.CategoryId,
                    UserId = course.UserId,
                    PictureUrl = course.PictureUrl,
                    Description = course.Description
                };
                return View(courseUpdateInput);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {
           
            if (!ModelState.IsValid)
            {
                var categories = await _catalogService.GetAllCategoryAsync();
                var course = await _catalogService.GetByCourseId(courseUpdateInput.Id);
                ViewBag.categorylist = new SelectList(categories, "Id", "Name", course.CategoryId);
                return View();

            }
            await _catalogService.UpdateCourseAsync(courseUpdateInput);
            
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            if(id==null)
            {
                return RedirectToAction("Index");
            }
            await _catalogService.DeleteCourse(id);
            return RedirectToAction("Index");
        }
    }
}
