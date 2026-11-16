using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Menu
{
    public class CreateModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public CreateModel(RestaurantDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MenuItem MenuItem { get; set; } = new MenuItem();

        public SelectList CategoriesList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            var categories = await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();
            CategoriesList = new SelectList(categories, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var categories = await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();
                CategoriesList = new SelectList(categories, "Id", "Name");
                return Page();
            }

            _context.MenuItems.Add(MenuItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

