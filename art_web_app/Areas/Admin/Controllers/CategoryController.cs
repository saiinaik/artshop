using System.Threading.Tasks;
using art_web_app.Data;
using art_web_app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace art_web_app.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CategoryController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // * Get Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }

        // * Get Create
        public IActionResult Create()
        {
            return View();
        }

        // * Post Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // * Get Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // * POST Update
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        // * Get Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var categoryToDelete = await _context.Categories.FindAsync(id);
            if (categoryToDelete == null) return View();

            _context.Remove(categoryToDelete);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
