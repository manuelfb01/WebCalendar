using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCalendar.Datos;
using WebCalendar.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebCalendar.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly ApplicationDbContext _context;

        public UsuariosController(IUsuarioServicio usuarioServicio, ApplicationDbContext context)
        {
            _usuarioServicio = usuarioServicio;
            _context = context;
        }

        // GET: Usuarios
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            int idUsuario = 0;
            string emailUsuario = "";
            string nombreUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idUsuario = Convert.ToInt32(claimuser.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault());
                emailUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            ViewData["idUsuario"] = idUsuario;
            ViewData["emailUsuario"] = emailUsuario;
            ViewData["nombreUsuario"] = nombreUsuario;

            return _context.Usuarios != null ?
                          View(await _context.Usuarios.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Usuarios'  is null.");
        }

        // GET: Usuarios/UsuarioCrear
        [HttpGet]
        public IActionResult UsuariosCrear()
        {
            return View();
        }

        // POST: Usuarios/UsuarioCrear
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsuariosCrear([Bind("ID_Usuario,Nombre,Apellidos,Email,KeyPass,Validado,FechaRegistro,FechaValidado,FechaActualizado")] Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                Usuarios usuarioExistente = _context.Usuarios.Where(u => u.Email == usuario.Email).FirstOrDefault();
                if (usuarioExistente == null)
                {
                    usuario.KeyPass = Utilidades.EncriptarKeyPass(usuario.KeyPass);
                    usuario.FechaRegistro = DateTime.Now;

                    Usuarios usuario_creado = await _usuarioServicio.GuardarUsuario(usuario);
                    return RedirectToAction("UsuariosIniciarSesion", "Usuarios");
                }
                else
                {
                    ViewData["Duplicado"] = "El usuario ya existe. Inicie sesión.";
                    return View(usuario);
                }

                
            }
            return View(usuario);
        }

        // GET: Usuarios/UsuarioEditar/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UsuariosEditar(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuarios = await _context.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound();
            }
            ClaimsPrincipal claimuser = HttpContext.User;
            int idUsuario = 0;
            string emailUsuario = "";
            string nombreUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idUsuario = Convert.ToInt32(claimuser.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault());
                emailUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            ViewData["idUsuario"] = idUsuario;
            ViewData["emailUsuario"] = emailUsuario;
            ViewData["nombreUsuario"] = nombreUsuario;

            return View(usuarios);
        }

        // POST: Usuarios/UsuarioEditar/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsuariosEditar(int id, [Bind("ID_Usuario,Nombre,Apellidos,Email,KeyPass,Validado,FechaRegistro,FechaValidado")] Usuarios usuario, string emailAnterior)
        {
            if (id != usuario.ID_Usuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.FechaActualizado = DateTime.Now;
                    if(usuario.Email != emailAnterior)
                    {
                        usuario.Validado = false;
                    }
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuariosExists(usuario.ID_Usuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
                //return RedirectToAction("UsuarioEditar", "Usuarios");
            }
            return View(usuario);
            
        }

        // GET: Usuarios/Delete/5
        [HttpGet]
        public async Task<IActionResult> UsuariosBorrar(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            ClaimsPrincipal claimuser = HttpContext.User;
            int idUsuario = 0;
            string emailUsuario = "";
            string nombreUsuario = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idUsuario = Convert.ToInt32(claimuser.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault());
                emailUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            ViewData["idUsuario"] = idUsuario;
            ViewData["emailUsuario"] = emailUsuario;
            ViewData["nombreUsuario"] = nombreUsuario;

            var usuarios = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.ID_Usuario == id);
            if (usuarios == null)
            {
                return NotFound();
            }

            return View(usuarios);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("UsuariosBorrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsuariosBorrarConfirmacion(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("No hay usuarios para borrar.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                IList<Competiciones> listaCompeticionesUsuario = await _context.Competiciones.Where(c => c.ID_Usuario == id).ToListAsync();
                if (listaCompeticionesUsuario.Count > 0)
                {
                    foreach(var competicion in listaCompeticionesUsuario)
                    {
                        //Eliminamos los partidos de la competición si existen
                        IList<Partidos> listaPartidosCompeticion = await _context.Partidos.Where(p => p.ID_Competicion == competicion.ID_Competicion).ToListAsync();
                        if (listaPartidosCompeticion.Count > 0)
                        {
                            _context.Partidos.RemoveRange(listaPartidosCompeticion);
                            await _context.SaveChangesAsync();
                        }

                        //Eliminamos las restricciones de la competición si existen
                        IList<Restricciones> listaRestriccionesCompeticion = await _context.Restricciones.Where(r => r.ID_Competicion == competicion.ID_Competicion).ToListAsync();
                        if (listaRestriccionesCompeticion.Count > 0)
                        {
                            _context.Restricciones.RemoveRange(listaRestriccionesCompeticion);
                            await _context.SaveChangesAsync();
                        }

                        // Eliminamos las jornadas de la competición si existen
                        IList<Jornadas> listaJornadasCompeticion = await _context.Jornadas.Where(j => j.ID_Competicion == competicion.ID_Competicion).ToListAsync();
                        if (listaJornadasCompeticion.Count > 0)
                        {
                            _context.Jornadas.RemoveRange(listaJornadasCompeticion);
                            await _context.SaveChangesAsync();
                        }

                        // Eliminamos los equipos de la competición si existen
                        IList<Equipos> listaEquiposCompeticion = await _context.Equipos.Where(e => e.ID_Competicion == competicion.ID_Competicion).ToListAsync();
                        if (listaEquiposCompeticion.Count > 0)
                        {
                            _context.Equipos.RemoveRange(listaEquiposCompeticion);
                            await _context.SaveChangesAsync();
                        }
                    }

                    //Cuando se ha eliminado todo lo referente a las competiciones del usuario, eliminamos todas las competiciones del usuario
                    _context.Competiciones.RemoveRange(listaCompeticionesUsuario);
                    await _context.SaveChangesAsync();
                }
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(UsuariosIniciarSesion));
        }

        [HttpGet]
        public IActionResult UsuariosIniciarSesion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsuariosIniciarSesion(string correo, string clave)
        {
            Usuarios usuario_encontrado = await _usuarioServicio.GetUsuario(correo, Utilidades.EncriptarKeyPass(clave));

            if (usuario_encontrado == null)
            {
                ViewData["Mensaje"] = "Usuario o contraseña no válidos";
                return View();
            }

            List<Claim> claims = new List<Claim>(){
                new Claim(ClaimTypes.PrimarySid, usuario_encontrado.ID_Usuario.ToString()),
                new Claim(ClaimTypes.Name, usuario_encontrado.Nombre),
                new Claim(ClaimTypes.Email, usuario_encontrado.Email)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            AuthenticationProperties propiedades = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                propiedades);

            return RedirectToAction("CompeticionesIndex", "Competiciones");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UsuariosCerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("UsuariosIniciarSesion", "Usuarios");
        }

        private bool UsuariosExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.ID_Usuario == id)).GetValueOrDefault();
        }
    }
}
