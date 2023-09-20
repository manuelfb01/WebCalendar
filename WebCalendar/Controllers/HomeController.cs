using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebCalendar.Datos;
using WebCalendar.Models;

namespace WebCalendar.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsuarioServicio _usuarioServicio;
        private readonly ApplicationDbContext _contexto;

        public HomeController(IUsuarioServicio usuarioServicio, ApplicationDbContext contexto)
        {
            _usuarioServicio = usuarioServicio;
            _contexto = contexto;
        }

        public IActionResult Index()
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
            return View();
        }

        public IActionResult Privacy()
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
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}