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
    public class ValidacionesCitasTest
    {
        [Test]
        public void VerificarSiLaFechaDeSolicitudDeCitaEsValida()
        {
            var validator = new ValidacionesCitas();
            var citas = new List<Cita>
            {
                new Cita{nombreVoluntario="Jhon Carrero",fecha="25/05/2023"},
                new Cita{nombreVoluntario="Jhon Carrero",fecha="26/05/2023"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.cita).ReturnsDbSet(citas);
            var blocCita = new BlocCita
            {
                nombreVoluntario = "Jhon Carrero",
                dia = "28/05/2023"
            };
            var resultado = validator.VerificarSiElVoluntarioNoTieneUnaCitaPendiente(rcMock.Object, blocCita);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void VerificarSiLaFechaDeSolicitudDeCitaEsValida2()
        {
            var validator = new ValidacionesCitas();
            var citas = new List<Cita>
            {
                new Cita{nombreVoluntario="Jhon Carrero",fecha="25/05/2023"},
                new Cita{nombreVoluntario="Jhon Carrero",fecha="26/05/2023"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.cita).ReturnsDbSet(citas);
            var blocCita = new BlocCita
            {
                nombreVoluntario = "Jhon Carrero",
                dia = "25/05/2023"
            };
            var resultado = validator.VerificarSiElVoluntarioNoTieneUnaCitaPendiente(rcMock.Object, blocCita);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void VerificarSiLaHoraEsValidaParaSolicitarCita()
        {
            var validator = new ValidacionesCitas();
            var voluntario = new List<Usuario>
            {
                new Usuario{nombre="Omar Jackser Josue",inicio="10:00",fin="12:00"},
                new Usuario{nombre="Carlos Vasquez",inicio="13:00",fin="15:00"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.usuarios).ReturnsDbSet(voluntario);
            var blocCita = new BlocCita
            {
                nombreVoluntario = "Omar Jackser Josue",
                horario="11:00"
            };
            var resultado = validator.VerificarHoraDeSolicitudDeCitaEsValida(rcMock.Object, blocCita);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void VerificarSiLaHoraEsValidaParaSolicitarCita2()
        {
            var validator = new ValidacionesCitas();
            var voluntario = new List<Usuario>
            {
                new Usuario{nombre="Omar Jackser Josue",inicio="10:00",fin="12:00"},
                new Usuario{nombre="Carlos Vasquez",inicio="13:00",fin="15:00"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.usuarios).ReturnsDbSet(voluntario);
            var blocCita = new BlocCita
            {
                nombreVoluntario = "Omar Jackser Josue",
                horario = "13:00"
            };
            var resultado = validator.VerificarHoraDeSolicitudDeCitaEsValida(rcMock.Object, blocCita);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void VerificarSilaHoraDeLaCitaEsValida()
        {
            var validator = new ValidacionesCitas();
            var citas = new List<Cita>
            {
                new Cita{horaInicio="1100",tiempoCita="120"},
                new Cita{horaInicio="1300",tiempoCita="140"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.cita).ReturnsDbSet(citas);
            var nuevaCita = new Cita
            {
                horaInicio="1500",
                tiempoCita="110"
            };
            var resultado = validator.VerificarHoraFinSeaMayorHoraInicio(rcMock.Object, nuevaCita);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void VerificarSilaHoraDeLaCitaEsValida2()
        {
            var validator = new ValidacionesCitas();
            var citas = new List<Cita>
            {
                new Cita{horaInicio="1100",tiempoCita="120"},
                new Cita{horaInicio="1300",tiempoCita="140"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(o => o.cita).ReturnsDbSet(citas);
            var nuevaCita = new Cita
            {
                horaInicio = "1000",
                tiempoCita = "110"
            };
            var resultado = validator.VerificarHoraFinSeaMayorHoraInicio(rcMock.Object, nuevaCita);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void VerificarSilaHoraDeSolicitudCitaEsValida()
        {
            var validator = new ValidacionesCitas();
            var listaVoluntario = new List<Usuario>
            {
                new Usuario{nombre="Omar Calvanapon",diaDisponible="Domingo",inicio="10:00",fin="12:00"},
                new Usuario{nombre="Daniel Enrique",diaDisponible="Domingo",inicio="12:00",fin="14:00"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(u=>u.usuarios).ReturnsDbSet(listaVoluntario);
            var nuevaBlocCita = new BlocCita
            {
                nombreDiscapacitado="Franco Saravia",
                nombreVoluntario = "Omar Calvanapon",
                horario="11:00",
                tiempoCita="60 minutos",
                dia="Domingo 1/06/2023"
            };
            var resultado = validator.VerificarSiLaHoraDeSolicitudCitaEsValida(rcMock.Object, nuevaBlocCita);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void VerificarSilaHoraDeSolicitudCitaEsValida2()
        {
            var validator = new ValidacionesCitas();
            var listaVoluntario = new List<Usuario>
            {
                new Usuario{nombre="Omar Calvanapon",diaDisponible="Domingo",inicio="10:00",fin="12:00"},
                new Usuario{nombre="Daniel Enrique",diaDisponible="Domingo",inicio="12:00",fin="14:00"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(u => u.usuarios).ReturnsDbSet(listaVoluntario);
            var nuevaSolicitud = new BlocCita
            {
                nombreDiscapacitado = "Franco Saravia",
                nombreVoluntario = "Daniel Enrique",
                horario = "13:00",
                tiempoCita = "60 minutos",
                dia = "Domingo 1/06/2023"
            };
            var resultado = validator.VerificarSiLaHoraDeSolicitudCitaEsValida(rcMock.Object, nuevaSolicitud);
            Assert.AreEqual(true, resultado);
        }
        [Test]
        public void VerificarSilaHoraDeSolicitudCitaNoEsValida()
        {
            var validator = new ValidacionesCitas();
            var listaVoluntario = new List<Usuario>
            {
                new Usuario{nombre="Omar Calvanapon",diaDisponible="Domingo",inicio="10:00",fin="12:00"},
                new Usuario{nombre="Daniel Enrique",diaDisponible="Domingo",inicio="12:00",fin="14:00"},
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(u => u.usuarios).ReturnsDbSet(listaVoluntario);
            var nuevaSolicitud = new BlocCita
            {
                nombreDiscapacitado = "Franco Saravia",
                nombreVoluntario = "Daniel Enrique",
                horario = "15:00",
                tiempoCita = "60 minutos",
                dia = "Domingo 1/06/2023"
            };
            var resultado = validator.VerificarSiLaHoraDeSolicitudCitaEsValida(rcMock.Object, nuevaSolicitud);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void VerificarSiDiscapacitadoSolicitoCitaAlMismoVoluntario()
        {
            var validator = new ValidacionesCitas();
            var listaSolicitarCita = new List<BlocCita>
            {
                new BlocCita{nombreDiscapacitado="Guadalupe Castillo",nombreVoluntario="Omar Calvanapon"}
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(u => u.blocCitas).ReturnsDbSet(listaSolicitarCita);
            var nuevaSolicitud = new BlocCita
            {
                nombreDiscapacitado = "Guadalupe Castillo",
                nombreVoluntario = "Omar Calvanapon",
            };
            var resultado = validator.VerificarSiElDiscapacitadoSolicitoDosCitasAlMismoVoluntario(rcMock.Object, nuevaSolicitud);
            Assert.AreEqual(false, resultado);
        }
        [Test]
        public void VerificarSiDiscapacitadoSolicitoCitaAlMismoVoluntario2()
        {
            var validator = new ValidacionesCitas();
            var listaSolicitarCita = new List<BlocCita>
            {
                new BlocCita{nombreDiscapacitado="Guadalupe Castillo",nombreVoluntario="Omar Calvanapon"}
            };
            var rcMock = new Mock<Context>(new DbContextOptions<Context>());
            rcMock.Setup(u => u.blocCitas).ReturnsDbSet(listaSolicitarCita);
            var nuevaSolicitud = new BlocCita
            {
                nombreDiscapacitado = "Guadalupe Castillo",
                nombreVoluntario = "Juan Carlos",
            };
            var resultado = validator.VerificarSiElDiscapacitadoSolicitoDosCitasAlMismoVoluntario(rcMock.Object, nuevaSolicitud);
            Assert.AreEqual(true, resultado);
        }
    }
}
