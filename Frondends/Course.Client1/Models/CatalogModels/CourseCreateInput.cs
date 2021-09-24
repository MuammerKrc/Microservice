using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Models.CatalogModels
{
    public class CourseCreateInput
    {
        public string CategoryId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string PictureUrl { get; set; }
        public FeatureViewModel Feature { get; set; }
        public IFormFile PhotoFile { get; set; } 
    }
}
