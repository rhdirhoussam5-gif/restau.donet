using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages.Admin.Reservations
{
    public class IndexModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public IndexModel(RestaurantDbContext context)
        {
            _context = context;
        }

        public IList<Reservation> Reservations { get; set; } = new List<Reservation>();
        public IList<RestaurantTable> Tables { get; set; } = new List<RestaurantTable>();

        [BindProperty(SupportsGet = true)]
        public ReservationStatus? StatusFilter { get; set; }

        [BindProperty]
        public Reservation NewReservation { get; set; } = new Reservation
        {
            ReservationDate = DateTime.Today.AddHours(20),
            NumberOfGuests = 2
        };

        public async Task OnGetAsync()
        {
            Tables = await _context.RestaurantTables.OrderBy(t => t.TableNumber).ToListAsync();

            var query = _context.Reservations
                .Include(r => r.RestaurantTable)
                .AsQueryable();

            if (StatusFilter.HasValue)
            {
                query = query.Where(r => r.Status == StatusFilter.Value);
            }

            Reservations = await query
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAddReservationAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            NewReservation.CreatedAt = DateTime.Now;
            if (NewReservation.RestaurantTableId.HasValue)
            {
                NewReservation.Status = ReservationStatus.Confirmee;
                var table = await _context.RestaurantTables.FindAsync(NewReservation.RestaurantTableId.Value);
                if (table != null)
                {
                    table.Status = TableStatus.Reservee;
                }
            }

            _context.Reservations.Add(NewReservation);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync(int id, ReservationStatus status, int? tableId)
        {
            var res = await _context.Reservations
                .Include(r => r.RestaurantTable)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (res != null)
            {
                res.Status = status;

                if (tableId.HasValue && tableId.Value > 0)
                {
                    res.RestaurantTableId = tableId.Value;
                    var table = await _context.RestaurantTables.FindAsync(tableId.Value);
                    if (table != null && status == ReservationStatus.Confirmee)
                    {
                        table.Status = TableStatus.Reservee;
                    }
                }
                else if (status == ReservationStatus.Annulee || status == ReservationStatus.Terminee)
                {
                    if (res.RestaurantTable != null)
                    {
                        res.RestaurantTable.Status = TableStatus.Disponible;
                    }
                }

                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { StatusFilter });
        }
    }
}

