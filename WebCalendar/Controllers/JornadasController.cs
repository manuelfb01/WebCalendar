using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebCalendar.Datos;
using WebCalendar.Models;

namespace WebCalendar.Controllers
{
    [Authorize]
    public class JornadasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JornadasController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool CrearJornadas(IList<Jornadas> jornadas)
        {
            int jornadasCreadas = 0;
            foreach(Jornadas jor in jornadas)
            {
                _context.Jornadas.Add(jor);
                jornadasCreadas = jornadasCreadas + _context.SaveChanges();
            }

            return jornadas.Count() == jornadasCreadas;
        }

        [HttpGet]
        public async Task<IActionResult> JornadasCrear(int? id)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            int idUsuario = 0;
            string emailUsuario = "";
            string nombreUsuario = "";
            string idCompeticion = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idUsuario = Convert.ToInt32(claimuser.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault());
                emailUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            ViewData["idUsuario"] = idUsuario;
            ViewData["emailUsuario"] = emailUsuario;
            ViewData["nombreUsuario"] = nombreUsuario;

            if (id == null)
            {
                ViewData["mensaje"] = "No existe la competición";
                View();
            }
            var equipos = await _context.Equipos.Where(e => e.ID_Competicion == id).ToListAsync();
            var jornadasCreadas = await _context.Jornadas.Where(j => j.ID_Competicion == id).ToListAsync();
            if (jornadasCreadas != null && jornadasCreadas.Count() > 0)
            {
                ViewData["mensaje"] = "La competición ya tiene jornadas creadas. Bórrelas para volver a crearlas";
                return View();
            }

            int numEquipos = equipos.Count();
            int numJornadas = (numEquipos-1)*2;
            int numVuelta = 1;
            IList<Jornadas> jornadas = new List<Jornadas>();
            //Jornadas jornada = null;
            for (int i = 1;i <= numJornadas/2;i++)
            {
                Jornadas jornada = new Jornadas();
                jornada.Num_Jornada = i;
                jornada.Num_Vuelta = numVuelta;
                jornada.ID_Competicion = (int)id;
                jornadas.Add(jornada);
            }

            numVuelta = 2;
            for (int i = numJornadas/2+1; i <= numJornadas; i++)
            {
                Jornadas jornada = new Jornadas();
                jornada.Num_Jornada = i;
                jornada.Num_Vuelta = numVuelta;
                jornada.ID_Competicion = (int)id;
                jornadas.Add(jornada);
            }

            bool creadas = CrearJornadas(jornadas);

            if (!creadas)
            {
                ViewData["mensaje"] = "Ha ocurrido un error al crear las jornadas";
                return View();
            }

            return RedirectToAction("CompeticionesIndex", "Competiciones");
        }

        [HttpGet]
        public async Task<IActionResult> JornadasBorrar(int? id)
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            int idUsuario = 0;
            string emailUsuario = "";
            string nombreUsuario = "";
            string idCompeticion = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                idUsuario = Convert.ToInt32(claimuser.Claims.Where(c => c.Type == ClaimTypes.PrimarySid).Select(c => c.Value).SingleOrDefault());
                emailUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).SingleOrDefault();
                nombreUsuario = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault();
            }

            ViewData["idUsuario"] = idUsuario;
            ViewData["emailUsuario"] = emailUsuario;
            ViewData["nombreUsuario"] = nombreUsuario;

            IList<Jornadas> jornadasCompeticion = await _context.Jornadas.Where(j => j.ID_Competicion == id).ToListAsync();
            if (jornadasCompeticion.Count == 0 || jornadasCompeticion == null)
            {
                ViewData["mensaje"] = "La competición no tiene jornadas";
                return View();
            }

            _context.Jornadas.RemoveRange(jornadasCompeticion);
            await _context.SaveChangesAsync();

            return RedirectToAction("CompeticionesIndex", "Competiciones");
        }



        public IActionResult Index()
        {
            return View();
        }
    }
}
