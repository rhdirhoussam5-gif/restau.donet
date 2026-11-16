using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Inventory
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public IndexModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();

        [BindProperty]
        public InventoryItem NewItem { get; set; } = new InventoryItem();

        public async Task OnGetAsync()
        {
            InventoryItems = await _context.InventoryItems
                .OrderBy(i => i.Name)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAddItemAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            NewItem.LastRestocked = DateTime.Now;
            _context.InventoryItems.Add(NewItem);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRestockAsync(int id, decimal addQuantity)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item != null && addQuantity > 0)
            {
                item.Quantity += addQuantity;
                item.LastRestocked = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var item = await _context.InventoryItems.FindAsync(id);
            if (item != null)
            {
                _context.InventoryItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}

