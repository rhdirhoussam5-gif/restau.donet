using System.ComponentModel.DataAnnotations;

namespace restauration.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du client est obligatoire.")]
        [StringLength(100)]
        [Display(Name = "Nom du client")]
        public string CustomerName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        [StringLength(150)]
        [Display(Name = "Email")]
        public string? CustomerEmail { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone est obligatoire.")]
        [Phone(ErrorMessage = "Format de téléphone invalide.")]
        [StringLength(30)]
        [Display(Name = "Téléphone")]
        public string CustomerPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "La date et l'heure sont obligatoires.")]
        [Display(Name = "Date & Heure")]
        public DateTime ReservationDate { get; set; } = DateTime.Now.AddHours(2);

        [Required(ErrorMessage = "Le nombre de convives est obligatoire.")]
        [Range(1, 40, ErrorMessage = "Le nombre de personnes doit être compris entre 1 et 40.")]
        [Display(Name = "Nombre de convives")]
        public int NumberOfGuests { get; set; } = 2;

        [Display(Name = "Statut")]
        public ReservationStatus Status { get; set; } = ReservationStatus.EnAttente;

        [StringLength(500)]
        [Display(Name = "Notes ou demandes particulières")]
        public string? Notes { get; set; }

        [Display(Name = "Table assignée")]
        public int? RestaurantTableId { get; set; }

        public RestaurantTable? RestaurantTable { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

