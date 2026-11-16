using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Menu
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public IndexModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public IList<Category> Categories { get; set; } = new List<Category>();

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();

            var query = _context.MenuItems.Include(m => m.Category).AsQueryable();

            if (CategoryId.HasValue && CategoryId.Value > 0)
            {
                query = query.Where(m => m.CategoryId == CategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(Search))
            {
                var term = Search.Trim().ToLower();
                query = query.Where(m => m.Name.ToLower().Contains(term));
            }

            MenuItems = await query.OrderBy(m => m.CategoryId).ThenBy(m => m.Name).ToListAsync();
        }

        public async Task<IActionResult> OnPostToggleAvailabilityAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item != null)
            {
                item.IsAvailable = !item.IsAvailable;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { CategoryId, Search });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item != null)
            {
                // Vérifier si lié à des commandes
                var hasOrders = await _context.OrderItems.AnyAsync(o => o.MenuItemId == id);
                if (hasOrders)
                {
                    // Désactiver au lieu de supprimer pour ne pas corrompre l'historique
                    item.IsAvailable = false;
                }
                else
                {
                    _context.MenuItems.Remove(item);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { CategoryId, Search });
        }
    }
}

