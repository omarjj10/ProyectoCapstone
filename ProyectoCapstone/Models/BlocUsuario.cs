using System.ComponentModel.DataAnnotations;

namespace ProyectoCapstone.Models
{
    public class BlocUsuario
    {
        public int BlocUsuarioID { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El apellido es obligatorio")]
        public string apellido { get; set; }
        [Required(ErrorMessage = "El correo es obligatorio")]
        public string correo { get; set; }
        public int? codigoVerificarCorreo { get; set; }
        [Required(ErrorMessage = "La clave de usuario es obligatorio")]
        public string clave { get; set; }
        public string? discapacidad { get; set; }
        public string? diaDisponible { get; set; }
        public string? inicio { get; set; }
        public string? fin { get; set; }
        public int? recomendacion { get; set; }
        public string? estado { get; set; }
        [MaxLength(8, ErrorMessage = "El nmero del DNI tiene como maximo 8 caracteres")]
        [Required(ErrorMessage = "El numero del DNI es obligatorio")]
        public string numeroDni { get; set; }
        public string? direccion { get; set; }
        [Required(ErrorMessage = "El numero del telefono es obligatorio")]
        public int telefono { get; set; }
    }
}
