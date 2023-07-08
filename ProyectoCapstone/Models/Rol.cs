namespace ProyectoCapstone.Models
{
    public class Rol
    {
        public int RolID { get; set; }
        public string? descripcion { get; set; }
        public ICollection<Usuario>? usuarios { get; set; }
    }
}
