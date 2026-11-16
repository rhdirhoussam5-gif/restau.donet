using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Orders
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public IndexModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<Order> Orders { get; set; } = new List<Order>();

        [BindProperty(SupportsGet = true)]
        public OrderStatus? SelectedStatus { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Orders
                .Include(o => o.RestaurantTable)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.MenuItem)
                .AsQueryable();

            if (SelectedStatus.HasValue)
            {
                query = query.Where(o => o.Status == SelectedStatus.Value);
            }

            Orders = await query.OrderByDescending(o => o.CreatedAt).ToListAsync();
        }

        public async Task<IActionResult> OnPostAdvanceStatusAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.RestaurantTable)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null)
            {
                switch (order.Status)
                {
                    case OrderStatus.EnAttente:
                        order.Status = OrderStatus.EnPreparation;
                        break;
                    case OrderStatus.EnPreparation:
                        order.Status = OrderStatus.Prete;
                        break;
                    case OrderStatus.Prete:
                        order.Status = OrderStatus.Servie;
                        break;
                    case OrderStatus.Servie:
                        order.Status = OrderStatus.Payee;
                        if (order.RestaurantTable != null)
                        {
                            order.RestaurantTable.Status = TableStatus.Disponible;
                        }
                        break;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { SelectedStatus });
        }

        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.RestaurantTable)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order != null)
            {
                order.Status = OrderStatus.Annulee;
                if (order.RestaurantTable != null)
                {
                    order.RestaurantTable.Status = TableStatus.Disponible;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { SelectedStatus });
        }
    }
}

