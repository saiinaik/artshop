using art_web_app.Data;
using art_web_app.Models;
using art_web_app.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace art_web_app.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        [TempData]
        public string StatusMessage { get; set; }

        public SubCategoryController(ApplicationDbContext context)
        {
            this._context = context;
        }

        // Get INDEX
        public async Task<IActionResult> Index()
        {
            var subCategories = await _context.SubCategories.Include(c => c.Category).ToListAsync();
            return View(subCategories);
        }

        // Get CREATE
        public async Task<IActionResult> Create()
        {
            SubCategoryAndCategoryVM model = new SubCategoryAndCategoryVM()
            {
                CategoryList = await _context.Categories.ToListAsync(),
                SubCategory = new SubCategory(),
                SubCategoryList = await _context.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        // Post Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryAndCategoryVM model)
        {
            if (ModelState.IsValid)
            {
                var subCategoryExists = await _context.SubCategories
                    .Include(s => s.Category)
                    .Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId).ToListAsync();


                if (subCategoryExists.Count() > 0)
                {
                    // Error
                    StatusMessage = "Error: Sub category under " + subCategoryExists.First().Category.Name + " category exists. Please try another name";
                }
                else
                {
                    await _context.SubCategories.AddAsync(model.SubCategory);
                    await _context.SaveChangesAsync();
                    StatusMessage = "Success : " + model.SubCategory.Name + " Sub-Category added succesfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCategoryAndCategoryVM modelVm = new SubCategoryAndCategoryVM()
            {
                CategoryList = await _context.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _context.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
                StatusMessage = StatusMessage,
            };

            return View(modelVm);
        }

        [ActionName("GetSubCategory")]
        public async Task<IActionResult> GetSubcategories(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();

            subCategories = await (from subCategory in _context.SubCategories where subCategory.CategoryId == id select subCategory).ToListAsync();

            return Json(new SelectList(subCategories, "Id", "Name"));
        }

        // Get Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var subCategory = await _context.SubCategories.SingleOrDefaultAsync(m => m.Id == id);

            if (subCategory == null) return NotFound();

            SubCategoryAndCategoryVM model = new SubCategoryAndCategoryVM()
            {
                CategoryList = await _context.Categories.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _context.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        // Post Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubCategoryAndCategoryVM model)
        {
            if (ModelState.IsValid)
            {
                var subCategoryExists = _context.SubCategories
                    .Include(s => s.Category)
                    .Where(s => s.Name == model.SubCategory.Name && s.Category.Id == model.SubCategory.CategoryId);


                if (subCategoryExists.Count() > 0)
                {
                    // Error
                    StatusMessage = "Error: Sub category under " + subCategoryExists.First().Category.Name + " category exists. Please try another name";
                }
                else
                {
                    var subCategoryFromDb = await _context.SubCategories.FindAsync(id);
                    subCategoryFromDb.Name = model.SubCategory.Name;

                    await _context.SaveChangesAsync();
                    StatusMessage = "Success : " + model.SubCategory.Name + " Sub-Category updated succesfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            SubCategoryAndCategoryVM modelVm = new SubCategoryAndCategoryVM()
            {
                CategoryList = await _context.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _context.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
                StatusMessage = StatusMessage,
            };

            return View(modelVm);
        }

        // Get Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var subCategory = await _context.SubCategories.SingleOrDefaultAsync(m => m.Id == id);

            if (subCategory == null) return NotFound();

            SubCategoryAndCategoryVM model = new SubCategoryAndCategoryVM()
            {
                CategoryList = await _context.Categories.ToListAsync(),
                SubCategory = subCategory,
                SubCategoryList = await _context.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync()
            };

            return View(model);
        }

        // Post Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SubCategoryAndCategoryVM model)
        {

            if(id == null) return NotFound();

            var subCategoryFromDb = await _context.SubCategories.FindAsync(id);

            if(subCategoryFromDb == null)
            {
                // Error
                StatusMessage = "Error: Sub category " + model.SubCategory.Name + " could not be deleted. Please try again";
            }
            else
            {
                _context.SubCategories.Remove(subCategoryFromDb);
                await _context.SaveChangesAsync();
                StatusMessage = "Success : " + model.SubCategory.Name + " Sub-Category deleted succesfully";
                return RedirectToAction(nameof(Index));
            }

            SubCategoryAndCategoryVM modelVm = new SubCategoryAndCategoryVM()
            {
                CategoryList = await _context.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                SubCategoryList = await _context.SubCategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync(),
                StatusMessage = StatusMessage,
            };

            return View(modelVm);
        }


    }
}
