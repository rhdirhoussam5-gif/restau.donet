using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restauration.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom du plat est obligatoire.")]
        [StringLength(120)]
        [Display(Name = "Nom du plat")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire.")]
        [Range(0.01, 10000.0, ErrorMessage = "Le prix doit être supérieur à 0.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Prix (€)")]
        public decimal Price { get; set; }

        [StringLength(500)]
        [Display(Name = "URL de l'image")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Disponible")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "Temps de préparation (min)")]
        public int PreparationTimeMinutes { get; set; } = 15;

        [Required(ErrorMessage = "La catégorie est obligatoire.")]
        [Display(Name = "Catégorie")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

