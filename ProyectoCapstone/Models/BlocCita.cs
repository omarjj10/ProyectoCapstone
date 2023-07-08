using System.ComponentModel.DataAnnotations;

namespace ProyectoCapstone.Models
{
    public class BlocCita
    {
        public int BlocCitaID { get; set; }
        public string lugarEncuentro { get; set; }
        [Required(ErrorMessage = "El dia de encuentro es obligatorio")]
        public string dia { get; set; }
        [Required(ErrorMessage = "El horario de encuentro es obligatorio")]
        public string horario { get; set; }
        [Required(ErrorMessage = "El tiempo que durara la cita de encuentro es obligatorio")]
        public string tiempoCita { get; set; }
        public string nombreDiscapacitado { get; set; }
        [Required(ErrorMessage = "El nombre del voluntario es obligatorio")]
        public string nombreVoluntario { get; set; }
        public Usuario? usuario { get; set; }
    }
}
