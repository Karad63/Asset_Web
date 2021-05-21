using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asset_Web.Models
{
    public class BrandModel
    {
        public int BrandModelId { get; set; }
        
        [Display(Name = "Model Name")]
        public string BrandModelName { get; set; }

        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        [Display(Name = "Brand")]
        public Brand Brand { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}