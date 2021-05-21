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
    public class BrandModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BrandModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BrandModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BrandModel.Include(b => b.Brand);
            var brandModelsSorted = applicationDbContext.OrderBy(x => x.Brand.BrandName).ThenBy(x => x.BrandModelName);
            return View(await brandModelsSorted.ToListAsync());
        }

        // GET: BrandModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brandModel = await _context.BrandModel
                .Include(b => b.Brand)
                .FirstOrDefaultAsync(m => m.BrandModelId == id);
            if (brandModel == null)
            {
                return NotFound();
            }

            return View(brandModel);
        }

        // GET: BrandModels/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brand.OrderBy(x => x.BrandName), "BrandId", "BrandName");
            return View();
        }

        // POST: BrandModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandModelId,BrandModelName,BrandId")] BrandModel brandModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brandModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandId", brandModel.BrandId);
            return View(brandModel);
        }

        // GET: BrandModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brandModel = await _context.BrandModel.FindAsync(id);
            if (brandModel == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brand.OrderBy(x => x.BrandName), "BrandId", "BrandName", brandModel.BrandId);
            return View(brandModel);
        }

        // POST: BrandModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandModelId,BrandModelName,BrandId")] BrandModel brandModel)
        {
            if (id != brandModel.BrandModelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brandModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandModelExists(brandModel.BrandModelId))
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
            ViewData["BrandId"] = new SelectList(_context.Brand, "BrandId", "BrandId", brandModel.BrandId);
            return View(brandModel);
        }

        // GET: BrandModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brandModel = await _context.BrandModel
                .Include(b => b.Brand)
                .FirstOrDefaultAsync(m => m.BrandModelId == id);
            if (brandModel == null)
            {
                return NotFound();
            }

            return View(brandModel);
        }

        // POST: BrandModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brandModel = await _context.BrandModel.FindAsync(id);
            _context.BrandModel.Remove(brandModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandModelExists(int id)
        {
            return _context.BrandModel.Any(e => e.BrandModelId == id);
        }
    }
}
