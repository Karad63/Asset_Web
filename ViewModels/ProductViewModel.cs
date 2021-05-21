using Asset_Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asset_Web.ViewModels
{
    // This ViewModel is used in Reports View.
    public class ProductViewModel
    {
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public Category ProdCat { get; set; }

        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        [Display(Name = "Brand")]
        public Brand Brand { get; set; }

        [Display(Name = "Model")]
        public int BrandModelId { get; set; }
        [Display(Name = "Model")]
        public BrandModel ProdModel { get; set; }
              

        [Display(Name = "Office")]
        public int OfficeId { get; set; }
        [Display(Name = "Office")]
        public Office ProdOffice { get; set; }


    }
}
