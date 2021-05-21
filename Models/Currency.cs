using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Asset_Web.Models
{
    public class Currency
    {
        public int CurrencyId { get; set; }

        [Display(Name = "Currency Name")]
        public string CurrencyName { get; set; }

        [Display(Name = "Exchange Rate USD")]
        public decimal ExRateUSD { get; set; }

        [Display(Name = "Exchange Rate Date")]
        public DateTime ExRateDate { get; set; }

        public ICollection<Office> Offices { get; set; }

        // Method to convert currencies using an API
        public decimal CurrencyConversion(string inCurrency, string outCurrency, decimal price)
        {
            string fromCurrency = inCurrency;
            string toCurrency = outCurrency;

            decimal amount = price;

            const string urlPattern = "http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";
            string url = string.Format(urlPattern, fromCurrency, toCurrency);

            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url);

                Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);
                decimal exchangeRate = (decimal)token.SelectToken("rate");

                return amount * exchangeRate;
            }
        }
    }
}