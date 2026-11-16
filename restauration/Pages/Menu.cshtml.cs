using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages
{
    public class MenuModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public MenuModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<Category> Categories { get; set; } = new List<Category>();
        public IList<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        [BindProperty(SupportsGet = true)]
        public int? SelectedCategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            var query = _context.MenuItems
                .Include(m => m.Category)
                .AsQueryable();

            if (SelectedCategoryId.HasValue && SelectedCategoryId > 0)
            {
                query = query.Where(m => m.CategoryId == SelectedCategoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                var term = SearchTerm.Trim().ToLower();
                query = query.Where(m => m.Name.ToLower().Contains(term) || 
                                         (m.Description != null && m.Description.ToLower().Contains(term)));
            }

            MenuItems = await query
                .OrderBy(m => m.CategoryId)
                .ThenBy(m => m.Name)
                .ToListAsync();
        }
    }
}

