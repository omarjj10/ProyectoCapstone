namespace ProyectoCapstone.Models
{
    public class Recomendacion
    {
        public int RecomendacionID { get; set; }
        public string? nombreVoluntario { get; set; }
        public string? nombreDiscapacitado { get; set; }
        public int? calificacion { get; set; }
        public string? comentario { get; set; }
        public Usuario? usuario { get; set;}
    }
}
