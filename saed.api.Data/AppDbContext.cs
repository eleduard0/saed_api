using Microsoft.EntityFrameworkCore;
using saed.api.Model;

namespace saed.api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        #region DbSets
        public DbSet<Proceso> Procesos { get; set; }

        public DbSet<Academia> Academias { get; set; }

        public DbSet<Instrumento> Instrumentos { get; set; }

        public DbSet<Categoria> Categorias { get; set; }

        public DbSet<Pregunta> Preguntas { get; set; }

        public DbSet<OpcionRespuesta> OpcionesRespuesta { get; set; }

        public DbSet<Evaluacion> Evaluaciones { get; set; }

        public DbSet<Respuesta> Respuestas { get; set; }

        public DbSet<Comentario> Comentarios { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Instrumento
            // Mapeo del grafo completo de entidades
            modelBuilder.Entity<Instrumento>().ToTable("Instrumentos");
            modelBuilder.Entity<Categoria>().ToTable("Categorias");
            modelBuilder.Entity<Pregunta>().ToTable("Preguntas");
            modelBuilder.Entity<OpcionRespuesta>().ToTable("OpcionesRespuesta");

            modelBuilder.Entity<Instrumento>()
                .Property(i => i.TipoInstrumento)
                .HasConversion<string>();

            modelBuilder.Entity<Pregunta>()
                .Property(p => p.Tipo)
                .HasConversion<string>();

            modelBuilder.Entity<Categoria>()
                .HasOne(c => c.Instrumento)
                .WithMany(i => i.Categorias)
                .HasForeignKey(c => c.InstrumentoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Instrumento>()
                .HasOne(i => i.Proceso)
                .WithMany()
                .HasForeignKey(i => i.ProcesoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Pregunta>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Preguntas)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OpcionRespuesta>()
                .HasOne(o => o.Pregunta)
                .WithMany(p => p.Opciones)
                .HasForeignKey(o => o.PreguntaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Categoria>()
                .HasIndex(c => c.InstrumentoId);

            modelBuilder.Entity<Pregunta>()
                .HasIndex(p => p.CategoriaId);

            modelBuilder.Entity<OpcionRespuesta>()
                .HasIndex(o => o.PreguntaId);

            modelBuilder.Entity<Instrumento>()
                .HasIndex(i => i.ProcesoId);

            modelBuilder.Entity<Instrumento>()
                .Property(i => i.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Categoria>()
                .Property(c => c.Nombre)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Pregunta>()
                .Property(p => p.Texto)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<OpcionRespuesta>()
                .Property(o => o.Texto)
                .HasMaxLength(300)
                .IsRequired();
            #endregion
        }
    }
}
