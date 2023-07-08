using Newtonsoft.Json;
using ProyectoCapstone.Data;
using ProyectoCapstone.Models;
using System.Net;

using System.Web;

namespace ProyectoCapstone.Validaciones
{
    public class ValidacionesUsuarioscs
    {
        public bool NombreUnico(Context contexto, Usuario usuario)
        {
            return contexto.usuarios.Count(u => u.nombre == usuario.nombre) == 0;
        }
        public bool ApellidoUnico(Context contexto, Usuario usuario)
        {
            return contexto.usuarios.Count(u => u.apellido == usuario.apellido) == 0;
        }
        public bool NumeroDniUnico(Context contexto, Usuario usuario)
        {
            return contexto.usuarios.Count(u => u.numeroDni == usuario.numeroDni) == 0;
        }
        public bool LongitudDni(Usuario usuario)
        {
            if (usuario.numeroDni.Length == 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool LongitudNumeroTelefono(Usuario usuario)
        {
            int numero = usuario.telefono;
            int contador = 0;
            while (numero >= 1)
            {
                contador = contador + 1;
                numero = numero / 10;
            }
            if (contador == 9)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool RegistroUsuario(Context contexto, Usuario usuario)
        {
            bool usuarioExiste = validarSiExisteUsuario(contexto, usuario.nombre, usuario.apellido, usuario.numeroDni);
            string horaInicio = "";
            string horaFin = "";
            for (int i = 0; i < usuario.inicio.Length; i++)
            {
                if (usuario.inicio[i] == 48 || usuario.inicio[i] == 49 || usuario.inicio[i] == 50 || usuario.inicio[i] == 51 || usuario.inicio[i] == 52 || usuario.inicio[i] == 53 || usuario.inicio[i] == 54 || usuario.inicio[i] == 55 || usuario.inicio[i] == 56 || usuario.inicio[i] == 57)
                {
                    horaInicio = horaInicio + usuario.inicio[i];
                }
            }
            int inicio = Int32.Parse(horaInicio);
            for (int i = 0; i < usuario.fin.Length; i++)
            {
                if (usuario.fin[i] == 48 || usuario.fin[i] == 49 || usuario.fin[i] == 50 || usuario.fin[i] == 51 || usuario.fin[i] == 52 || usuario.fin[i] == 53 || usuario.fin[i] == 54 || usuario.fin[i] == 55 || usuario.fin[i] == 56 || usuario.fin[i] == 57)
                {
                    horaFin = horaFin + usuario.fin[i];
                }
            }
            int fin = Int32.Parse(horaFin);
            int numero = usuario.telefono;
            int contador = 0;
            while (numero >= 1)
            {
                contador = contador + 1;
                numero = numero / 10;
            }
            if (usuarioExiste)
            {
                if (inicio < fin)
                {
                    if (contador == 9)
                    {
                        if (usuario.numeroDni.Length == 8)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool validarSiExisteUsuario(Context contexto, string nombre, string apellido, string dni)
        {
            var usuario = contexto.usuarios.Where(u => u.nombre == nombre).Where(u => u.apellido == apellido).Where(u => u.numeroDni == dni).FirstOrDefault();
            if (usuario != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
