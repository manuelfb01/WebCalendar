using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebCalendar.Datos;
using WebCalendar.Models;

namespace WebCalendar.Controllers
{
    [Authorize]
    public class RestriccionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestriccionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Restricciones
        public async Task<IActionResult> RestriccionesIndex(int? id)
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
            ViewData["idCompeticion"] = id;

            var competiciones = _context.Competiciones.Where(c => c.ID_Competicion == id);
            Competiciones competicion = competiciones.First();
            ViewData["NombreCompeticion"] = competicion.NombreCompeticion;

            //var applicationDbContext = _context.Restricciones.Include(r => r.Competicion).Include(r => r.TipoRestriccion);
            var listaRestricciones = _context.Restricciones.Where(r => r.ID_Competicion == id).OrderBy(r => r.ID_Jornada);
            IList<TiposRestricciones> listaTiposRestricciones = await _context.TiposRestricciones.ToListAsync();
            IList<Jornadas> listaJornadas = await _context.Jornadas.Where(r => r.ID_Competicion == id).ToListAsync();
            IList<Equipos> listaEquipos = await _context.Equipos.Where(r => r.ID_Competicion == id).ToListAsync();
            ViewBag.TiposRestricciones = listaTiposRestricciones;
            ViewBag.Jornadas = listaJornadas;
            ViewBag.Equipos = listaEquipos;

            return View(await listaRestricciones.ToListAsync());
        }

        // GET: Restricciones/Create
        [HttpGet]
        public IActionResult RestriccionesCrear(int? id)
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
            ViewData["idCompeticion"] = id;

            var tiposRestricciones = _context.TiposRestricciones;
            ViewData["ID_Tipo_Restriccion"] = new SelectList(tiposRestricciones, "ID_Tipo_Restriccion", "Tipo_Restriccion");

            var competicionActual = _context.Competiciones.Where(e => e.ID_Competicion == id);
            ViewData["ID_Competicion"] = new SelectList(competicionActual, "ID_Competicion", "NombreCompeticion");

            var jornadasCompeticion = _context.Jornadas.Where(e => e.ID_Competicion == id);
            ViewData["ID_Jornada"] = new SelectList(jornadasCompeticion, "ID_Jornada", "Num_Jornada");

            var equipoLocal = _context.Equipos.Where(e => e.ID_Competicion == id).OrderBy(e => e.NombreEquipo);
            ViewData["ID_Equipo"] = new SelectList(equipoLocal, "ID_Equipo", "NombreEquipo");

            var equipoRival = _context.Equipos.Where(e => e.ID_Competicion == id).OrderBy(e => e.NombreEquipo);
            ViewData["ID_EquipoRival"] = new SelectList(equipoRival, "ID_Equipo", "NombreEquipo");

            //ViewData["ID_Competicion"] = new SelectList(_context.Competiciones, "ID_Competicion", "NombreCompeticion");
            //ViewData["ID_Tipo_Restriccion"] = new SelectList(_context.TiposRestricciones, "Id_Tipo_Restriccion", "Tipo_Restriccion");


            return View();
        }

        // POST: Restricciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestriccionesCrear([Bind("Id_Restriccion,ID_Tipo_Restriccion,ID_Competicion,ID_Jornada,ID_Equipo,ID_EquipoRival")] Restricciones restricciones)
        {
            if (ModelState.IsValid)
            {
                if (restricciones.ID_Tipo_Restriccion < 3)
                {
                    restricciones.ID_EquipoRival = null;
                }
                _context.Add(restricciones);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(RestriccionesIndex), new { id = restricciones.ID_Competicion });
            }
            ViewData["ID_Competicion"] = new SelectList(_context.Competiciones, "ID_Competicion", "NombreCompeticion", restricciones.ID_Competicion);
            ViewData["ID_Tipo_Restriccion"] = new SelectList(_context.TiposRestricciones, "Id_Tipo_Restriccion", "Tipo_Restriccion", restricciones.ID_Tipo_Restriccion);
            return View(restricciones);
        }

        // GET: Restricciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Restricciones == null)
            {
                return NotFound();
            }

            var restricciones = await _context.Restricciones.FindAsync(id);
            if (restricciones == null)
            {
                return NotFound();
            }
            ViewData["ID_Competicion"] = new SelectList(_context.Competiciones, "ID_Competicion", "NombreCompeticion", restricciones.ID_Competicion);
            ViewData["ID_Tipo_Restriccion"] = new SelectList(_context.TiposRestricciones, "Id_Tipo_Restriccion", "Tipo_Restriccion", restricciones.ID_Tipo_Restriccion);
            return View(restricciones);
        }

        // POST: Restricciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Restriccion,ID_Tipo_Restriccion,ID_Competicion,ID_Jornada,ID_EquipoLocal,ID_EquipoVisitante")] Restricciones restricciones)
        {
            if (id != restricciones.ID_Restriccion)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restricciones);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestriccionesExists(restricciones.ID_Restriccion))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ID_Competicion"] = new SelectList(_context.Competiciones, "ID_Competicion", "NombreCompeticion", restricciones.ID_Competicion);
            ViewData["ID_Tipo_Restriccion"] = new SelectList(_context.TiposRestricciones, "Id_Tipo_Restriccion", "Tipo_Restriccion", restricciones.ID_Tipo_Restriccion);
            return View(restricciones);
        }

        // GET: Restricciones/Delete/5
        public async Task<IActionResult> RestriccionesBorrar(int? id)
        {
            if (id == null || _context.Restricciones == null)
            {
                return NotFound();
            }

            var restriccion = await _context.Restricciones
                .Include(r => r.Competicion)
                .Include(r => r.TipoRestriccion)
                .FirstOrDefaultAsync(m => m.ID_Restriccion == id);
            if (restriccion == null)
            {
                return NotFound();
            }

            Competiciones competicion = await _context.Competiciones.Where(c => c.ID_Competicion == restriccion.ID_Competicion).FirstOrDefaultAsync();
            TiposRestricciones tipoRestriccion = await _context.TiposRestricciones.Where(t => t.ID_Tipo_Restriccion == restriccion.ID_Tipo_Restriccion).FirstOrDefaultAsync();
            Jornadas jornada = await _context.Jornadas.Where(j => j.ID_Jornada == restriccion.ID_Jornada).FirstOrDefaultAsync();
            Equipos equipo = await _context.Equipos.Where(e => e.ID_Equipo == restriccion.ID_Equipo).FirstOrDefaultAsync();
            Equipos equipoRival = await _context.Equipos.Where(e => e.ID_Equipo == restriccion.ID_EquipoRival).FirstOrDefaultAsync();
            ViewBag.Competicion = competicion;
            ViewBag.TipoRestriccion = tipoRestriccion;
            ViewBag.Jornada = jornada;
            ViewBag.Equipo = equipo;
            ViewBag.EquipoRival = equipoRival;

            return View(restriccion);
        }

        // POST: Restricciones/Delete/5
        [HttpPost, ActionName("RestriccionesBorrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestriccionesBorrarConfirmacion(int id)
        {
            if (_context.Restricciones == null)
            {
                return Problem("No hay restricciones para borrar.");
            }
            var restricciones = await _context.Restricciones.FindAsync(id);
            var idCompeticion = restricciones.ID_Competicion;
            if (restricciones != null)
            {
                _context.Restricciones.Remove(restricciones);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(RestriccionesIndex), new {id = idCompeticion});
        }

        private bool RestriccionesExists(int id)
        {
          return (_context.Restricciones?.Any(e => e.ID_Restriccion == id)).GetValueOrDefault();
        }
    }
}
