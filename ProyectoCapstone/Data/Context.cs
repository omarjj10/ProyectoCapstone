using ProyectoCapstone.Models;
using Microsoft.EntityFrameworkCore;

namespace ProyectoCapstone.Data
{
    public class Context:DbContext
    {
        public Context(DbContextOptions<Context> options):base(options) { }
        public DbSet<Rol> rols { get; set; }
        public virtual DbSet<Usuario> usuarios { get; set; }
        public virtual DbSet<Cita> cita { get; set; }
        public virtual DbSet<BlocCita> blocCitas { get; set; }
        public DbSet<Contactanos> contactanos { get; set; }
        public DbSet<Recomendacion> recomendaciones { get; set; }
        public DbSet<BlocUsuario> blocUsuarios { get; set; }
    }
}
