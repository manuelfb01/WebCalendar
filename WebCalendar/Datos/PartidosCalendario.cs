using System.ComponentModel.DataAnnotations;

namespace WebCalendar.Datos
{
    public class PartidosCalendario
    {
        public PartidosCalendario(int idJornada, int idEquipoLocal, int idEquipoVisitante)
        {
            ID_Jornada = idJornada;
            ID_EquipoLocal = idEquipoLocal;
            ID_EquipoVisitante = idEquipoVisitante;
        }

        public int ID_Partido { get; set; }

        public int ID_Competicion { get; set; }

        public int ID_Jornada { get; set; }

        public int ID_EquipoLocal { get; set; }

        public int ID_EquipoVisitante { get; set; }
    }
}
