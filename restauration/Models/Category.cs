using System.ComponentModel.DataAnnotations;

namespace restauration.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la catégorie est obligatoire.")]
        [StringLength(100)]
        [Display(Name = "Nom")]
        public string Name { get; set; } = string.Empty;

        [StringLength(250)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Ordre d'affichage")]
        public int DisplayOrder { get; set; } = 0;

        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}

