using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebCalendar.Datos;
using WebCalendar.Models;
using System.Text.Json;
using Google.OrTools.Sat;
using SportsSchedulingSat;
using NuGet.Packaging;

namespace WebCalendar.Controllers
{
    [Authorize]
    public class CompeticionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompeticionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Competiciones
        [HttpGet]
        public async Task<IActionResult> CompeticionesIndex()
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

            IList<Competiciones> listaCompeticiones = await _context.Competiciones.Where(c => c.ID_Usuario == idUsuario).ToListAsync();
            IList<Equipos> listaEquipos = await _context.Equipos.ToListAsync();
            IList<Jornadas> listaJornadas = await _context.Jornadas.ToListAsync();
            IList<Restricciones> listaRestricciones = await _context.Restricciones.ToListAsync();
            IList<Partidos> listaPartidos = await _context.Partidos.ToListAsync();

            //ViewBag.competiciones = listaCompeticiones;
            ViewBag.equipos = listaEquipos;
            ViewBag.jornadas = listaJornadas;
            ViewBag.restricciones = listaRestricciones;
            ViewBag.partidos = listaPartidos;
            return View(listaCompeticiones);
        }

        // GET: Competiciones/Details/5
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Competiciones == null)
            {
                return NotFound();
            }

            var competiciones = await _context.Competiciones
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.ID_Competicion == id);
            if (competiciones == null)
            {
                return NotFound();
            }

            return View(competiciones);
        }*/

        // GET: Competiciones/CompeticionesCrear
        [HttpGet]
        public IActionResult CompeticionesCrear()
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

            var usuarioActual = _context.Usuarios.Where(c => c.ID_Usuario == idUsuario);

            //ViewData["ID_Usuario"] = new SelectList(_context.Usuarios, "ID_Usuario", "Apellidos");
            ViewData["ID_Usuario"] = new SelectList(usuarioActual, "ID_Usuario", "Apellidos");
            return View();
        }

        // POST: Competiciones/CompeticionesCrear
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompeticionesCrear([Bind("ID_Competicion,NombreCompeticion,Jor_Rep_Enfrentamiento,Alternar_Local_Vuelta,Restricciones,ID_Usuario")] Competiciones competicion)
        {
            if (ModelState.IsValid)
            {
                Competiciones competicionExistente = _context.Competiciones.Where(c => c.ID_Usuario == competicion.ID_Usuario).Where(c => c.NombreCompeticion == competicion.NombreCompeticion).FirstOrDefault();
                if (competicionExistente == null)
                {
                    _context.Add(competicion);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(CompeticionesIndex));
                }
                else
                {
                    ViewData["Duplicado"] = "Ya existe una competición con ese nombre.";
                    ViewData["ID_Usuario"] = new SelectList(_context.Usuarios, "ID_Usuario", "Apellidos", competicion.ID_Usuario);
                    return View(competicion);
                }
                
            }
            ViewData["ID_Usuario"] = new SelectList(_context.Usuarios, "ID_Usuario", "Apellidos", competicion.ID_Usuario);
            return View(competicion);
        }

        // GET: Competiciones/CompeticionesEditar/5
        [HttpGet]
        public async Task<IActionResult> CompeticionesEditar(int? id)
        {
            if (id == null || _context.Competiciones == null)
            {
                return NotFound();
            }

            var competiciones = await _context.Competiciones.FindAsync(id);
            if (competiciones == null)
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

            ViewData["ID_Usuario"] = new SelectList(_context.Usuarios, "ID_Usuario", "Apellidos", competiciones.ID_Usuario);
            return View(competiciones);
        }

        // POST: Competiciones/CompeticionesEditar/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompeticionesEditar(int id, [Bind("ID_Competicion,NombreCompeticion,Jor_Rep_Enfrentamiento,Alternar_Local_Vuelta,Restricciones,ID_Usuario")] Competiciones competiciones)
        {
            if (id != competiciones.ID_Competicion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(competiciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompeticionesExists(competiciones.ID_Competicion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(CompeticionesIndex));
            }
            ViewData["ID_Usuario"] = new SelectList(_context.Usuarios, "ID_Usuario", "Apellidos", competiciones.ID_Usuario);
            return View(competiciones);
        }

        // GET: Competiciones/CompeticionesBorrar/5
        [HttpGet]
        public async Task<IActionResult> CompeticionesBorrar(int? id)
        {
            if (id == null || _context.Competiciones == null)
            {
                return NotFound();
            }

            var competiciones = await _context.Competiciones
                .Include(c => c.Cliente)
                .FirstOrDefaultAsync(m => m.ID_Competicion == id);
            if (competiciones == null)
            {
                return NotFound();
            }

            return View(competiciones);
        }

        // POST: Competiciones/CompeticionesBorrar/5
        [HttpPost, ActionName("CompeticionesBorrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompeticionesBorrarConfirmacion(int id)
        {
            if (_context.Competiciones == null)
            {
                return Problem("No hay competiciones para borrar");
            }
            var competiciones = await _context.Competiciones.FindAsync(id);
            if (competiciones != null)
            {
                //Eliminamos los partidos de la competición si existen
                IList<Partidos> listaPartidosCompeticion = await _context.Partidos.Where(p => p.ID_Competicion == id).ToListAsync();
                if (listaPartidosCompeticion.Count > 0)
                {
                    _context.Partidos.RemoveRange(listaPartidosCompeticion);
                    await _context.SaveChangesAsync();
                }

                //Eliminamos las restricciones de la competición si existen
                IList<Restricciones> listaRestriccionesCompeticion = await _context.Restricciones.Where(r => r.ID_Competicion == id).ToListAsync();
                if (listaRestriccionesCompeticion.Count > 0)
                {
                    _context.Restricciones.RemoveRange(listaRestriccionesCompeticion);
                    await _context.SaveChangesAsync();
                }

                // Eliminamos las jornadas de la competición si existen
                IList<Jornadas> listaJornadasCompeticion = await _context.Jornadas.Where(j => j.ID_Competicion == id).ToListAsync();
                if (listaJornadasCompeticion.Count > 0)
                {
                    _context.Jornadas.RemoveRange(listaJornadasCompeticion);
                    await _context.SaveChangesAsync();
                }

                // Eliminamos los equipos de la competición si existen
                IList<Equipos> listaEquiposCompeticion = await _context.Equipos.Where(e => e.ID_Competicion == id).ToListAsync();
                if (listaEquiposCompeticion.Count > 0)
                {
                    _context.Equipos.RemoveRange(listaEquiposCompeticion);
                    await _context.SaveChangesAsync();
                }

                _context.Competiciones.Remove(competiciones);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CompeticionesIndex));
        }

        private bool CompeticionesExists(int id)
        {
          return (_context.Competiciones?.Any(e => e.ID_Competicion == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> CompeticionesCrearCalendario(int? id)
        {
            if (id == null || _context.Competiciones == null)
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

            var competiciones = await _context.Competiciones.FirstOrDefaultAsync(c => c.ID_Competicion == id);
            if (competiciones == null)
            {
                return NotFound();
            }

            return View(competiciones);
        }

        [HttpPost]
        public async Task<IActionResult> CompeticionesCrearCalendario(int id)
        {
            var competicion = await _context.Competiciones.Where(c => c.ID_Competicion == id).FirstOrDefaultAsync();
            if (competicion == null)
            {
                return NotFound();
            }

            IList<Equipos> listaEquipos = await _context.Equipos.Where(e => e.ID_Competicion == id).OrderBy(e => e.ID_Equipo).ToListAsync();
            IList<EquiposCalendario> listaEquiposCalendario = new List<EquiposCalendario>();
            int numEquipo = 0;
            foreach (var equipo in listaEquipos)
            {
                EquiposCalendario equipoCalendario = new EquiposCalendario(equipo.ID_Equipo, numEquipo);
                listaEquiposCalendario.Add(equipoCalendario);
                numEquipo++;
            }

            IList<Restricciones> listaRestricciones = await _context.Restricciones.Where(r => r.ID_Competicion == id).ToListAsync();
            IList<RestriccionesCalendario> listaRestriccionesCalendario = new List<RestriccionesCalendario>();
            int numEquipoLista = 0;
            int numEquipoRivalLista;

            foreach (var item in listaRestricciones)
            {
                numEquipoLista = listaEquiposCalendario.Where(e => e.ID_Equipo == item.ID_Equipo).First().NumEquipo;
                numEquipoRivalLista = 0;
                if (item.ID_EquipoRival != null)
                {
                    numEquipoRivalLista = listaEquiposCalendario.Where(e => e.ID_Equipo == item.ID_EquipoRival).First().NumEquipo;
                    //Equipos equipoRival = await _context.Equipos.Where(e => e.ID_Competicion == id).Where(e => e.ID_Equipo == item.ID_EquipoRival).FirstAsync();
                }

                int numJornada = _context.Jornadas.Where(j => j.ID_Competicion == id).Where(j => j.ID_Jornada == item.ID_Jornada).First().Num_Jornada;

                RestriccionesCalendario restriccionCalendario =
                    new RestriccionesCalendario(item.ID_Tipo_Restriccion, numJornada - 1, numEquipoLista, numEquipoRivalLista);

                listaRestriccionesCalendario.Add(restriccionCalendario);
            }

            IList<PartidosCalendario> listadoPartidosCalendario = Globals.OpponentModel(listaEquipos.Count(), 60, competicion.Jor_Rep_Enfrentamiento, competicion.Num_Jor_Loc, competicion.Alternar_Local_Vuelta, listaRestriccionesCalendario);

            if (listadoPartidosCalendario != null)
            {
                foreach (var part in listadoPartidosCalendario)
                {
                    Partidos partido = new Partidos();
                    partido.ID_Competicion = id;
                    partido.ID_Jornada = part.ID_Jornada + 1;
                    partido.ID_EquipoLocal = listaEquiposCalendario.Where(e => e.NumEquipo == part.ID_EquipoLocal).First().ID_Equipo;
                    partido.ID_EquipoVisitante = listaEquiposCalendario.Where(e => e.NumEquipo == part.ID_EquipoVisitante).First().ID_Equipo;

                    _context.Add(partido);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("CompeticionesVerCalendario", "Competiciones", new { id = id });
            }
            else
            {
                ViewData["SinSolucion"] = "No hay solución de calendario para esta competición con las restricciones dadas.";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> CompeticionesVerCalendario(int id)
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

            IList<Jornadas> listaJornadas = await _context.Jornadas.Where(j => j.ID_Competicion == id).OrderBy(j => j.Num_Jornada).ToListAsync();
            IList<Equipos> listaEquipos = await _context.Equipos.Where(e => e.ID_Competicion ==id).ToListAsync();
            IList<Partidos> listaPartidosCompleta = await _context.Partidos.Where(p => p.ID_Competicion == id).OrderBy(p => p.ID_Jornada).ToListAsync();
            Competiciones competicion = await _context.Competiciones.Where(c => c.ID_Competicion == id).FirstAsync();

            ViewBag.listaEquipos = listaEquipos;
            ViewBag.listaJornadas = listaJornadas;
            ViewData["competicion"] = competicion.NombreCompeticion;

            return View(listaPartidosCompleta);
        }

        [HttpGet]
        public async Task<IActionResult> CompeticionesBorrarCalendario (int id)
        {
            if (_context.Partidos == null)
            {
                return Problem("No hay competiciones para borrar");
            }

            IList<Partidos> partidosCompeticion = await _context.Partidos.Where(p => p.ID_Competicion == id).ToListAsync();
            if (partidosCompeticion.Count > 0)
            {
                _context.Partidos.RemoveRange(partidosCompeticion);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("CompeticionesIndex", "Competiciones");
        }
    }
}
