using Microsoft.AspNetCore.Mvc;
using ProyectoCapstone.Data;
using ProyectoCapstone.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ProyectoCapstone.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _db;
        public HomeController(ILogger<HomeController> logger, Context db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        public IActionResult Confirmar()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Inicio()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            string nombreVoluntario = user.nombre;
            ViewBag.id = user.UsuarioID;
            IEnumerable<BlocCita> citasPendientes = _db.blocCitas;
            IEnumerable<Recomendacion> recomendaciones=_db.recomendaciones;
            string nombreUsuario = user.nombre;
            if (citasPendientes.Count(c => c.nombreVoluntario.Equals(nombreVoluntario)) == 1)
            {
                ViewBag.notificacion = "Hola, " + user.nombre + " Tienes una cita pendiente porfavor haga click para confirmar";
            }
            string tiempo = DateTime.Now.ToString("HH:mm tt");
            string diaActual = DateTime.Now.ToString("dd");
            string mesActual = DateTime.Now.ToString("MM");
            string diaCita = "";
            string mesCita = "";
            string TiempoCita = "";
            string horaActual = "";
            string horaInicioCita = "";
            int timeCita;
            int hoursInicio;
            int horaCita;
            int dayCita;
            int monthCita;
            for (int i = 0; i < tiempo.Length; i++)
            {
                if (tiempo[i] == 48 || tiempo[i] == 49 || tiempo[i] == 50 || tiempo[i] == 51 || tiempo[i] == 52 || tiempo[i] == 53 || tiempo[i] == 54 || tiempo[i] == 55 || tiempo[i] == 56 || tiempo[i] == 57)
                {
                    horaActual = horaActual + tiempo[i];
                }
            }
            int diaActualLocal = Int32.Parse(diaActual);
            int mesActualLocal = Int32.Parse(mesActual);
            int horaActualLocal = Int32.Parse(horaActual);
            var cita = _db.cita.Where(c => c.nombreDiscapacitado.Equals(nombreUsuario)).ToList();
            if (recomendaciones.Count(r => r.nombreDiscapacitado.Equals(nombreUsuario)) == 0)
            {
                if (cita.Count() > 0)
                {
                    foreach (var item in cita)
                    {
                        diaCita = "";
                        mesCita = "";
                        TiempoCita = "";
                        horaInicioCita = "";
                        timeCita = 0;
                        hoursInicio = 0;
                        horaCita = 0;
                        dayCita = 0;
                        monthCita = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            diaCita = diaCita + item.fecha[i];
                        }
                        for (int i = 3; i < 5; i++)
                        {
                            mesCita = mesCita + item.fecha[i];
                        }
                        for (int i = 0; i < item.tiempoCita.Length; i++)
                        {
                            if (item.tiempoCita[i] == 48 || item.tiempoCita[i] == 49 || item.tiempoCita[i] == 50 || item.tiempoCita[i] == 51 || item.tiempoCita[i] == 52 || item.tiempoCita[i] == 53 || item.tiempoCita[i] == 54 || item.tiempoCita[i] == 55 || item.tiempoCita[i] == 56 || item.tiempoCita[i] == 57)
                            {
                                TiempoCita = TiempoCita + item.tiempoCita[i];
                            }
                        }
                        for (int i = 0; i < item.horaInicio.Length; i++)
                        {
                            if (item.horaInicio[i] == 48 || item.horaInicio[i] == 49 || item.horaInicio[i] == 50 || item.horaInicio[i] == 51 || item.horaInicio[i] == 52 || item.horaInicio[i] == 53 || item.horaInicio[i] == 54 || item.horaInicio[i] == 55 || item.horaInicio[i] == 56 || item.horaInicio[i] == 57)
                            {
                                horaInicioCita = horaInicioCita + item.horaInicio[i];
                            }
                        }
                        timeCita = Int32.Parse(TiempoCita);
                        hoursInicio = Int32.Parse(horaInicioCita);
                        horaCita = hoursInicio + timeCita;
                        dayCita = Int32.Parse(diaCita);
                        monthCita = Int32.Parse(mesCita);
                        if (horaActualLocal >= horaCita && diaActualLocal >= dayCita && mesActualLocal>=monthCita)
                        {
                            var citaDiscapacitado = _db.cita.Where(c => c.fecha.Equals(item.fecha)).FirstOrDefault();
                            ViewBag.nombreVoluntario = citaDiscapacitado.nombreVoluntario;
                            ViewBag.recomendacion = "recomendacion";
                        }
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult Recomendacion(Recomendacion recomendacion)
        {
            var user = obtenerUsuario();
            recomendacion.nombreDiscapacitado = user.nombre;
            var voluntario = _db.usuarios.Where(u => u.nombre.Equals(recomendacion.nombreVoluntario)).FirstOrDefault();
            voluntario.recomendacion = voluntario.recomendacion + recomendacion.calificacion;
            if (ModelState.IsValid)
            {
                _db.recomendaciones.Add(recomendacion);
                _db.SaveChanges();
                return RedirectToAction("Inicio", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Cerrar(Recomendacion recomendacion)
        {
            var user = obtenerUsuario();
            recomendacion.nombreDiscapacitado = user.nombre;
            if (ModelState.IsValid)
            {
                _db.recomendaciones.Add(recomendacion);
                _db.SaveChanges();
                return RedirectToAction("Inicio", "Home");
            }
            return View();
        }
        public IActionResult QuienesSomos()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        public IActionResult QueHacemos()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        public IActionResult Contactanos()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        [HttpPost]
        public IActionResult Contactanos(Contactanos contactanos)
        {
            var user = obtenerUsuario();
            if (ModelState.IsValid)
            {
                _db.contactanos.Add(contactanos);
                _db.SaveChanges();
                if (user == null)
                {
                    return RedirectToAction("Login", "Acceder");
                }
                else
                {
                    return RedirectToAction("Inicio", "Home");
                }
            }
            return View();
        }
        public IActionResult ListaContactanos()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            IEnumerable<Contactanos> listaContactanos = _db.contactanos;
            return View(listaContactanos);
        }
        public Usuario obtenerUsuario()
        {
            ClaimsPrincipal claim = HttpContext.User;
            int idUsuario;
            Usuario user = new Usuario();
            if (claim.Identity.IsAuthenticated)
            {
                string id = claim.Claims.Where(u => u.Type == ClaimTypes.NameIdentifier)
                                            .Select(u => u.Value).SingleOrDefault();
                idUsuario = int.Parse(id);
                user = _db.usuarios.Where(u => u.UsuarioID == idUsuario).FirstOrDefault();
                return user;
            }
            else
            {
                return user;
            }
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}