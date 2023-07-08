using System.ComponentModel.DataAnnotations;

namespace ProyectoCapstone.Models
{
    public class Cita
    {
        public int CitaID { get; set; }
        [Required(ErrorMessage = "El lugar de encuentro es obligatorio")]
        public string lugarEncuentro { get; set; }
        [Required(ErrorMessage = "La fecha de la cita es obligatorio")]
        public string fecha { get; set; }
        [Required(ErrorMessage = "El tiempo de duracion de la cita es obligatorio")]
        public string horaInicio { get; set; }
        public string tiempoCita { get; set; }
        public string nombreDiscapacitado { get; set; }
        [Required(ErrorMessage = "El nombre del discapacitado es obligatorio")]
        public string nombreVoluntario { get; set; }
        public Usuario? usuario { get; set; }
    }
}
