using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Orders
{
    public class CreateModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public CreateModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<RestaurantTable> Tables { get; set; } = new List<RestaurantTable>();
        public IList<Category> Categories { get; set; } = new List<Category>();
        public IList<MenuItem> AvailableMenuItems { get; set; } = new List<MenuItem>();

        [BindProperty]
        public Order Order { get; set; } = new Order();

        [BindProperty]
        public List<int> SelectedItemIds { get; set; } = new List<int>();

        [BindProperty]
        public List<int> ItemQuantities { get; set; } = new List<int>();

        public async Task OnGetAsync(int? tableId)
        {
            Tables = await _context.RestaurantTables.OrderBy(t => t.TableNumber).ToListAsync();
            Categories = await _context.Categories.OrderBy(c => c.DisplayOrder).ToListAsync();
            AvailableMenuItems = await _context.MenuItems
                .Include(m => m.Category)
                .Where(m => m.IsAvailable)
                .OrderBy(m => m.CategoryId)
                .ThenBy(m => m.Name)
                .ToListAsync();

            if (tableId.HasValue)
            {
                Order.RestaurantTableId = tableId.Value;
                Order.OrderType = OrderType.SurPlace;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Valider qu'au moins un article a été sélectionné avec une quantité positive
            var itemsToAdd = new List<OrderItem>();
            decimal total = 0m;

            for (int i = 0; i < SelectedItemIds.Count; i++)
            {
                int itemId = SelectedItemIds[i];
                int qty = i < ItemQuantities.Count ? ItemQuantities[i] : 0;

                if (qty > 0)
                {
                    var menuItem = await _context.MenuItems.FindAsync(itemId);
                    if (menuItem != null)
                    {
                        var orderItem = new OrderItem
                        {
                            MenuItemId = menuItem.Id,
                            Quantity = qty,
                            UnitPrice = menuItem.Price
                        };
                        total += qty * menuItem.Price;
                        itemsToAdd.Add(orderItem);
                    }
                }
            }

            if (!itemsToAdd.Any())
            {
                ModelState.AddModelError("", "Veuillez sélectionner au moins un plat avec une quantité supérieure à 0.");
                await OnGetAsync(Order.RestaurantTableId);
                return Page();
            }

            Order.OrderNumber = $"CMD-{DateTime.Now:yyyyMMddHHmmss}";
            Order.CreatedAt = DateTime.Now;
            Order.Status = OrderStatus.EnAttente;
            Order.TotalAmount = total;
            Order.OrderItems = itemsToAdd;

            // Si c'est sur place et qu'une table est assignée, marquer la table comme occupée
            if (Order.OrderType == OrderType.SurPlace && Order.RestaurantTableId.HasValue)
            {
                var table = await _context.RestaurantTables.FindAsync(Order.RestaurantTableId.Value);
                if (table != null)
                {
                    table.Status = TableStatus.Occupee;
                }
            }

            _context.Orders.Add(Order);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = Order.Id });
        }
    }
}

