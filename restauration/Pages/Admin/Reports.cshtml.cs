using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin
{
    public class ReportsModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public ReportsModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public decimal TotalRevenueAllTime { get; set; }
        public decimal RevenueToday { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public int TotalPaidOrders { get; set; }
        public decimal AverageBasket { get; set; }

        public class TopDishStat
        {
            public string DishName { get; set; } = string.Empty;
            public string CategoryName { get; set; } = string.Empty;
            public int QuantitySold { get; set; }
            public decimal TotalRevenue { get; set; }
        }

        public IList<TopDishStat> TopDishes { get; set; } = new List<TopDishStat>();

        public Dictionary<string, decimal> RevenueByCategory { get; set; } = new Dictionary<string, decimal>();
        public Dictionary<OrderType, int> OrdersByType { get; set; } = new Dictionary<OrderType, int>();

        public async Task OnGetAsync()
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);

            var paidOrders = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.MenuItem)
                        .ThenInclude(m => m!.Category)
                .Where(o => o.Status == OrderStatus.Payee)
                .ToListAsync();

            TotalRevenueAllTime = paidOrders.Sum(o => o.TotalAmount);
            TotalPaidOrders = paidOrders.Count;
            AverageBasket = TotalPaidOrders > 0 ? TotalRevenueAllTime / TotalPaidOrders : 0m;

            RevenueToday = paidOrders
                .Where(o => o.CreatedAt >= today)
                .Sum(o => o.TotalAmount);

            RevenueThisMonth = paidOrders
                .Where(o => o.CreatedAt >= startOfMonth)
                .Sum(o => o.TotalAmount);

            // Top plats vendus
            var orderItems = paidOrders.SelectMany(o => o.OrderItems).ToList();
            TopDishes = orderItems
                .GroupBy(i => i.MenuItemId)
                .Select(g => new TopDishStat
                {
                    DishName = g.First().MenuItem?.Name ?? "Inconnu",
                    CategoryName = g.First().MenuItem?.Category?.Name ?? "Autre",
                    QuantitySold = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.QuantitySold)
                .Take(5)
                .ToList();

            // Chiffre d'affaires par catégorie
            RevenueByCategory = orderItems
                .GroupBy(i => i.MenuItem?.Category?.Name ?? "Divers")
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity * x.UnitPrice));

            // Répartition par type de commande
            OrdersByType = paidOrders
                .GroupBy(o => o.OrderType)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}

