using Microsoft.EntityFrameworkCore;

namespace _3raPracticaProgramada_OscarNaranjoZuniga.Models
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Comentarios> Comentarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comentarios>()
                .Property(c => c.FechaCreacion)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Comentarios>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Comentarios)
                .HasForeignKey(c => c.UsuarioId);
        }
    }
}
