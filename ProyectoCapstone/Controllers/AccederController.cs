using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProyectoCapstone.Data;
using ProyectoCapstone.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Web;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ProyectoCapstone.Controllers
{
    public class AccederController : Controller
    {
        private readonly Context _db;
        public AccederController(Context db)
        {
            _db = db;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Usuario usuario)
        {
            var user = validarCredenciales(usuario.correo, usuario.clave);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.nombre+" "+user.apellido),
                    new Claim(ClaimTypes.NameIdentifier,user.UsuarioID.ToString()),
                    new Claim(ClaimTypes.Email,user.correo),
                    new Claim(ClaimTypes.Role,user.rol.descripcion)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                                new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Inicio", "Home");
            }
            ViewData["Mensaje"] = "El correo y/o la contraseña son incorrectas";
            return View();
        }
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceder");
        }
        [HttpGet]
        public IActionResult RegistrarVoluntario(string? error)
        {
            if(error != null)
            {
                ViewBag.mensajeCorreo = "Su codigo de verificación es erronea, porfavor ingrese nuevamente todos sus datos.";
            }
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarVoluntario(BlocUsuario usuario)
        {
            usuario.estado = "disponible";
            Random aleatorio = new Random();
            int codigo = aleatorio.Next(100000, 999999);
            usuario.codigoVerificarCorreo = codigo;
            if (ModelState.IsValid)
            {
                dynamic respuesta = Get("https://dniruc.apisperu.com/api/v1/dni/" + usuario.numeroDni + "?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6Impvc3VlY2FsdmFuYXBvbi4yMDEwQGdtYWlsLmNvbSJ9.9KgW1UKeWtrVHohnLLKDmmTUp4nOVrcAXCat1mA2N6w");
                if (respuesta.success == true)
                {
                    string nombreGet = respuesta.apellidoPaterno.ToString() + " " + respuesta.apellidoMaterno.ToString() + " " + respuesta.nombres.ToString();
                    string nombreUs = usuario.apellido + " " + usuario.nombre;
                    string nomMayuscula = nombreUs.ToUpper();
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
                    int numero = usuario.telefono;
                    int contador = 0;
                    while (numero >= 1)
                    {
                        contador = contador + 1;
                        numero = numero / 10;
                    }
                    bool usuarioExiste = validarSiExisteUsuario(usuario.nombre, usuario.apellido, usuario.numeroDni);
                    if (usuarioExiste == true)
                    {
                        if (nomMayuscula.Equals(nombreGet))
                        {
                            if (inicio < fin)
                            {
                                if (contador == 9)
                                {
                                    EnviarCorreo(codigo, usuario.correo);
                                    _db.blocUsuarios.Add(usuario);
                                    _db.SaveChanges();
                                    return RedirectToAction("ConfirmarCorreoVoluntario",new { id = usuario.BlocUsuarioID });
                                }
                                else
                                {
                                    ViewData["Telefono"] = "El numero de telefono debe contener 9 digitos";
                                }
                            }
                            else
                            {
                                ViewData["Hora"] = "La hora de inicio debe ser menor a la hora de fin";
                            }
                        }
                        else
                        {
                            ViewData["Nombre"] = "Error! Su nombre y/o apellido no coincide con su número de Dni";
                        }
                    }
                    else
                    {
                        ViewData["Existe"] = "Error el usuario ya esta registrado";
                    }
                }
                else
                {
                    ViewData["DniReniec"] = "Número de DNI no se encuentra en el padrón de RENIEC";
                }
            }
            ViewData["Mensaje"] = "Error modelo es incorrecto";
            return View();
        }
        [HttpGet]
        public IActionResult RegistrarDiscapacitado(string?error)
        {
            if (error != null)
            {
                ViewBag.mensajeCorreo = "Su codigo de verificación es erronea, porfavor ingrese nuevamente todos sus datos.";
            }
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarDiscapacitado(BlocUsuario usuario)
        {
            Random aleatorio = new Random();
            int codigo = aleatorio.Next(100000, 999999);
            usuario.codigoVerificarCorreo = codigo;
            if (ModelState.IsValid)
            {
                dynamic respuesta = Get("https://dniruc.apisperu.com/api/v1/dni/" + usuario.numeroDni + "?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJlbWFpbCI6Impvc3VlY2FsdmFuYXBvbi4yMDEwQGdtYWlsLmNvbSJ9.9KgW1UKeWtrVHohnLLKDmmTUp4nOVrcAXCat1mA2N6w");
                if (respuesta.success == true)
                {
                    string nombreGet = respuesta.apellidoPaterno.ToString() + " " + respuesta.apellidoMaterno.ToString() + " " + respuesta.nombres.ToString();
                    string nombreUs = usuario.apellido + " " + usuario.nombre;
                    string nomMayuscula = nombreUs.ToUpper();
                    int numero = usuario.telefono;
                    int contador = 0;
                    while (numero >= 1)
                    {
                        contador = contador + 1;
                        numero = numero / 10;
                    }
                    bool usuarioExiste = validarSiExisteUsuario(usuario.nombre, usuario.apellido, usuario.numeroDni);
                    if (usuarioExiste == true)
                    {
                        if (nomMayuscula.Equals(nombreGet))
                        {
                            if (contador == 9)
                            {
                                EnviarCorreo(codigo, usuario.correo);
                                _db.blocUsuarios.Add(usuario);
                                _db.SaveChanges();
                                return RedirectToAction("ConfirmarCorreoDiscapacitado", new {id=usuario.BlocUsuarioID});
                            }
                            else
                            {
                                ViewData["Telefono"] = "El numero de telefono debe contener 9 digitos";
                            }
                        }
                        else
                        {
                            ViewData["Nombre"] = "Error! Su nombre y/o apellido no coincide con su número de Dni";
                        }
                    }
                    else
                    {
                        ViewData["Existe"] = "Error el usuario ya esta registrado";
                    }
                }
                else
                {
                    ViewData["DniReniec"] = "Número de DNI no se encuentra en el padrón de RENIEC";
                }
            }
            ViewData["Mensaje"] = "Erro el modelo es incorrecto";
            return View();
        }
        [HttpGet]
        public IActionResult ConfirmarCorreoDiscapacitado(int? id,string?mensaje2)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var usuario = _db.blocUsuarios.Find(id);
            if(usuario == null)
            {
                return NotFound();
            }
            if (mensaje2 != null)
            {
                ViewBag.error2 = "Error! Porfavor completar los campos requeridos.";
            }
            ViewBag.nombre = usuario.nombre;
            ViewBag.apellido = usuario.apellido;
            ViewBag.correo = usuario.correo;
            ViewBag.clave = usuario.clave;
            ViewBag.discapacidad = usuario.discapacidad;
            ViewBag.dni = usuario.numeroDni;
            ViewBag.direccion = usuario.direccion;
            ViewBag.telefono = usuario.telefono;
            ViewBag.codigoVer = usuario.codigoVerificarCorreo;
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmarCorreoDiscapacitado(Usuario usuario,string? codigo,string? codigoVerificacion)
        {
            if (codigoVerificacion != null)
            {
                usuario.RolID = 3;
                if (ModelState.IsValid)
                {
                    if (codigoVerificacion.Equals(codigo))
                    {
                        EliminarDiscapacitado(usuario);
                        _db.usuarios.Add(usuario);
                        _db.SaveChanges();
                        return RedirectToAction("Login", "Acceder");
                    }
                    else
                    {
                        var us = _db.blocUsuarios.Where(u => u.nombre == usuario.nombre).FirstOrDefault();
                        _db.blocUsuarios.Remove(us);
                        _db.SaveChanges();
                        return RedirectToAction("RegistrarDiscapacitado", new { error = "error" });
                    }
                }
            }
            else
            {
                var user = _db.blocUsuarios.Where(u => u.nombre == usuario.nombre).FirstOrDefault();
                return RedirectToAction("ConfirmarCorreoDiscapacitado", "Acceder", new { id = user.BlocUsuarioID, mensaje2 = "error2" });
            }
            return View(usuario);
        }
        public void EliminarDiscapacitado(Usuario usuario)
        {
            var blocUsuario = _db.blocUsuarios.Where(u => u.nombre == usuario.nombre).FirstOrDefault();
            _db.blocUsuarios.Remove(blocUsuario);
            _db.SaveChanges();
        }
        [HttpGet]
        public IActionResult ConfirmarCorreoVoluntario(int? id,string? mensaje2)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            var usuario = _db.blocUsuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            if (mensaje2 != null)
            {
                ViewBag.error2 = "Error! Porfavor completar los campos requeridos.";
            }
            ViewBag.nombre = usuario.nombre;
            ViewBag.apellido = usuario.apellido;
            ViewBag.correo = usuario.correo;
            ViewBag.clave = usuario.clave;
            ViewBag.discapacidad = usuario.discapacidad;
            ViewBag.dni = usuario.numeroDni;
            ViewBag.direccion = usuario.direccion;
            ViewBag.telefono = usuario.telefono;
            ViewBag.diaDisponible = usuario.diaDisponible;
            ViewBag.inicio = usuario.inicio;
            ViewBag.fin = usuario.fin;
            ViewBag.estado = usuario.estado;
            ViewBag.codigoVer = usuario.codigoVerificarCorreo;
            return View();
        }
        [HttpPost]
        public IActionResult ConfirmarCorreoVoluntario(Usuario usuario, string? codigo, string? codigoVerificacion)
        {
            if (codigoVerificacion != null)
            {
                usuario.RolID = 2;
                usuario.recomendacion = 0;
                if (ModelState.IsValid)
                {
                    if (codigoVerificacion.Equals(codigo))
                    {
                        EliminarVoluntario(usuario);
                        _db.usuarios.Add(usuario);
                        _db.SaveChanges();
                        return RedirectToAction("Login", "Acceder");
                    }
                    else
                    {
                        var us = _db.blocUsuarios.Where(u => u.nombre == usuario.nombre).FirstOrDefault();
                        _db.blocUsuarios.Remove(us);
                        _db.SaveChanges();
                        return RedirectToAction("RegistrarVoluntario", new {error = "error" });
                    }
                }
            }
            else
            {
                var user = _db.blocUsuarios.Where(u => u.nombre == usuario.nombre).FirstOrDefault();
                return RedirectToAction("ConfirmarCorreoVoluntario", "Acceder", new { id = user.BlocUsuarioID, mensaje2 = "error2" });
            }
            return View(usuario);
        }
        public void EliminarVoluntario(Usuario usuario)
        {
            var blocUsuario = _db.blocUsuarios.Where(u => u.nombre == usuario.nombre).FirstOrDefault();
            _db.blocUsuarios.Remove(blocUsuario);
            _db.SaveChanges();
        }
        public void EnviarCorreo(int codigo,string correo)
        {
            string servidor = "smtp.gmail.com";
            int puerto = 587;
            string gmailUser = "josuecalvanapon.2010@gmail.com";
            string gmailPass = "pufjlovvrvkfiwfg";
            MimeMessage msg = new();
            msg.From.Add(new MailboxAddress("Prueba", gmailUser));
            msg.To.Add(new MailboxAddress("Destino", correo));
            msg.Subject = "VerificarCorreo";
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
        public bool validarSiExisteUsuario(string nombre,string apellido,string dni)
        {
            var usuario = _db.usuarios.Where(u => u.nombre == nombre).Where(u => u.apellido == apellido).Where(u => u.numeroDni == dni).FirstOrDefault();
            if (usuario != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private Usuario validarCredenciales(string correo, string password)
        {
            var user = _db.usuarios.Include(r => r.rol)
                                .Where(u => u.correo == correo)
                                .Where(u => u.clave == password)
                                .FirstOrDefault();
            return user;
        }
        public dynamic Get(string url)
        {
            HttpWebRequest myWebRequest = (HttpWebRequest)WebRequest.Create(url);
            myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";
            //myWebRequest.CookieContainer = myCookie;
            myWebRequest.Credentials = CredentialCache.DefaultCredentials;
            myWebRequest.Proxy = null;
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
            Stream myStream = myHttpWebResponse.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myStream);
            //Leemos los datos
            string Datos = HttpUtility.HtmlDecode(myStreamReader.ReadToEnd());

            dynamic data = JsonConvert.DeserializeObject(Datos);

            return data;
        }
    }
}
