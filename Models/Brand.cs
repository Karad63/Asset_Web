using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asset_Web.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        
        [Display(Name = "Brand")]
        public string BrandName { get; set; }
        
        public ICollection<BrandModel> BrandModels { get; set; }
    }
}