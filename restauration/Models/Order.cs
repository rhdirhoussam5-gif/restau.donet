using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauration.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "N° Commande")]
        public string OrderNumber { get; set; } = $"CMD-{DateTime.Now:yyyyMMddHHmmss}";

        [Display(Name = "Table")]
        public int? RestaurantTableId { get; set; }

        public RestaurantTable? RestaurantTable { get; set; }

        [StringLength(100)]
        [Display(Name = "Nom du client")]
        public string? CustomerName { get; set; }

        [Display(Name = "Type de commande")]
        public OrderType OrderType { get; set; } = OrderType.SurPlace;

        [Display(Name = "Statut")]
        public OrderStatus Status { get; set; } = OrderStatus.EnAttente;

        [Display(Name = "Date de création")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Montant total (€)")]
        public decimal TotalAmount { get; set; } = 0m;

        [StringLength(50)]
        [Display(Name = "Mode de paiement")]
        public string? PaymentMethod { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarques")]
        public string? Notes { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

