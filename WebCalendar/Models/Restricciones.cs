using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.Models
{
    [Index(nameof(ID_Tipo_Restriccion), nameof(ID_Competicion), nameof(ID_Jornada), nameof(ID_Equipo), 
        nameof(ID_EquipoRival), IsUnique = true)]
    public class Restricciones
    {
        [Key]
        public int ID_Restriccion {  get; set; }

        [Required(ErrorMessage = "El tipo de restricción es obligatorio")]
        [Display(Name = "Tipo de restricción")]
        public int ID_Tipo_Restriccion { get; set; }

        [ForeignKey("ID_Tipo_Restriccion")]
        public TiposRestricciones? TipoRestriccion { get; set; }

        [Required(ErrorMessage = "La competición es obligatoria")]
        [Display(Name = "Competición")]
        public int ID_Competicion { get; set; }

        [ForeignKey("ID_Competicion")]
        public Competiciones? Competicion { get; set; }

        [Required(ErrorMessage = "La jornada es obligatoria")]
        [Display(Name = "Jornada")]
        public int ID_Jornada { get; set; }

        //[ForeignKey("ID_Jornada")]
        //public Jornadas? Jornada { get; set; }

        [Required(ErrorMessage = "El equipo es obligatorio")]
        [Display(Name = "Equipo")]
        public int ID_Equipo {  get; set; }

        [Display(Name = "Equipo rival")]
        public int? ID_EquipoRival { get; set; }

    }
}
