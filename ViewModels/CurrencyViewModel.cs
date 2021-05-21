using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asset_Web.ViewModels
{
    public class CurrencyViewModel
    {
        public int CurrencyId { get; set; }

        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }

        public string ErrorMessage { get; set; }
    }
}
