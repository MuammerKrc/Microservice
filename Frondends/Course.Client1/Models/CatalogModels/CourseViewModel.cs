using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.Client1.Models.CatalogModels
{
    public class CourseViewModel
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string UserId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string StockPictureUrl { get; set; }
        public DateTime CreatedTime { get; set; }
        public FeatureViewModel Feature { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
