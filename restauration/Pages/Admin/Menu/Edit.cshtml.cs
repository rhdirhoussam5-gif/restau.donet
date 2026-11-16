using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Menu
{
    public class EditModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public EditModel(RestaurantDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MenuItem MenuItem { get; set; } = null!;

        public SelectList CategoriesList { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            MenuItem = item;
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

            _context.Attach(MenuItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.MenuItems.AnyAsync(e => e.Id == MenuItem.Id))
                {
                    return NotFound();
                }
                throw;
            }

            return RedirectToPage("./Index");
        }
    }
}

