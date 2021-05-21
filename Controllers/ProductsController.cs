using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asset_Web.Data;
using Asset_Web.Models;


namespace Asset_Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Product
                .Include(p => p.ProdCat)
                .Include(p => p.ProdModel.Brand)
                .Include(p => p.ProdModel)
                .Include(p => p.ProdOffice)
                .Include(p => p.ProdOffice.Currency);

            var productContextSorted = applicationDbContext.OrderBy(x => x.ProdCat.CatName).ThenBy(x => x.ProdModel.Brand.BrandName).ThenBy(x => x.ProdModel.BrandModelName);
            return View(await productContextSorted.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProdCat)
                .Include(p => p.ProdModel.Brand)
                .Include(p => p.ProdModel)
                .Include(p => p.ProdOffice)
                .Include(p => p.ProdOffice.Currency)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category.OrderBy(x => x.CatName), "CategoryId", "CatName");
            //ViewData["BrandList"] = new SelectList(_context.Brand, "BrandId", "BrandName");
            ViewData["BrandModelId"] = new SelectList(_context.BrandModel.OrderBy(x => x.BrandModelName), "BrandModelId", "BrandModelName");
            ViewData["OfficeId"] = new SelectList(_context.Office.OrderBy(x => x.OfficeName), "OfficeId", "OfficeName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,CategoryId,BrandModelId,ProdPrice,ProdPurchaseDate,OfficeId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", product.CategoryId);
            //ViewData["BrandList"] = new SelectList(_context.Brand, "BrandId", "BrandId");
            ViewData["BrandModelId"] = new SelectList(_context.BrandModel, "BrandModelId", "BrandModelId", product.BrandModelId);
            ViewData["OfficeId"] = new SelectList(_context.Office, "OfficeId", "OfficeId", product.OfficeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category.OrderBy(x => x.CatName), "CategoryId", "CatName", product.CategoryId);
            ViewData["BrandModelId"] = new SelectList(_context.BrandModel.OrderBy(x => x.BrandModelName), "BrandModelId", "BrandModelName", product.BrandModelId);
            ViewData["OfficeId"] = new SelectList(_context.Office.OrderBy(x => x.OfficeName), "OfficeId", "OfficeName", product.OfficeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CategoryId,BrandModelId,ProdPrice,ProdPurchaseDate,OfficeId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "CategoryId", "CategoryId", product.CategoryId);
            ViewData["BrandModelId"] = new SelectList(_context.BrandModel, "BrandModelId", "BrandModelId", product.BrandModelId);
            ViewData["OfficeId"] = new SelectList(_context.Office, "OfficeId", "OfficeId", product.OfficeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.ProdCat)
                .Include(p => p.ProdModel.Brand)
                .Include(p => p.ProdModel)
                .Include(p => p.ProdOffice)
                .Include(p => p.ProdOffice.Currency)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
