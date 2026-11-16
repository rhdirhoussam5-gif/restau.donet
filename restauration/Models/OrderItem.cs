using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauration.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        [Required]
        [Display(Name = "Plat")]
        public int MenuItemId { get; set; }
        public MenuItem? MenuItem { get; set; }

        [Range(1, 100, ErrorMessage = "La quantité doit être supérieure ou égale à 1.")]
        [Display(Name = "Quantité")]
        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Prix unitaire (€)")]
        public decimal UnitPrice { get; set; }

        [StringLength(250)]
        [Display(Name = "Précisions (ex: sans oignon)")]
        public string? Notes { get; set; }

        [NotMapped]
        public decimal SubTotal => Quantity * UnitPrice;
    }
}

