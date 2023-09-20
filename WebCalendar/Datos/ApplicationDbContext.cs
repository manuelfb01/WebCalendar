using Microsoft.EntityFrameworkCore;
using WebCalendar.Models;

namespace WebCalendar.Datos
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        //Agregamos los modelos de BD
        public DbSet<Usuarios> Usuarios { get; set; }

        public DbSet<Competiciones> Competiciones { get; set; }

        public DbSet<Equipos> Equipos { get; set; }

        public DbSet<Jornadas> Jornadas { get; set; }

        public DbSet<TiposRestricciones> TiposRestricciones { get; set; }

        public DbSet<Restricciones> Restricciones { get; set; }

        public DbSet<Partidos> Partidos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TiposRestricciones>().HasData(
                new TiposRestricciones() { ID_Tipo_Restriccion = 1, Tipo_Restriccion = "Equipo tiene que ser local" },
                new TiposRestricciones() { ID_Tipo_Restriccion = 2, Tipo_Restriccion = "Equipo tiene que ser visitante" },
                new TiposRestricciones() { ID_Tipo_Restriccion = 3, Tipo_Restriccion = "Equipos se tienen que enfrentar" },
                new TiposRestricciones() { ID_Tipo_Restriccion = 4, Tipo_Restriccion = "Equipos NO se tienen que enfrentar" }
                );
        }
    }
}
