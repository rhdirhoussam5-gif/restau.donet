using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public IndexModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<Category> Categories { get; set; } = new List<Category>();

        [BindProperty]
        public Category NewCategory { get; set; } = new Category();

        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories
                .Include(c => c.MenuItems)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAddCategoryAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            _context.Categories.Add(NewCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.MenuItems)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category != null)
            {
                if (category.MenuItems.Any())
                {
                    TempData["ErrorMessage"] = $"Impossible de supprimer la catégorie '{category.Name}' car elle contient encore {category.MenuItems.Count} plat(s). Veuillez d'abord réassigner ou supprimer ses plats.";
                    return RedirectToPage();
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}

