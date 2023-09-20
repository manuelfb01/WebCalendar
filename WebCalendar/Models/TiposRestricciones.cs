using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.Models
{
    public class TiposRestricciones
    {
        [Key]
        public int ID_Tipo_Restriccion { get; set; }

        [Required]
        [MaxLength(150)]
        public string Tipo_Restriccion { get; set; }
    }
}
