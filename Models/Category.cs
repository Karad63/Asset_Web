using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asset_Web.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Display(Name = "Category Name")]
        public string CatName { get; set; }

        public ICollection<Product> products { get; set; }
    }
}