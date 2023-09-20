namespace WebCalendar.Datos
{
    public class RestriccionesCalendario
    {
        public RestriccionesCalendario(int tipoRestriccion, int jornada, int equipo, int equipoRival)
        {
            TipoRestriccion = tipoRestriccion;
            Jornada = jornada;
            Equipo = equipo;
            EquipoRival = equipoRival;
        }

        public int TipoRestriccion {  get; set; }
        public int Jornada { get; set; }
        public int Equipo { get; set; }
        public int EquipoRival { get; set; }
    }
}
