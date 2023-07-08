using System.ComponentModel.DataAnnotations;

namespace ProyectoCapstone.Models
{
    public class Contactanos
    {
        public int ContactanosID { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string telefono { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string email { get; set; }
        [Required(ErrorMessage = "Este campo es requerido")]
        public string comentario { get; set; }
    }
}
