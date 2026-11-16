using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public IndexModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public decimal RevenueToday { get; set; }
        public int ActiveOrdersCount { get; set; }
        public int TotalTablesCount { get; set; }
        public int OccupiedTablesCount { get; set; }
        public int ReservationsTodayCount { get; set; }
        public int LowStockCount { get; set; }

        public IList<Order> ActiveOrders { get; set; } = new List<Order>();
        public IList<Reservation> UpcomingReservations { get; set; } = new List<Reservation>();
        public IList<InventoryItem> LowStockItems { get; set; } = new List<InventoryItem>();

        public async Task OnGetAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);

            // Chiffre d'affaires du jour (commandes payées aujourd'hui)
            RevenueToday = await _context.Orders
                .Where(o => o.CreatedAt >= today && o.CreatedAt < tomorrow && o.Status == OrderStatus.Payee)
                .SumAsync(o => o.TotalAmount);

            // Commandes actives (non payées et non annulées)
            ActiveOrdersCount = await _context.Orders
                .CountAsync(o => o.Status != OrderStatus.Payee && o.Status != OrderStatus.Annulee);

            ActiveOrders = await _context.Orders
                .Include(o => o.RestaurantTable)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.MenuItem)
                .Where(o => o.Status != OrderStatus.Payee && o.Status != OrderStatus.Annulee)
                .OrderByDescending(o => o.CreatedAt)
                .Take(10)
                .ToListAsync();

            // Tables
            TotalTablesCount = await _context.RestaurantTables.CountAsync();
            OccupiedTablesCount = await _context.RestaurantTables
                .CountAsync(t => t.Status == TableStatus.Occupee);

            // Réservations du jour
            ReservationsTodayCount = await _context.Reservations
                .CountAsync(r => r.ReservationDate >= today && r.ReservationDate < tomorrow && r.Status != ReservationStatus.Annulee);

            UpcomingReservations = await _context.Reservations
                .Include(r => r.RestaurantTable)
                .Where(r => r.ReservationDate >= today && r.Status != ReservationStatus.Annulee)
                .OrderBy(r => r.ReservationDate)
                .Take(5)
                .ToListAsync();

            // Alertes stock
            var allInventory = await _context.InventoryItems.ToListAsync();
            LowStockItems = allInventory.Where(i => i.IsLowStock).ToList();
            LowStockCount = LowStockItems.Count;
        }

        // Action rapide pour passer le statut d'une commande à l'étape suivante
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
                        // Libérer la table associée
                        if (order.RestaurantTable != null)
                        {
                            order.RestaurantTable.Status = TableStatus.Disponible;
                        }
                        break;
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}

