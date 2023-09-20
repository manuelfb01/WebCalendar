using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.InteropServices;

namespace WebCalendar.Models
{
    public class Competiciones
    {
        [Key]
        public int ID_Competicion { get; set; }

        [Required(ErrorMessage = "El nombre de la competición es obligatorio")]
        [Display(Name = "Nombre de la competición")]
        [StringLength(100)]
        public string NombreCompeticion { get; set; }

        [Required(ErrorMessage = "Debe ser un número entero positivo")]
        [Display(Name = "Número de jornadas para enfrentarse al mismo equipo")]
        public int Jor_Rep_Enfrentamiento { get; set; } = 6;

        [Required]
        [Display(Name = "Alternar como local y visitante al final de cada vuelta")]
        public bool Alternar_Local_Vuelta { get; set; } = true;

        [Required]
        [Display(Name = "Número máximo de jornadas seguidas jugando como local o visitante")]
        public int Num_Jor_Loc { get; set; } = 3;

        public int ID_Usuario { get; set; }

        [ForeignKey("ID_Usuario")]
        public Usuarios? Cliente { get; set; }
    }
}
