using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Orders
{
    public class DetailsModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public DetailsModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.RestaurantTable)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.MenuItem)
                        .ThenInclude(m => m!.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            Order = order;
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, OrderStatus status)
        {
            var order = await _context.Orders
                .Include(o => o.RestaurantTable)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order != null)
            {
                order.Status = status;

                // Si payé ou annulé, libérer la table
                if ((status == OrderStatus.Payee || status == OrderStatus.Annulee) && order.RestaurantTable != null)
                {
                    order.RestaurantTable.Status = TableStatus.Disponible;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id });
        }

        public async Task<IActionResult> OnPostPayAsync(int id, string paymentMethod)
        {
            var order = await _context.Orders
                .Include(o => o.RestaurantTable)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order != null)
            {
                order.Status = OrderStatus.Payee;
                order.PaymentMethod = paymentMethod;

                if (order.RestaurantTable != null)
                {
                    order.RestaurantTable.Status = TableStatus.Disponible;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id });
        }
    }
}
