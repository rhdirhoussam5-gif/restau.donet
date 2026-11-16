using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public IndexModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<MenuItem> FeaturedItems { get; set; } = new List<MenuItem>();
        public int AvailableTablesCount { get; set; }
        public int TotalTablesCount { get; set; }

        public async Task OnGetAsync()
        {
            // Récupère une sélection de plats phares
            FeaturedItems = await _context.MenuItems
                .Include(m => m.Category)
                .Where(m => m.IsAvailable)
                .Take(6)
                .ToListAsync();

            TotalTablesCount = await _context.RestaurantTables.CountAsync();
            AvailableTablesCount = await _context.RestaurantTables
                .CountAsync(t => t.Status == TableStatus.Disponible);
        }
    }
}
