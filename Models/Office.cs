using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asset_Web.Models
{
    public class Office
    {
        public int OfficeId { get; set; }
        
        [Display(Name = "Office Name")]
        public string OfficeName { get; set; }
        
        [Display(Name = "Country")]
        public string OfficeCountry { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyId { get; set; }
        [Display(Name = "Currency")]
        public Currency Currency { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}