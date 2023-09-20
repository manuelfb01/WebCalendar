using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebCalendar.Models
{
    public class Equipos
    {
        [Key]
        public int ID_Equipo { get; set; }

        [Required(ErrorMessage = "El nombre del equipo es obligatorio")]
        [Display(Name = "Nombre del equipo")]
        [StringLength(100)]
        [MaxLength(100, ErrorMessage = "No puede tener más de 100 caracteres"), MinLength(2, ErrorMessage = "Debe tener al menos 2 caracteres")]
        public string NombreEquipo { get; set; }

        public int ID_Competicion { get; set; }

        [ForeignKey("ID_Competicion")]
        public Competiciones? Competicion { get; set; }
    }
}
