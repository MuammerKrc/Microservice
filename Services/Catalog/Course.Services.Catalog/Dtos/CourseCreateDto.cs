using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Services.Catalog.Dtos
{
    public class CourseCreateDto
    {
        public string CategoryId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public FeatureDto Feature { get; set; }
    }
}
