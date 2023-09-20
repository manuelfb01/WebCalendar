using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;
using WebCalendar.Models;

namespace WebCalendar.Datos
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly ApplicationDbContext _dbContext;

        public UsuarioServicio(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Usuarios> GetUsuario(string correo, string clave)
        {
            Usuarios usuario_encontrado = await _dbContext.Usuarios.Where(u => u.Email == correo && u.KeyPass == clave).FirstOrDefaultAsync();

            return usuario_encontrado;
        }

        public async Task<Usuarios> GuardarUsuario(Usuarios modelo)
        {
            _dbContext.Usuarios.Add(modelo);
            await _dbContext.SaveChangesAsync();
            return modelo;
        }
    }
}
