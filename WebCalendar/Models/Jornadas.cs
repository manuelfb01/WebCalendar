using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCalendar.Models
{
    public class Jornadas
    {
        [Key]
        public int ID_Jornada { get; set; }

        public int Num_Jornada { get; set; }

        public int Num_Vuelta { get; set; }

        public int ID_Competicion { get; set; }

        [ForeignKey("ID_Competicion")]
        public Competiciones? Competicion { get; set; }
    }
}
