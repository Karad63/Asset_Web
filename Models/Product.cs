using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Asset_Web.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Display(Name = "Category")]
        public Category ProdCat { get; set; }

        [Display(Name = "Model")]
        public int BrandModelId { get; set; }
        [Display(Name = "Model")]
        public BrandModel ProdModel { get; set; }
        
        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = false)]
        public decimal ProdPrice { get; set; }
        
        [Display(Name = "Purchase Date")]
        public DateTime ProdPurchaseDate { get; set; }

        [Display(Name = "Office")]
        public int OfficeId { get; set; }
        [Display(Name = "Office")]
        public Office ProdOffice { get; set; }
        
    }
}
