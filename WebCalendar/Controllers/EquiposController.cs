using System;
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

namespace WebCalendar.Controllers
{
    [Authorize]
    public class EquiposController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EquiposController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Equipos
        [HttpGet]
        public async Task<IActionResult> EquiposIndex(int? id)
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

            var listaEquipos = _context.Equipos.Where(e => e.ID_Competicion == id);
            return View(await listaEquipos.ToListAsync());
        }

        // GET: Equipos/Details/5
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipos = await _context.Equipos
                .Include(e => e.Competicion)
                .FirstOrDefaultAsync(m => m.ID_Equipo == id);
            if (equipos == null)
            {
                return NotFound();
            }

            return View(equipos);
        }*/

        // GET: Equipos/Create
        [HttpGet]
        public IActionResult EquiposCrear(int ?id)
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

            //ViewData["ID_Competicion"] = new SelectList(_context.Competiciones, "ID_Competicion", "NombreCompeticion");

            var competicionActual = _context.Competiciones.Where(e => e.ID_Competicion == id);
            ViewData["ID_Competicion"] = new SelectList(competicionActual, "ID_Competicion", "NombreCompeticion");


            return View();
        }

        // POST: Equipos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EquiposCrear([Bind("ID_Equipo,NombreEquipo,ID_Competicion")] Equipos equipos)
        {
            if (ModelState.IsValid)
            {
                Equipos equipoExistente = _context.Equipos.Where(e => e.ID_Competicion == equipos.ID_Competicion).Where(e => e.NombreEquipo == equipos.NombreEquipo).FirstOrDefault();
                if (equipoExistente == null)
                {
                    _context.Add(equipos);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(EquiposIndex), new { id = equipos.ID_Competicion });
                }

                ViewData["Duplicado"] = "Ya existe un equipo con ese nombre.";
                ViewData["ID_Competicion"] = new SelectList(_context.Competiciones, "ID_Competicion", "NombreCompeticion", equipos.ID_Competicion);
                ViewData["idCompeticion"] = equipos.ID_Competicion;
                return View(equipos);

            }
            ViewData["ID_Competicion"] = new SelectList(_context.Competiciones, "ID_Competicion", "NombreCompeticion", equipos.ID_Competicion);
            return View(equipos);
        }

        // GET: Equipos/Edit/5
        [HttpGet]
        public async Task<IActionResult> EquiposEditar(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipos = await _context.Equipos.FindAsync(id);
            if (equipos == null)
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

            ViewData["ID_Competicion"] = new SelectList(_context.Competiciones, "ID_Competicion", "NombreCompeticion", equipos.ID_Competicion);
            return View(equipos);
        }

        // POST: Equipos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EquiposEditar(int id, [Bind("ID_Equipo,NombreEquipo,ID_Competicion")] Equipos equipos)
        {
            if (id != equipos.ID_Equipo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Equipos.Update(equipos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EquiposExists(equipos.ID_Equipo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(EquiposIndex), new {id = equipos.ID_Competicion});
            }

            /*ClaimsPrincipal claimuser = HttpContext.User;
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
            ViewData["nombreUsuario"] = nombreUsuario;*/

            ViewData["ID_Competicion"] = new SelectList(_context.Competiciones, "ID_Competicion", "NombreCompeticion", equipos.ID_Competicion);
            return View(equipos);
        }

        // GET: Equipos/Delete/5
        [HttpGet]
        public async Task<IActionResult> EquiposBorrar(int? id)
        {
            if (id == null || _context.Equipos == null)
            {
                return NotFound();
            }

            var equipos = await _context.Equipos
                .Include(e => e.Competicion)
                .FirstOrDefaultAsync(m => m.ID_Equipo == id);
            if (equipos == null)
            {
                return NotFound();
            }

            return View(equipos);
        }

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("EquiposBorrar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EquiposBorrarConfirmacion(int id)
        {
            if (_context.Equipos == null)
            {
                return Problem("No hay equipos para borrar");
            }
            var equipos = await _context.Equipos.FindAsync(id);
            var idCompeticion = equipos.ID_Competicion;
            if (equipos != null)
            {
                _context.Equipos.Remove(equipos);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EquiposIndex), new { id = idCompeticion });
        }

        private bool EquiposExists(int id)
        {
          return (_context.Equipos?.Any(e => e.ID_Equipo == id)).GetValueOrDefault();
        }
    }
}
