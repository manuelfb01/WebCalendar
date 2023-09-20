using System.ComponentModel.DataAnnotations;

namespace WebCalendar.Models
{
    public class Partidos
    {
        [Key]
        public int ID_Partido { get; set; }

        public int ID_Competicion { get; set; }

        public int ID_Jornada { get; set; }

        public int ID_EquipoLocal { get; set; }

        public int ID_EquipoVisitante { get; set; }
    }
}
