using System.ComponentModel.DataAnnotations;

namespace ProyectoCapstone.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public int RolID { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string apellido { get; set; }
        public string? correo { get; set; }
        public int? codigoVerificarCorreo { get; set; }
        public string? clave { get; set; }
        public string? discapacidad { get; set; }
        public string? diaDisponible { get; set; }
        public string? inicio { get; set; }
        public string? fin { get; set; }
        public int? recomendacion { get; set; }
        public string? estado { get; set; }
        [MaxLength(8, ErrorMessage ="El nmero del DNI tiene como maximo 8 caracteres")]
        [Required(ErrorMessage = "El numero del DNI es obligatorio")]
        public string numeroDni { get; set; }
        public string? direccion { get; set; }
        [Required(ErrorMessage = "El numero del telefono es obligatorio")]
        public int telefono { get; set; }
        public Rol? rol { get; set; }
        public ICollection<Cita>? citas { get; set; }
        public ICollection<BlocCita>? blocCitas { get; set; }
        public ICollection<Recomendacion>? recomendaciones { get; set;}
    }
}
