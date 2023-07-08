using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;
using ProyectoCapstone.Data;
using ProyectoCapstone.Models;
using System.Security.Claims;

namespace ProyectoCapstone.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly Context _db;
        public UsuarioController(Context db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre+" "+user.apellido;
            ViewBag.id = user.UsuarioID;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> obtenerTodos()
        {
            var todos = await _db.usuarios.Where(u=>u.RolID==2||u.RolID==3).ToListAsync();
            return Json(new { data = todos });
        }
        [HttpDelete]
        public async Task<IActionResult> Eliminar(int? id)
        {
            var usuario = await _db.usuarios.FindAsync(id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error al borrar" });
            }
            _db.usuarios.Remove(usuario);
            await _db.SaveChangesAsync();
            return Json(new { success = true, message = "Usuario borrado exitosamente" });
        }
        [HttpGet]
        public IActionResult Editar(int? Id,string? error)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre+ " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var usuario = _db.usuarios.Find(Id);
            if (usuario == null)
            {
                return NotFound();
            }
            if (error != null)
            {
                ViewBag.error="La hora de inicio debe ser menor a la hora de fin";
            }
            return View(usuario);
        }
        [HttpPost]
        public IActionResult Editar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                string voluntarioInicio = "";
                string voluntarioFin = "";
                for (int i = 0; i < usuario.inicio.Length; i++)
                {
                    if (usuario.inicio[i] == 48 || usuario.inicio[i] == 49 || usuario.inicio[i] == 50 || usuario.inicio[i] == 51 || usuario.inicio[i] == 52 || usuario.inicio[i] == 53 || usuario.inicio[i] == 54 || usuario.inicio[i] == 55 || usuario.inicio[i] == 56 || usuario.inicio[i] == 57)
                    {
                        voluntarioInicio = voluntarioInicio + usuario.inicio[i];
                    }
                }
                for (int i = 0; i < usuario.fin.Length; i++)
                {
                    if (usuario.fin[i] == 48 || usuario.fin[i] == 49 || usuario.fin[i] == 50 || usuario.fin[i] == 51 || usuario.fin[i] == 52 || usuario.fin[i] == 53 || usuario.fin[i] == 54 || usuario.fin[i] == 55 || usuario.fin[i] == 56 || usuario.fin[i] == 57)
                    {
                        voluntarioFin = voluntarioFin + usuario.fin[i];
                    }
                }
                int inicio = Int32.Parse(voluntarioInicio);
                int fin = Int32.Parse(voluntarioFin);
                if (inicio < fin)
                {
                    _db.usuarios.Update(usuario);
                    _db.SaveChanges();
                    return RedirectToAction("Inicio", "Home");
                }
                else
                {
                    return RedirectToAction("Editar", new { Id = usuario.UsuarioID, error = "error" });
                }
            }
            return View(usuario);
        }
        [HttpGet]
        public IActionResult EditarCorreo(int? Id,string?mensaje,string?mensaje2)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre+ " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var usuario = _db.usuarios.Find(Id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewBag.correo = usuario.correo;
            if (mensaje != null)
            {
                ViewBag.error = "Error! Porfavor completar los campos requeridos";
            }
            if (mensaje2 != null)
            {
                ViewBag.correoNoIgaul = "Error! Debe de coincidir los correos";
            }
            return View(usuario);
        }
        [HttpPost]
        public IActionResult EditarCorreo(Usuario usuario,string correoUsuario,string correoUsuario2)
        {
            if (correoUsuario !=null || correoUsuario2!=null)
            {
                if (correoUsuario == correoUsuario2)
                {
                    Random aleatorio = new Random();
                    int codigo = aleatorio.Next(100000, 999999);
                    usuario.codigoVerificarCorreo = codigo;
                    usuario.correo = correoUsuario;
                    if (ModelState.IsValid)
                    {
                        EnviarCorreo(codigo, usuario.correo);
                        _db.usuarios.Update(usuario);
                        _db.SaveChanges();
                        return RedirectToAction("ConfirmarCorreo", "Usuario", new {Id=usuario.UsuarioID});
                    }
                }
                else
                {
                    return RedirectToAction("EditarCorreo", "Usuario", new { Id = usuario.UsuarioID, mensaje2 = "error2" });
                }
            }
            else
            {
                return RedirectToAction("EditarCorreo", "Usuario", new { Id = usuario.UsuarioID, mensaje = "error" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult ConfirmarCorreo(int? Id,string? mensaje,string? mensaje2)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var usuario = _db.usuarios.Find(Id);
            if (usuario == null)
            {
                return NotFound();
            }
            ViewBag.codigoVer = usuario.codigoVerificarCorreo;
            if (mensaje != null)
            {
                ViewBag.error = "Error! El codigo que digito es incorrecto, porfavor ingrese nuevamente el codigo.";
            }
            if (mensaje2 != null)
            {
                ViewBag.error2 = "Error! Porfavor completar los campos requeridos.";
            }
            return View(usuario);
        }
        [HttpPost]
        public IActionResult ConfirmarCorreo(Usuario usuario, string? codigo, string? codigoVerificacion)
        {
            if (codigoVerificacion != null)
            {
                if (codigoVerificacion.Equals(codigo))
                {
                    if (ModelState.IsValid)
                    {
                        _db.usuarios.Update(usuario);
                        _db.SaveChanges();
                        return RedirectToAction("Login", "Acceder");
                    }
                }
                else
                {
                    return RedirectToAction("ConfirmarCorreo","Usuario", new { Id = usuario.UsuarioID, mensaje = "error" });
                } 
            }
            else
            {
                return RedirectToAction("ConfirmarCorreo", "Usuario", new { Id = usuario.UsuarioID, mensaje2 = "error2" });
            }
            return View(usuario);
        }
        [HttpGet]
        public IActionResult EditarClave(int? Id, string? mensaje, string? mensaje2,string? mensaje3)
        {
            var user = obtenerUsuario();
            ViewBag.nombre = user.nombre + " " + user.apellido;
            ViewBag.id = user.UsuarioID;
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            var usuario = _db.usuarios.Find(Id);
            if (usuario == null)
            {
                return NotFound();
            }
            if (mensaje != null)
            {
                ViewBag.error = "Error! Porfavor completar los campos requeridos";
            }
            if (mensaje2 != null)
            {
                ViewBag.claveNoIgaul = "Error! Debe de coincidir las contraseñas";
            }
            if (mensaje3 != null)
            {
                ViewBag.claveIncorrecta = "Error! Contraseña Incorrecta.";
            }
            ViewBag.clave = usuario.clave;
            return View(usuario);
        }
        [HttpPost]
        public IActionResult EditarClave(Usuario usuario,string claveActual,string claveUsuario,string claveUsuario2,string claveUser)
        {
            if (claveUsuario != null || claveUsuario2 != null||claveActual!=null)
            {
                if (claveActual == claveUser)
                {
                    if (claveUsuario == claveUsuario2)
                    {
                        usuario.clave = claveUsuario;
                        if (ModelState.IsValid)
                        {
                            _db.usuarios.Update(usuario);
                            _db.SaveChanges();
                            return RedirectToAction("Login", "Acceder");
                        }
                    }
                    else
                    {
                        return RedirectToAction("EditarClave", "Usuario", new { Id = usuario.UsuarioID, mensaje2 = "error2" });
                    }
                }
                else
                {
                    return RedirectToAction("EditarClave", "Usuario", new { Id = usuario.UsuarioID, mensaje3 = "error3" });
                }
            }
            else
            {
                return RedirectToAction("EditarClave", "Usuario", new { Id = usuario.UsuarioID, mensaje = "error" });
            }
            return View();
        }
        public void EnviarCorreo(int codigo, string correo)
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
            //Usted tiene una solicutd de cita:\r\n Nombre del discapacitado: { solicitarCita.nombreDiscapacitado} \r\nNombre del voluntario: { solicitarCita.nombreVoluntario}\r\nFecha de encuentro: { solicitarCita.dia}\r\nHora de encuentro: { solicitarCita.horario}\r\nLugar de encuentro: { solicitarCita.lugarEncuentro}\r\nPorfavor Inicie sesión para confirmar la solicitud de cita.
            cuerpoMensaje.TextBody = "Su codigo de verificacion es:\r\n {codigo} \r\nPorfavor ingrese este codigo para confirmar que su correo es valido";
            cuerpoMensaje.HtmlBody = $"<p>Su codigo de verificacion es:</p> <p>{codigo}</p> <p>Porfavor ingrese este codigo para confirmar que su correo es valido</p>";
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
