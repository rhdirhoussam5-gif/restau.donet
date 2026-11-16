using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauration.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de l'article est obligatoire.")]
        [StringLength(100)]
        [Display(Name = "Nom de l'ingrédient / article")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, 99999.0)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Quantité en stock")]
        public decimal Quantity { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Unité (kg, L, pièces...)")]
        public string Unit { get; set; } = "kg";

        [Required]
        [Range(0, 99999.0)]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Seuil d'alerte minimum")]
        public decimal MinAlertQuantity { get; set; } = 5;

        [Display(Name = "Dernier réapprovisionnement")]
        public DateTime LastRestocked { get; set; } = DateTime.Now;

        [NotMapped]
        public bool IsLowStock => Quantity <= MinAlertQuantity;
    }
}

