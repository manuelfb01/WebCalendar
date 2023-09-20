using Microsoft.EntityFrameworkCore;
using WebCalendar.Models;

namespace WebCalendar.Datos
{
    public interface IUsuarioServicio
    {
        Task<Usuarios> GetUsuario(string correo, string clave);

        Task<Usuarios> GuardarUsuario(Usuarios modelo);

    }
}
