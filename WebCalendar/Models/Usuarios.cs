using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebCalendar.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class Usuarios
    {
        [Key]
        public int ID_Usuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios")]
        [StringLength(200)]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress]
        [StringLength(500)]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string KeyPass { get; set; }

        [DefaultValue(false)]
        public bool Validado { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime? FechaRegistro { get; set; }

        public DateTime? FechaValidado { get; set; }

        public DateTime? FechaActualizado { get; set; }
    }
}
