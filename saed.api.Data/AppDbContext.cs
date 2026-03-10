using Microsoft.EntityFrameworkCore;
using saed.api.Model;

namespace saed.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Proceso> Procesos { get; set; }

        public DbSet<Academia> Academias { get; set; }

        public DbSet<Instrumento> Instrumentos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Pregunta> Preguntas { get; set; }

        public DbSet<OpcionRespuesta> OpcionesRespuesta { get; set; }

        public DbSet<Evaluacion> Evaluaciones { get; set; }

        public DbSet<Respuesta> Respuestas { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }
    }
}