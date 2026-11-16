using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin
{
    public class KitchenModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public KitchenModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<Order> KitchenOrders { get; set; } = new List<Order>();

        public async Task OnGetAsync()
        {
            // Récupère uniquement les commandes en attente ou en préparation
            KitchenOrders = await _context.Orders
                .Include(o => o.RestaurantTable)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.MenuItem)
                .Where(o => o.Status == OrderStatus.EnAttente || o.Status == OrderStatus.EnPreparation)
                .OrderBy(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostStartPreparationAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null && order.Status == OrderStatus.EnAttente)
            {
                order.Status = OrderStatus.EnPreparation;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMarkReadyAsync(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null && (order.Status == OrderStatus.EnAttente || order.Status == OrderStatus.EnPreparation))
            {
                order.Status = OrderStatus.Prete;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}

