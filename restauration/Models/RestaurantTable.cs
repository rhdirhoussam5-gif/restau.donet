using System.ComponentModel.DataAnnotations;

namespace restauration.Models
{
    public class RestaurantTable
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le numéro de table est obligatoire.")]
        [Display(Name = "Numéro de table")]
        public int TableNumber { get; set; }

        [Required(ErrorMessage = "La capacité est obligatoire.")]
        [Range(1, 50, ErrorMessage = "La capacité doit être entre 1 et 50.")]
        [Display(Name = "Capacité (personnes)")]
        public int Capacity { get; set; } = 4;

        [Display(Name = "Statut")]
        public TableStatus Status { get; set; } = TableStatus.Disponible;

        [StringLength(100)]
        [Display(Name = "Emplacement")]
        public string Location { get; set; } = "Salle principale";

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

