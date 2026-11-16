using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Tables
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public IndexModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<RestaurantTable> Tables { get; set; } = new List<RestaurantTable>();

        [BindProperty]
        public RestaurantTable NewTable { get; set; } = new RestaurantTable();

        public async Task OnGetAsync()
        {
            Tables = await _context.RestaurantTables
                .Include(t => t.Orders.Where(o => o.Status != OrderStatus.Payee && o.Status != OrderStatus.Annulee))
                .OrderBy(t => t.TableNumber)
                .ToListAsync();
        }

        // Ajouter une nouvelle table
        public async Task<IActionResult> OnPostAddTableAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            // Vérifier numéro unique
            if (await _context.RestaurantTables.AnyAsync(t => t.TableNumber == NewTable.TableNumber))
            {
                ModelState.AddModelError("NewTable.TableNumber", "Ce numéro de table existe déjà.");
                await OnGetAsync();
                return Page();
            }

            _context.RestaurantTables.Add(NewTable);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        // Basculer le statut d'une table
        public async Task<IActionResult> OnPostUpdateStatusAsync(int tableId, TableStatus status)
        {
            var table = await _context.RestaurantTables.FindAsync(tableId);
            if (table != null)
            {
                table.Status = status;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        // Supprimer une table
        public async Task<IActionResult> OnPostDeleteTableAsync(int tableId)
        {
            var table = await _context.RestaurantTables.FindAsync(tableId);
            if (table != null)
            {
                _context.RestaurantTables.Remove(table);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}

