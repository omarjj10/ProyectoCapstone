using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoCapstone.Data;
using ProyectoCapstone.Models;
using System.Security.Claims;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ProyectoCapstone.Controllers
{
    [Authorize]
    public class CitaController : Controller
    {
        private readonly Context _db;
        public CitaController(Context db)
        {
            _db = db;
        }
        public IActionResult Index(string?advertencia,string? mismoVoluntario)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            var voluntario = _db.usuarios.Where(u => u.RolID == 2 && u.estado.Equals("disponible")).ToList();
            List<Usuario> listaVoluntario=voluntario.OrderByDescending(x => x.recomendacion).ToList();
            if (advertencia != null)
            {
                ViewBag.advertencia = advertencia;
            }
            if (mismoVoluntario != null)
            {
                ViewBag.mismoVoluntario = mismoVoluntario;
            }
            return View(listaVoluntario);
        }
        [HttpGet]
        public IActionResult SolicitarCita(int? id,string? mensaje,string? mensaje2)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre+" "+user.apellido;
            ViewBag.nombre2 = user.nombre;
            ViewBag.id = user.UsuarioID;
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var voluntario = _db.usuarios.Find(id);
            if (voluntario == null)
            {
                return NotFound();
            }
            ViewBag.dia = voluntario.diaDisponible;
            ViewBag.horaInicio = voluntario.inicio;
            ViewBag.horaFin = voluntario.fin;
            ViewBag.nombreVoluntario = voluntario.nombre;
            if (mensaje != null)
            {
                ViewBag.errorHorario = mensaje;
            }
            if (mensaje2 != null)
            {
                ViewBag.errorModelo=mensaje2;
            }
            return View();
        }
        [HttpPost]
        public IActionResult SolicitarCita(BlocCita blocCita)
        {
            string fechaDiablocCita = "";
            string fechaMesblocCita = "";
            string DiaCita = "";
            string mesCita = "";
            int dayCita;
            int monthCita;
            string horarioCita = "";
            if (ModelState.IsValid)
            {
                for (int i = 0; i < blocCita.horario.Length; i++)
                {
                    if (blocCita.horario[i] == 48 || blocCita.horario[i] == 49 || blocCita.horario[i] == 50 || blocCita.horario[i] == 51 || blocCita.horario[i] == 52 || blocCita.horario[i] == 53 || blocCita.horario[i] == 54 || blocCita.horario[i] == 55 || blocCita.horario[i] == 56 || blocCita.horario[i] == 57)
                    {
                        horarioCita = horarioCita + blocCita.horario[i];
                    }
                }
                int horario = Int32.Parse(horarioCita);
                var voluntario = _db.usuarios.Where(u => u.nombre == blocCita.nombreVoluntario).FirstOrDefault();
                string voluntarioInicio = "";
                string voluntarioFin = "";
                for (int i = 0; i < voluntario.inicio.Length; i++)
                {
                    if (voluntario.inicio[i] == 48 || voluntario.inicio[i] == 49 || voluntario.inicio[i] == 50 || voluntario.inicio[i] == 51 || voluntario.inicio[i] == 52 || voluntario.inicio[i] == 53 || voluntario.inicio[i] == 54 || voluntario.inicio[i] == 55 || voluntario.inicio[i] == 56 || voluntario.inicio[i] == 57)
                    {
                        voluntarioInicio = voluntarioInicio + voluntario.inicio[i];
                    }
                }
                for (int i = 0; i < voluntario.fin.Length; i++)
                {
                    if (voluntario.fin[i] == 48 || voluntario.fin[i] == 49 || voluntario.fin[i] == 50 || voluntario.fin[i] == 51 || voluntario.fin[i] == 52 || voluntario.fin[i] == 53 || voluntario.fin[i] == 54 || voluntario.fin[i] == 55 || voluntario.fin[i] == 56 || voluntario.fin[i] == 57)
                    {
                        voluntarioFin = voluntarioFin + voluntario.fin[i];
                    }
                }
                for (int i = 0; i < 2; i++)
                {
                    fechaDiablocCita = fechaDiablocCita + blocCita.dia[i];
                }
                for (int i = 3; i < 5; i++)
                {
                    fechaMesblocCita = fechaMesblocCita + blocCita.dia[i];
                }
                int inicio = Int32.Parse(voluntarioInicio);
                int fin = Int32.Parse(voluntarioFin);
                int diablocCita = Int32.Parse(fechaDiablocCita);
                int mesblocCita = Int32.Parse(fechaMesblocCita);
                var cita = _db.cita.Where(c => c.nombreVoluntario.Equals(blocCita.nombreVoluntario)).ToList();
                //verificar si el voluntario tiene una cita pendiente en la fecha que el discapacitado desea solicitar la cita
                if (cita.Count() > 0)
                {
                    foreach (var item in cita)
                    {
                        DiaCita = "";
                        mesCita = "";
                        dayCita = 0;
                        monthCita = 0;
                        for (int i = 0; i < 2; i++)
                        {
                            DiaCita = DiaCita + item.fecha[i];
                        }
                        for (int i = 3; i < 5; i++)
                        {
                            mesCita = mesCita + item.fecha[i];
                        }
                        dayCita = Int32.Parse(DiaCita);
                        monthCita = Int32.Parse(mesCita);
                        if ((diablocCita < dayCita && mesblocCita < monthCita) || (diablocCita > dayCita && mesblocCita < monthCita) || (diablocCita == dayCita && mesblocCita == monthCita))
                        {
                            return RedirectToAction("Index", "Cita", new { advertencia = "error" });
                        }
                    }
                }
                //verificar si el usuario discapacitado solicita una cita al mismo voluntario dos veces seguida
                if (_db.blocCitas.Count(c => c.nombreVoluntario == blocCita.nombreVoluntario && c.nombreDiscapacitado == blocCita.nombreDiscapacitado) == 1)
                {
                    return RedirectToAction("Index", "Cita", new { mismoVoluntario = "error" });
                }
                var volu = _db.usuarios.Where(u => u.nombre == blocCita.nombreVoluntario).FirstOrDefault();
                var dis = _db.usuarios.Where(u => u.nombre == blocCita.nombreDiscapacitado).FirstOrDefault();
                if (horario >= inicio && horario <= fin)
                {
                    EnviarCorreo(blocCita,volu.correo);
                    EnviarCorreo4(blocCita, dis.correo);
                    _db.blocCitas.Add(blocCita);
                    _db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("SolicitarCita", "Cita", new { id = voluntario.UsuarioID, mensaje = "error" });
                }
            }
            else
            {
                var voluntario = _db.usuarios.Where(u => u.nombre == blocCita.nombreVoluntario).FirstOrDefault();
                return RedirectToAction("SolicitarCita","Cita",new { id = voluntario.UsuarioID, mensaje2 = "error2" });
            }
        }
        [HttpGet]
        public IActionResult ConfirmarCita()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre+" "+user.apellido;
            ViewBag.id = user.UsuarioID;
            string nombreVoluntario = user.nombre;
            BlocCita blocCita = validar(nombreVoluntario);
            ViewBag.nomDis = blocCita.nombreDiscapacitado;
            ViewBag.nomVol = blocCita.nombreVoluntario;
            ViewBag.fecha = blocCita.dia;
            ViewBag.horaInicio = blocCita.horario;
            ViewBag.duracion = blocCita.tiempoCita;
            ViewBag.lugar = blocCita.lugarEncuentro;
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmarCita(Cita cita)
        { 
            var user = obtenerUsuario();
            string nombreVoluntario = user.nombre;
            var blocCita = _db.blocCitas
                                .Where(c => c.nombreVoluntario.Equals(nombreVoluntario))
                                .FirstOrDefault();
            string nombreDiscapacitado = blocCita.nombreDiscapacitado;
            var recomendacion = _db.recomendaciones.Where(r => r.nombreDiscapacitado.Equals(nombreDiscapacitado)).ToList();
            if (recomendacion.Count > 0)
            {
                foreach(var item in recomendacion)
                {
                    item.nombreDiscapacitado = "null";
                }
            }
            var vol = _db.usuarios.Where(u => u.nombre == cita.nombreVoluntario).FirstOrDefault();
            var dis = _db.usuarios.Where(u => u.nombre == cita.nombreDiscapacitado).FirstOrDefault();
            if (ModelState.IsValid)
            {
                EnviarCorreo2(cita,dis.correo);
                EnviarCorreo5(cita, vol.correo);
                _db.cita.Add(cita);
                _db.blocCitas.Remove(blocCita);
                _db.SaveChanges();
                return RedirectToAction("Confirmar", "Home");
            }
            return View(cita);
        }
        [HttpPost]
        public IActionResult CancelarCita(Cita cita)
        {
            var blcoCita = _db.blocCitas.Where(c => c.nombreDiscapacitado == cita.nombreDiscapacitado).Where(c => c.nombreVoluntario == cita.nombreVoluntario).FirstOrDefault();
            var us = _db.usuarios.Where(u => u.nombre == cita.nombreDiscapacitado).FirstOrDefault();
            if (ModelState.IsValid)
            {
                EnviarCorreo3(blcoCita, us.correo);
                _db.blocCitas.Remove(blcoCita);
                _db.SaveChanges();
                return RedirectToAction("Inicio", "Home");
            }
            return View();
        }
        public IActionResult HistorialCita(int? id)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var voluntario = _db.usuarios.Find(id);
            if (voluntario == null)
            {
                return NotFound();
            }
            IEnumerable<Cita> listaCitas = _db.cita.Where(c => c.nombreVoluntario.Equals(voluntario.nombre));
            return View(listaCitas);
        }
        [HttpGet]
        public IActionResult ListaCita()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> obtenerTodos()
        {
            var todos = await _db.cita.ToListAsync();
            return Json(new { data = todos });
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int? id)
        {
            var cita = await _db.cita.FindAsync(id);
            if (cita == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            _db.cita.Remove(cita);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Cita borrado exitosamente" });
        }
        private BlocCita validar(string nombreVoluntario)
        {
            var cita = _db.blocCitas.Where(u => u.nombreVoluntario==nombreVoluntario)
                                .FirstOrDefault();
            return cita;
        }
        public void EnviarCorreo(BlocCita blocCita,string correo)
        {
            string servidor = "smtp.gmail.com";
            int puerto = 587;
            string gmailUser = "josuecalvanapon.2010@gmail.com";
            string gmailPass = "pufjlovvrvkfiwfg";
            MimeMessage msg = new();
            msg.From.Add(new MailboxAddress("Prueba", gmailUser));
            msg.To.Add(new MailboxAddress("Destino", correo));
            msg.Subject = "Cita";
            BodyBuilder cuerpoMensaje = new BodyBuilder();
            cuerpoMensaje.TextBody = "Usted tiene una solicutd de cita:\r\n Nombre del discapacitado: {blocCita.nombreDiscapacitado} \r\nNombre del voluntario: {blocCita.nombreVoluntario}\r\nFecha de encuentro: {blocCita.dia}\r\nHora de encuentro: {blocCita.horario}\r\nLugar de encuentro: {blocCita.lugarEncuentro}\r\nPorfavor Inicie sesión para confirmar la solicitud de cita.";
            cuerpoMensaje.HtmlBody = $"<p>Usted tiene una solicutd de cita:</p> <p>Nombre del discapacitado: {blocCita.nombreDiscapacitado}</p> <p>Nombre del voluntario: {blocCita.nombreVoluntario}</p> <p>Fecha de encuentro: {blocCita.dia}</p> <p>Hora de encuentro: {blocCita.horario}</p> <p>Lugar de encuentro: {blocCita.lugarEncuentro}</p> <p>Porfavor Inicie sesión para confirmar la solicitud de cita.</p>"; 
            msg.Body = cuerpoMensaje.ToMessageBody();
            SmtpClient client = new();
            client.CheckCertificateRevocation = false;
            client.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(gmailUser, gmailPass);
            client.Send(msg);
            client.Disconnect(true);
        }
        public void EnviarCorreo4(BlocCita blocCita, string correo)
        {
            string servidor = "smtp.gmail.com";
            int puerto = 587;
            string gmailUser = "josuecalvanapon.2010@gmail.com";
            string gmailPass = "pufjlovvrvkfiwfg";
            MimeMessage msg = new();
            msg.From.Add(new MailboxAddress("Prueba", gmailUser));
            msg.To.Add(new MailboxAddress("Destino", correo));
            msg.Subject = "Cita";
            BodyBuilder cuerpoMensaje = new BodyBuilder();
            cuerpoMensaje.TextBody = "Su Solicitud de cita fue exitosa\r\nPorfavor espere a que el voluntario confirme la cita";
            cuerpoMensaje.HtmlBody = $"<p>Su Solicitud de cita fue exitosa</p> <p>Porfavor espere a que el voluntario confirme la cita</p>";
            msg.Body = cuerpoMensaje.ToMessageBody();
            SmtpClient client = new();
            client.CheckCertificateRevocation = false;
            client.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(gmailUser, gmailPass);
            client.Send(msg);
            client.Disconnect(true);
        }
        public void EnviarCorreo2(Cita cita,string correo)
        {
            string servidor = "smtp.gmail.com";
            int puerto = 587;
            string gmailUser = "josuecalvanapon.2010@gmail.com";
            string gmailPass = "pufjlovvrvkfiwfg";
            MimeMessage msg = new();
            msg.From.Add(new MailboxAddress("Prueba", gmailUser));
            msg.To.Add(new MailboxAddress("Destino", correo));
            msg.Subject = "Cita";
            BodyBuilder cuerpoMensaje = new BodyBuilder();
            cuerpoMensaje.TextBody = "Se confirmo la cita que solicito, información de la cita:\r\n Nombre del discapacitado: {cita.nombreDiscapacitado} \r\nNombre del voluntario: {cita.nombreVoluntario}\r\nFecha de encuentro: {cita.dia}\r\nHora de encuentro: {cita.horario}\r\nLugar de encuentro: {cita.lugarEncuentro}.";
            cuerpoMensaje.HtmlBody = $"<p>Se confirmo la cita que solicito, información de la cita:</p> <p>Nombre del discapacitado: {cita.nombreDiscapacitado}</p> <p>Nombre del voluntario: {cita.nombreVoluntario}</p> <p>Lugar de Encuentro: {cita.lugarEncuentro}</p> <p>Hora de encuentro: {cita.horaInicio}</p>";
            msg.Body = cuerpoMensaje.ToMessageBody();
            SmtpClient client = new();
            client.CheckCertificateRevocation = false;
            client.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(gmailUser, gmailPass);
            client.Send(msg);
            client.Disconnect(true);
        }
        public void EnviarCorreo5(Cita cita, string correo)
        {
            string servidor = "smtp.gmail.com";
            int puerto = 587;
            string gmailUser = "josuecalvanapon.2010@gmail.com";
            string gmailPass = "pufjlovvrvkfiwfg";
            MimeMessage msg = new();
            msg.From.Add(new MailboxAddress("Prueba", gmailUser));
            msg.To.Add(new MailboxAddress("Destino", correo));
            msg.Subject = "Cita";
            BodyBuilder cuerpoMensaje = new BodyBuilder();
            cuerpoMensaje.TextBody = "Usted tiene una cita pendiente, información de la cita:\r\n Nombre del discapacitado: {cita.nombreDiscapacitado} \r\nNombre del voluntario: {cita.nombreVoluntario}\r\nFecha de encuentro: {cita.dia}\r\nHora de encuentro: {cita.horario}\r\nLugar de encuentro: {cita.lugarEncuentro}.";
            cuerpoMensaje.HtmlBody = $"<p>Usted tiene una cita pendiente, información de la cita:</p> <p>Nombre del discapacitado: {cita.nombreDiscapacitado}</p> <p>Nombre del voluntario: {cita.nombreVoluntario}</p> <p>Lugar de Encuentro: {cita.lugarEncuentro}</p> <p>Hora de encuentro: {cita.horaInicio}</p>";
            msg.Body = cuerpoMensaje.ToMessageBody();
            SmtpClient client = new();
            client.CheckCertificateRevocation = false;
            client.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(gmailUser, gmailPass);
            client.Send(msg);
            client.Disconnect(true);
        }
        public void EnviarCorreo3(BlocCita blocCita,string correo)
        {
            string servidor = "smtp.gmail.com";
            int puerto = 587;
            string gmailUser = "josuecalvanapon.2010@gmail.com";
            string gmailPass = "pufjlovvrvkfiwfg";
            MimeMessage msg = new();
            msg.From.Add(new MailboxAddress("Prueba", gmailUser));
            msg.To.Add(new MailboxAddress("Destino", correo));
            msg.Subject = "Cita";
            BodyBuilder cuerpoMensaje = new BodyBuilder();
            cuerpoMensaje.TextBody = "Lo Sentimos pero el voluntario: {blocCita.nombreVoluntario} \r\nAh cancelado la cita de encuentro que usted solicito, porfavor inicie sesión para solicitar otra cita.";
            cuerpoMensaje.HtmlBody = $"<p>Lo Sentimos pero el voluntario: {blocCita.nombreVoluntario}</p> <p>Ah cancelado la cita de encuentro que usted solicito, porfavor inicie sesión para solicitar otra cita.</p>";
            msg.Body = cuerpoMensaje.ToMessageBody();
            SmtpClient client = new();
            client.CheckCertificateRevocation = false;
            client.Connect(servidor, puerto, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate(gmailUser, gmailPass);
            client.Send(msg);
            client.Disconnect(true);
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
    }
}
