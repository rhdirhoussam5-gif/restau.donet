using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using restauration.Data;
using restauration.Models;

namespace restauration.Pages
{
    public class ReserverModel : PageModel
    {
        private readonly RestaurantDbContext _context;

        public ReserverModel(RestaurantDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; } = new Reservation
        {
            ReservationDate = DateTime.Today.AddHours(20),
            NumberOfGuests = 2
        };

        public bool IsSuccess { get; set; }
        public string? SuccessMessage { get; set; }

        public void OnGet()
        {
            // Initialisation avec l'heure du dîner par défaut
            Reservation.ReservationDate = DateTime.Today.AddHours(20);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Vérifier que la date n'est pas dans le passé
            if (Reservation.ReservationDate < DateTime.Now.AddMinutes(-30))
            {
                ModelState.AddModelError("Reservation.ReservationDate", "La date de réservation ne peut pas être dans le passé.");
                return Page();
            }

            // Trouver automatiquement une table disponible adaptée au nombre de convives
            var availableTable = await _context.RestaurantTables
                .Where(t => t.Status == TableStatus.Disponible && t.Capacity >= Reservation.NumberOfGuests)
                .OrderBy(t => t.Capacity)
                .FirstOrDefaultAsync();

            if (availableTable != null)
            {
                Reservation.RestaurantTableId = availableTable.Id;
                Reservation.Status = ReservationStatus.Confirmee;
                availableTable.Status = TableStatus.Reservee;
            }
            else
            {
                // En attente de confirmation manuelle par le personnel
                Reservation.Status = ReservationStatus.EnAttente;
            }

            Reservation.CreatedAt = DateTime.Now;

            _context.Reservations.Add(Reservation);
            await _context.SaveChangesAsync();

            IsSuccess = true;
            SuccessMessage = availableTable != null
                ? $"Votre réservation pour {Reservation.NumberOfGuests} personne(s) le {Reservation.ReservationDate:dd/MM/yyyy à HH:mm} est CONFIRMÉE (Table n°{availableTable.TableNumber}) !"
                : $"Votre demande de réservation pour {Reservation.NumberOfGuests} personne(s) a bien été reçue. Notre équipe vous confirmera la disponibilité sous peu.";

            return Page();
        }
    }
}

