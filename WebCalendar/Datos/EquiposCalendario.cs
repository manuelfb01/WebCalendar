namespace WebCalendar.Datos
{
    public class EquiposCalendario
    {
        public EquiposCalendario(int idEquipo, int numEquipo)
        {
            ID_Equipo = idEquipo;
            NumEquipo = numEquipo;
        }

        public int ID_Equipo { get; set; }

        public int NumEquipo { get; set; }
    }
}
