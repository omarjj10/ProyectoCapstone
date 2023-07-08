using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProyectoCapstone.Data;
using Moq;
using Moq.EntityFrameworkCore;
using ProyectoCapstone.Models;
using ProyectoCapstone.Validaciones;

namespace ProyectoCapstone.Test.Validaciones
{
    public class ValidacionesUsuariosTest
    {
        [Test]
        public void NombreUsuarioUnico()
        {
            var contextMock = new Mock<Context>(new DbContextOptions<Context>());
            contextMock.Setup(u => u.usuarios).ReturnsDbSet(new List<Usuario>
            {
                new Usuario{nombre="Omar Jackser Josue"},
                new Usuario{nombre="Jhon Ever"},
                new Usuario{nombre="Richard Ronaldo"},
                new Usuario{nombre="Daniel Enrique"},
            });
            var context = contextMock.Object;
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.NombreUnico(context, new Usuario { nombre = "Omar Jackser Josue" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void NombreUsuarioUnico2()
        {
            var contextMock = new Mock<Context>(new DbContextOptions<Context>());
            contextMock.Setup(u => u.usuarios).ReturnsDbSet(new List<Usuario>
            {
                new Usuario{nombre="Omar Jackser Josue"},
                new Usuario{nombre="Jhon Ever"},
                new Usuario{nombre="Richard Ronaldo"},
                new Usuario{nombre="Daniel Enrique"},
            });
            var context = contextMock.Object;
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.NombreUnico(context, new Usuario { nombre = "Daniel Enrique" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void NombreUsuarioUnico3()
        {
            var contextMock = new Mock<Context>(new DbContextOptions<Context>());
            contextMock.Setup(u => u.usuarios).ReturnsDbSet(new List<Usuario>
            {
                new Usuario{nombre="Omar Jackser Josue"},
                new Usuario{nombre="Jhon Ever"},
                new Usuario{nombre="Richard Ronaldo"},
                new Usuario{nombre="Daniel Enrique"},
            });
            var context = contextMock.Object;
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.NombreUnico(context, new Usuario { nombre = "Juan Marcos" });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void ApellidoUsuarioUnico()
        {
            var contextMock = new Mock<Context>(new DbContextOptions<Context>());
            contextMock.Setup(u => u.usuarios).ReturnsDbSet(new List<Usuario>
            {
                new Usuario{apellido="Calvanapon Castillo"},
                new Usuario{apellido="Carrero Silva"},
                new Usuario{apellido="Rudas Alvarez"},
                new Usuario{apellido="Horna Zegarra"},
            });
            var context = contextMock.Object;
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.ApellidoUnico(context, new Usuario { apellido = "Sanchez Vasquez" });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void ApellidoUsuarioUnico2()
        {
            var contextMock = new Mock<Context>(new DbContextOptions<Context>());
            contextMock.Setup(u => u.usuarios).ReturnsDbSet(new List<Usuario>
            {
                new Usuario{apellido="Calvanapon Castillo"},
                new Usuario{apellido="Carrero Silva"},
                new Usuario{apellido="Rudas Alvarez"},
                new Usuario{apellido="Horna Zegarra"},
            });
            var context = contextMock.Object;
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.ApellidoUnico(context, new Usuario { apellido = "Calvanapon Castillo" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void ApellidoUsuarioUnico3()
        {
            var contextMock = new Mock<Context>(new DbContextOptions<Context>());
            contextMock.Setup(u => u.usuarios).ReturnsDbSet(new List<Usuario>
            {
                new Usuario{apellido="Calvanapon Castillo"},
                new Usuario{apellido="Carrero Silva"},
                new Usuario{apellido="Rudas Alvarez"},
                new Usuario{apellido="Horna Zegarra"},
            });
            var context = contextMock.Object;
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.ApellidoUnico(context, new Usuario { apellido = "Rudas Alvarez" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void NumeroDniUnicoUsuario()
        {
            var contextMock = new Mock<Context>(new DbContextOptions<Context>());
            contextMock.Setup(u => u.usuarios).ReturnsDbSet(new List<Usuario>
            {
                new Usuario{numeroDni="73522529"},
                new Usuario{numeroDni="72135436"},
                new Usuario{numeroDni="71234534"},
                new Usuario{numeroDni="72334546"},
            });
            var context = contextMock.Object;
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.NumeroDniUnico(context, new Usuario { numeroDni = "73522529" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void NumeroDniUnicoUsuario2()
        {
            var contextMock = new Mock<Context>(new DbContextOptions<Context>());
            contextMock.Setup(u => u.usuarios).ReturnsDbSet(new List<Usuario>
            {
                new Usuario{numeroDni="73522529"},
                new Usuario{numeroDni="72135436"},
                new Usuario{numeroDni="71234534"},
                new Usuario{numeroDni="72334546"},
            });
            var context = contextMock.Object;
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.NumeroDniUnico(context, new Usuario { numeroDni = "12345678" });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void LongitudDniUsuario()
        {
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.LongitudDni(new Usuario { numeroDni="12345678"});
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void LongitudDniUsuario2()
        {
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.LongitudDni(new Usuario { numeroDni = "123456789" });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void LongitudTelefonoUsuario()
        {
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.LongitudNumeroTelefono(new Usuario { telefono = 123456789 });
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void LongitudTelefonoUsuario2()
        {
            var validator = new ValidacionesUsuarioscs();
            var resultado = validator.LongitudNumeroTelefono(new Usuario { telefono = 1234567892 });
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroUsuarioCorrectamente()
        {
            var validator = new ValidacionesUsuarioscs();
            var listaUsuarios = new List<Usuario>
            {
                new Usuario{nombre="Jhon Ever",apellido="Carrero Silva",numeroDni="12345678",telefono=123456789,inicio="12:00",fin="15:00",direccion="Av. Martires de Uchuracay #1462"},
                new Usuario{nombre="Richard Ronaldo",apellido="Rudas Alvarez",numeroDni="12345678",telefono=123456789,inicio="9:00",fin="11:00",direccion="Av. Martires de Uchuracay #1462"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.usuarios).ReturnsDbSet(listaUsuarios);
            var usuario= new Usuario
            {
                nombre="Omar Jackser Josue",apellido="Calvanapon Castillo",numeroDni="73522529",telefono=922513085,inicio="10:00",fin="12:00",direccion="Av. Martires de Uchuracay #1462"
            };
            var resultado = validator.RegistroUsuario(rcMock.Object, usuario);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void RegistroUsuarioIncorrectamenteUsuarioYaExiste()
        {
            var validator = new ValidacionesUsuarioscs();
            var listaUsuarios = new List<Usuario>
            {
                new Usuario{nombre="Jhon Ever",apellido="Carrero Silva",numeroDni="12345678",telefono=123456789,inicio="12:00",fin="15:00",direccion="Av. Martires de Uchuracay #1462"},
                new Usuario{nombre="Richard Ronaldo",apellido="Rudas Alvarez",numeroDni="12345678",telefono=123456789,inicio="9:00",fin="11:00",direccion="Av. Martires de Uchuracay #1462"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.usuarios).ReturnsDbSet(listaUsuarios);
            var usuario = new Usuario
            {
                nombre = "Jhon Ever",
                apellido = "Carrero Silva",
                numeroDni = "12345678",
                telefono = 922513085,
                inicio = "10:00",
                fin = "12:00",
                direccion = "Av. Martires de Uchuracay #1462"
            };
            var resultado = validator.RegistroUsuario(rcMock.Object, usuario);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroUsuarioIncorrectamenteHorarioDisponibleIncorrecto()
        {
            var validator = new ValidacionesUsuarioscs();
            var listaUsuarios = new List<Usuario>
            {
                new Usuario{nombre="Jhon Ever",apellido="Carrero Silva",numeroDni="12345678",telefono=123456789,inicio="12:00",fin="15:00",direccion="Av. Martires de Uchuracay #1462"},
                new Usuario{nombre="Richard Ronaldo",apellido="Rudas Alvarez",numeroDni="12345678",telefono=123456789,inicio="9:00",fin="11:00",direccion="Av. Martires de Uchuracay #1462"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.usuarios).ReturnsDbSet(listaUsuarios);
            var usuario = new Usuario
            {
                nombre = "Omar Jackser Josue",
                apellido = "Calvanapon Castillo",
                numeroDni = "73522529",
                telefono = 922513085,
                inicio = "12:00",
                fin = "10:00",
                direccion = "Av. Martires de Uchuracay #1462"
            };
            var resultado = validator.RegistroUsuario(rcMock.Object, usuario);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroUsuarioIncorrectamenteLongitudNumeroTelefono()
        {
            var validator = new ValidacionesUsuarioscs();
            var listaUsuarios = new List<Usuario>
            {
                new Usuario{nombre="Jhon Ever",apellido="Carrero Silva",numeroDni="12345678",telefono=123456789,inicio="12:00",fin="15:00",direccion="Av. Martires de Uchuracay #1462"},
                new Usuario{nombre="Richard Ronaldo",apellido="Rudas Alvarez",numeroDni="12345678",telefono=123456789,inicio="9:00",fin="11:00",direccion="Av. Martires de Uchuracay #1462"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.usuarios).ReturnsDbSet(listaUsuarios);
            var usuario = new Usuario
            {
                nombre = "Omar Jackser Josue",
                apellido = "Calvanapon Castillo",
                numeroDni = "73522529",
                telefono = 22513085,
                inicio = "10:00",
                fin = "12:00",
                direccion = "Av. Martires de Uchuracay #1462"
            };
            var resultado = validator.RegistroUsuario(rcMock.Object, usuario);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void RegistroUsuarioIncorrectamenteLongitudDni()
        {
            var validator = new ValidacionesUsuarioscs();
            var listaUsuarios = new List<Usuario>
            {
                new Usuario{nombre="Jhon Ever",apellido="Carrero Silva",numeroDni="12345678",telefono=123456789,inicio="12:00",fin="15:00",direccion="Av. Martires de Uchuracay #1462"},
                new Usuario{nombre="Richard Ronaldo",apellido="Rudas Alvarez",numeroDni="12345678",telefono=123456789,inicio="9:00",fin="11:00",direccion="Av. Martires de Uchuracay #1462"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.usuarios).ReturnsDbSet(listaUsuarios);
            var usuario = new Usuario
            {
                nombre = "Omar Jackser Josue",
                apellido = "Calvanapon Castillo",
                numeroDni = "735225",
                telefono = 922513085,
                inicio = "10:00",
                fin = "12:00",
                direccion = "Av. Martires de Uchuracay #1462"
            };
            var resultado = validator.RegistroUsuario(rcMock.Object, usuario);
            Assert.AreEqual(false, resultado);
        }
    }
}
