using ProyectoCapstone.Data;
using ProyectoCapstone.Models;
namespace ProyectoCapstone.Validaciones
{
    public class ValidacionesCitas
    {
        public bool VerificarSiElVoluntarioNoTieneUnaCitaPendiente(Context contexto, BlocCita solicitarCita)
        {
            string fechaActualSistema = "";
            string fechaCita = "";
            int fechCita;
            for (int i = 0; i < solicitarCita.dia.Length; i++)
            {
                if (solicitarCita.dia[i] == 48 || solicitarCita.dia[i] == 49 || solicitarCita.dia[i] == 50 || solicitarCita.dia[i] == 51 || solicitarCita.dia[i] == 52 || solicitarCita.dia[i] == 53 || solicitarCita.dia[i] == 54 || solicitarCita.dia[i] == 55 || solicitarCita.dia[i] == 56 || solicitarCita.dia[i] == 57)
                {
                    fechaActualSistema = fechaActualSistema + solicitarCita.dia[i];
                }
            }
            int fechaActualLocal = Int32.Parse(fechaActualSistema);
            var cita = contexto.cita.Where(c => c.nombreVoluntario.Equals(solicitarCita.nombreVoluntario)).FirstOrDefault();
            for (int i = 0; i < cita.fecha.Length; i++)
            {
                if (cita.fecha[i] == 48 || cita.fecha[i] == 49 || cita.fecha[i] == 50 || cita.fecha[i] == 51 || cita.fecha[i] == 52 || cita.fecha[i] == 53 || cita.fecha[i] == 54 || cita.fecha[i] == 55 || cita.fecha[i] == 56 || cita.fecha[i] == 57)
                {
                    fechaCita = fechaCita + cita.fecha[i];
                }
            }
            fechCita = Int32.Parse(fechaCita);
            if (fechaActualLocal > fechCita)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool VerificarHoraDeSolicitudDeCitaEsValida(Context contexto,BlocCita solicitarCita)
        {
            var voluntario = contexto.usuarios.Where(u => u.nombre == solicitarCita.nombreVoluntario).FirstOrDefault();
            string horaSolicitarCita = "";
            string horaInicioVoluntario = "";
            string horaFinVoluntario = "";
            for (int i = 0; i < solicitarCita.horario.Length; i++)
            {
                if (solicitarCita.horario[i] == 48 || solicitarCita.horario[i] == 49 || solicitarCita.horario[i] == 50 || solicitarCita.horario[i] == 51 || solicitarCita.horario[i] == 52 || solicitarCita.horario[i] == 53 || solicitarCita.horario[i] == 54 || solicitarCita.horario[i] == 55 || solicitarCita.horario[i] == 56 || solicitarCita.horario[i] == 57)
                {
                    horaSolicitarCita = horaSolicitarCita + solicitarCita.horario[i];
                }
            }
            for (int i = 0; i < voluntario.inicio.Length; i++)
            {
                if (voluntario.inicio[i] == 48 || voluntario.inicio[i] == 49 || voluntario.inicio[i] == 50 || voluntario.inicio[i] == 51 || voluntario.inicio[i] == 52 || voluntario.inicio[i] == 53 || voluntario.inicio[i] == 54 || voluntario.inicio[i] == 55 || voluntario.inicio[i] == 56 || voluntario.inicio[i] == 57)
                {
                    horaInicioVoluntario = horaInicioVoluntario + voluntario.inicio[i];
                }
            }
            for (int i = 0; i < voluntario.fin.Length; i++)
            {
                if (voluntario.fin[i] == 48 || voluntario.fin[i] == 49 || voluntario.fin[i] == 50 || voluntario.fin[i] == 51 || voluntario.fin[i] == 52 || voluntario.fin[i] == 53 || voluntario.fin[i] == 54 || voluntario.fin[i] == 55 || voluntario.fin[i] == 56 || voluntario.fin[i] == 57)
                {
                    horaFinVoluntario = horaFinVoluntario + voluntario.fin[i];
                }
            }
            int horaCita = Int32.Parse(horaSolicitarCita);
            int horaInicio = Int32.Parse(horaInicioVoluntario);
            int horaFin = Int32.Parse(horaFinVoluntario);
            if(horaCita>=horaInicio && horaCita < horaFin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool VerificarHoraFinSeaMayorHoraInicio(Context contexto, Cita nuevaCita)
        {
            int horaInicioNueva = Int32.Parse(nuevaCita.horaInicio);
            int tiempoCitaNueva = Int32.Parse(nuevaCita.tiempoCita);
            int horaFinNueva = horaInicioNueva+tiempoCitaNueva;
            return contexto.cita
            .Count(c =>horaFinNueva > (Int32.Parse(c.horaInicio)+Int32.Parse(c.tiempoCita))) == 0;
        }
        public bool VerificarSiLaHoraDeSolicitudCitaEsValida(Context contexto,BlocCita solicitarCita)
        {
            var voluntario = contexto.usuarios.Where(u => u.nombre == solicitarCita.nombreVoluntario).FirstOrDefault();
            string horaInicio = "";
            string horaFin = "";
            string horarioCita = "";
            for (int i = 0; i < solicitarCita.horario.Length; i++)
            {
                if (solicitarCita.horario[i] == 48 || solicitarCita.horario[i] == 49 || solicitarCita.horario[i] == 50 || solicitarCita.horario[i] == 51 || solicitarCita.horario[i] == 52 || solicitarCita.horario[i] == 53 || solicitarCita.horario[i] == 54 || solicitarCita.horario[i] == 55 || solicitarCita.horario[i] == 56 || solicitarCita.horario[i] == 57)
                {
                    horarioCita = horarioCita + solicitarCita.horario[i];
                }
            }
            int horario = Int32.Parse(horarioCita);
            for (int i = 0; i < voluntario.inicio.Length; i++)
            {
                if (voluntario.inicio[i] == 48 || voluntario.inicio[i] == 49 || voluntario.inicio[i] == 50 || voluntario.inicio[i] == 51 || voluntario.inicio[i] == 52 || voluntario.inicio[i] == 53 || voluntario.inicio[i] == 54 || voluntario.inicio[i] == 55 || voluntario.inicio[i] == 56 || voluntario.inicio[i] == 57)
                {
                    horaInicio = horaInicio + voluntario.inicio[i];
                }
            }
            int inicio = Int32.Parse(horaInicio);
            for (int i = 0; i < voluntario.fin.Length; i++)
            {
                if (voluntario.fin[i] == 48 || voluntario.fin[i] == 49 || voluntario.fin[i] == 50 || voluntario.fin[i] == 51 || voluntario.fin[i] == 52 || voluntario.fin[i] == 53 || voluntario.fin[i] == 54 || voluntario.fin[i] == 55 || voluntario.fin[i] == 56 || voluntario.fin[i] == 57)
                {
                    horaFin = horaFin + voluntario.fin[i];
                }
            }
            int fin = Int32.Parse(horaFin);
            if (horario > inicio && horario < fin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool VerificarSiElDiscapacitadoSolicitoDosCitasAlMismoVoluntario(Context contexto, BlocCita solicitarCita)
        {
            return contexto.blocCitas.Count(c => c.nombreVoluntario == solicitarCita.nombreVoluntario && c.nombreDiscapacitado==solicitarCita.nombreDiscapacitado) == 0;
        }
    }
}
