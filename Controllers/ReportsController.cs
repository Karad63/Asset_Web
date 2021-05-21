using Asset_Web.Data;
using Asset_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Asset_Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category.OrderBy(x => x.CatName), "CategoryId", "CatName");
            ViewData["BrandList"] = new SelectList(_context.Brand.OrderBy(x => x.BrandName), "BrandId", "BrandName");
            ViewData["BrandModelId"] = new SelectList(_context.BrandModel.OrderBy(x => x.BrandModelName), "BrandModelId", "BrandModelName");
            ViewData["OfficeId"] = new SelectList(_context.Office.OrderBy(x => x.OfficeName), "OfficeId", "OfficeName");
            return View();
        }

        public async Task<IActionResult> ShowOutgoingAssets()
        {
            var productContext = _context.Product
                .Include(p => p.ProdCat)
                .Include(p => p.ProdModel.Brand)
                .Include(p => p.ProdModel)
                .Include(p => p.ProdOffice)
                .Include(p => p.ProdOffice.Currency)
                .Where(p => p.ProdPurchaseDate.AddYears(3) <= DateTime.Today.AddMonths(6));
            var productContextSorted = productContext.OrderBy(x=> x.ProdOffice.OfficeName).ThenBy(x=> x.ProdPurchaseDate).ThenBy(x => x.ProdCat.CatName).ThenBy(x => x.ProdModel.Brand.BrandName).ThenBy(x => x.ProdModel.BrandModelName);
            return View(await productContextSorted.ToListAsync());
        }

        //static int CheckAge(DateTime purDate)
        //{
        //    int monthLeft = 0;
        //    if (purDate.AddYears(3) <= DateTime.Today.AddMonths(6))
        //    {
        //        monthLeft = 6;
        //    }
        //    if (purDate.AddYears(3) <= DateTime.Today.AddMonths(3))
        //    {
        //        monthLeft = 3;
        //    }
        //    return monthLeft;
        //}

        public async Task<IActionResult> ShowSummaryReport()
        {
            var productContext = await _context.Product
                .Include(p => p.ProdCat)
                .Include(p => p.ProdModel.Brand)
                .Include(p => p.ProdModel)
                .Include(p => p.ProdOffice)
                .Include(p => p.ProdOffice.Currency)
                .ToListAsync();

            ViewData["total"] = productContext.Select(x => x.ProdPrice).Sum().ToString("N");
            ViewData["count"] = productContext.Count;

            ViewData["office"] = ListOffices(productContext);
            ViewData["category"] = ListCategories(productContext);
            ViewData["brand"] = ListBrands(productContext);
            ViewData["brandModel"] = ListBrandModels(productContext);

            return View();
        }

        public decimal Summarize(List<Models.Product> products)
        {
            decimal summary = 0;
            foreach (var item in products)
            {
                var itemCurrency = new Currency();
                itemCurrency = item.ProdOffice.Currency;
                var priceUSD = item.ProdPrice * itemCurrency.ExRateUSD;               
                summary += priceUSD;
            }
            return summary;
        }

        public async Task<IActionResult> ShowOfficeReport(int id)
        {
            var officeContext =  await _context.Product
                .Include(p => p.ProdCat)
                .Include(p => p.ProdModel.Brand)
                .Include(p => p.ProdModel)
                .Include(p => p.ProdOffice)
                .Include(p => p.ProdOffice.Currency)
                .Where(p => p.OfficeId == id)
                .ToListAsync();
            var officeContextSorted = officeContext
                .OrderBy(x => x.ProdCat.CatName).ThenBy(x => x.ProdModel.Brand.BrandName).ThenBy(x => x.ProdModel.BrandModelName);

            decimal total = Summarize(officeContext);

            ViewData["office"] = _context.Office.FirstOrDefault(x => x.OfficeId == id).OfficeName;
            ViewData["total"] = total.ToString("N");
            ViewData["count"] = officeContext.Count;
            //ViewData["currency"] = officeContext.FirstOrDefault().ProdOffice.Currency.CurrencyName;

            ViewData["categorie"] = ListCategories(officeContext);
            ViewData["brand"] = ListBrands(officeContext);
            ViewData["brandModel"] = ListBrandModels(officeContext);

            return View(officeContextSorted);           
        }

        public async Task<IActionResult> ShowCategoryReport(int id)
        {
            var categoryContext = await _context.Product
                .Include(p => p.ProdCat)
                .Include(p => p.ProdModel.Brand)
                .Include(p => p.ProdModel)
                .Include(p => p.ProdOffice)
                .Include(p => p.ProdOffice.Currency)
                .Where(p => p.CategoryId == id)
                .ToListAsync();
            var categoryContextSorted = categoryContext
                .OrderBy(x => x.ProdOffice.OfficeName).ThenBy(x => x.ProdModel.Brand.BrandName).ThenBy(x => x.ProdModel.BrandModelName);

            decimal total = Summarize(categoryContext);

            ViewData["category"] = _context.Category.FirstOrDefault(x => x.CategoryId == id).CatName;
            ViewData["total"] = total.ToString("N");
            ViewData["count"] = categoryContext.Count;
            //ViewData["currency"] = officeContext.FirstOrDefault().ProdOffice.Currency.CurrencyName;

            ViewData["office"] = ListOffices(categoryContext);                       
            ViewData["brand"] = ListBrands(categoryContext);
            ViewData["brandModel"] = ListBrandModels(categoryContext);

            return View(categoryContextSorted);
        }

        public async Task<IActionResult> ShowBrandReport(int id)
        {
            var brandContext = await _context.Product
                .Include(p => p.ProdCat)
                .Include(p => p.ProdModel.Brand)
                .Include(p => p.ProdModel)
                .Include(p => p.ProdOffice)
                .Include(p => p.ProdOffice.Currency)
                .Where(p => p.ProdModel.BrandId == id)
                .ToListAsync();
            var brandContextSorted = brandContext
                .OrderBy(x => x.ProdOffice.OfficeName).ThenBy(x => x.ProdCat.CatName).ThenBy(x => x.ProdModel.BrandModelName);

            decimal total = Summarize(brandContext);

            ViewData["brand"] = _context.Brand.FirstOrDefault(x => x.BrandId == id).BrandName;
            ViewData["total"] = total.ToString("N");
            ViewData["count"] = brandContext.Count;
            //ViewData["currency"] = officeContext.FirstOrDefault().ProdOffice.Currency.CurrencyName;

            ViewData["office"] = ListOffices(brandContext);
            ViewData["category"] = ListCategories(brandContext);
            ViewData["brandModel"] = ListBrandModels(brandContext);

            return View(brandContextSorted);
        }

        public async Task<IActionResult> ShowBrandModelReport(int id)
        {
            var brandModelContext = await _context.Product
                .Include(p => p.ProdCat)
                .Include(p => p.ProdModel.Brand)
                .Include(p => p.ProdModel)
                .Include(p => p.ProdOffice)
                .Include(p => p.ProdOffice.Currency)
                .Where(p => p.BrandModelId == id)
                .ToListAsync();
            var brandModelContextSorted = brandModelContext
                .OrderBy(x => x.ProdOffice.OfficeName).ThenBy(x => x.ProdCat.CatName).ThenBy(x => x.ProdModel.Brand.BrandName);

            decimal total = Summarize(brandModelContext);

            ViewData["brandModel"] = _context.BrandModel.FirstOrDefault(x => x.BrandModelId == id).BrandModelName;
            ViewData["total"] = total.ToString("N");
            ViewData["count"] = brandModelContext.Count;
            //ViewData["currency"] = officeContext.FirstOrDefault().ProdOffice.Currency.CurrencyName;

            ViewData["office"] = ListOffices(brandModelContext);
            //ViewData["category"] = ListCategories(brandModelContext);
            //ViewData["brand"] = ListBrands(brandModelContext);

            return View(brandModelContextSorted);
        }

        public List<List<string>> ListCategories (List<Models.Product> products )
        {
            // List distinct categories
            List<int> categories = new List<int>();
            foreach (var item in products.OrderBy(x => x.ProdCat.CatName))
            {
                categories.Add(item.ProdCat.CategoryId);
            }
            var distCat = categories.Distinct();

            // Create a list with summarized data about categories
            List<List<string>> catSummarize = new List<List<string>>();
            string catName;
            string catCount;
            string catSumN;
            string catSum;
            foreach (var item in distCat)
            {
                List<Models.Product> catProducts = new(); 
                decimal summary = 0;
                foreach (var prod in products)
                {                    
                    if (prod.CategoryId == item)
                    {
                        catProducts.Add(prod);
                    }
                    summary = Summarize(catProducts);
                }
                catName = _context.Category.FirstOrDefault(x => x.CategoryId == item).CatName;
                catCount = products.Where(c => c.CategoryId == item).Count().ToString();
                //catSumN = products.Where(c => c.CategoryId == item).Select(p => p.ProdPrice).Sum().ToString("N");
                //catSum = products.Where(c => c.CategoryId == item).Select(p => p.ProdPrice).Sum().ToString("F0");
                catSumN = summary.ToString("N");
                catSum = summary.ToString("F0");
                catSummarize.Add(new List<string> { catName, catCount, catSumN, catSum });
            }
            return catSummarize;
        }

        public List<List<string>> ListBrands(List<Models.Product> products)
        {
            // List distinct brands
            List<int> brands = new List<int>();
            foreach (var item in products.OrderBy(x => x.ProdModel.Brand.BrandName))
            {
                brands.Add(item.ProdModel.Brand.BrandId);
            }
            var distBrand = brands.Distinct();

            // Create a list with summarized data about brands
            List<List<string>> brandSummarize = new List<List<string>>();
            string brandName;
            string brandCount;
            string brandSumN;
            string brandSum;
            foreach (var item in distBrand)
            {
                List<Models.Product> brandProducts = new();
                decimal summary = 0;
                foreach (var prod in products)
                {
                    if (prod.ProdModel.BrandId == item)
                    {
                        brandProducts.Add(prod);                        
                    }
                    summary = Summarize(brandProducts);
                }
                brandName = _context.Brand.FirstOrDefault(x => x.BrandId == item).BrandName;
                brandCount = products.Where(b => b.ProdModel.BrandId == item).Count().ToString();
                //brandSumN = products.Where(b => b.ProdModel.BrandId == item).Select(p => p.ProdPrice).Sum().ToString("N");
                //brandSum = products.Where(b => b.ProdModel.BrandId == item).Select(p => p.ProdPrice).Sum().ToString("F0");
                brandSumN = summary.ToString("N");
                brandSum = summary.ToString("F0");
                brandSummarize.Add(new List<string> { brandName, brandCount, brandSumN, brandSum });
            }
            return brandSummarize;
        }

        public List<List<string>> ListBrandModels(List<Models.Product> products)
        {
            // List distinct brandmodels
            List<int> brandModels = new List<int>();
            foreach (var item in products.OrderBy(x => x.ProdModel.BrandModelName))
            {
                brandModels.Add(item.ProdModel.BrandModelId);
            }
            var distBrandModel = brandModels.Distinct();

            // Create a list with summarized data about brandModels
            List<List<string>> brandModelSummarize = new List<List<string>>();
            string brandModelName;
            string brandModelCount;
            string brandModelSumN;
            string brandModelSum;
            foreach (var item in distBrandModel)
            {
                List<Models.Product> brandModelProducts = new();
                decimal summary = 0;
                foreach (var prod in products)
                {
                    if (prod.BrandModelId == item)
                    {
                        brandModelProducts.Add(prod);
                    }
                    summary = Summarize(brandModelProducts);
                }
                brandModelName = _context.BrandModel.FirstOrDefault(x => x.BrandModelId == item).BrandModelName;
                brandModelCount = products.Where(b => b.BrandModelId == item).Count().ToString();
                //brandModelSumN = products.Where(b => b.BrandModelId == item).Select(p => p.ProdPrice).Sum().ToString("N");
                //brandModelSum = products.Where(b => b.BrandModelId == item).Select(p => p.ProdPrice).Sum().ToString("F0");
                brandModelSumN = summary.ToString("N");
                brandModelSum = summary.ToString("F0");
                brandModelSummarize.Add(new List<string> { brandModelName, brandModelCount, brandModelSumN, brandModelSum });
            }
            return brandModelSummarize;
        }

        public List<List<string>> ListOffices(List<Models.Product> products)
        {
            // List distinct offices
            List<int> offices = new List<int>();
            foreach (var item in products.OrderBy(x => x.ProdOffice.OfficeName))
            {
                offices.Add(item.OfficeId);
            }
            var distOffices = offices.Distinct();

            // Create a list with summarized data about offices
            List<List<string>> officeSummarize = new List<List<string>>();
            string officeName;
            string officeCount;
            string officeSumN;
            string officeSum;
            foreach (var item in distOffices)
            {
                List<Models.Product> officeProducts = new();
                decimal summary = 0;
                foreach (var prod in products)
                {
                    if (prod.OfficeId == item)
                    {
                        officeProducts.Add(prod);
                    }
                    summary = Summarize(officeProducts);
                }
                officeName = _context.Office.FirstOrDefault(x => x.OfficeId == item).OfficeName;
                officeCount = products.Where(c => c.OfficeId == item).Count().ToString();
                //officeSumN = products.Where(c => c.OfficeId == item).Select(p => p.ProdPrice).Sum().ToString("N");
                //officeSum = products.Where(c => c.OfficeId == item).Select(p => p.ProdPrice).Sum().ToString("F0");
                officeSumN = summary.ToString("N");
                officeSum = summary.ToString("F0");
                officeSummarize.Add(new List<string> { officeName, officeCount, officeSumN, officeSum });
            }
            return officeSummarize;
        }
    }
}
