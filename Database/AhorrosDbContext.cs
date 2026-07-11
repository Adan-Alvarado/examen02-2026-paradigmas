using ExamenU2Paradigmas.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamenU2Paradigmas.Database
{
    public class AhorrosDbContext : DbContext
    {
        public AhorrosDbContext(DbContextOptions<AhorrosDbContext> options) : base(options)
        {
        }

        public DbSet<Simulacion> Simulaciones { get; set; }
        public DbSet<ProyeccionMes> ProyeccionesMensuales { get; set; }
        public DbSet<ProyeccionAnio> ProyeccionesAnuales { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar relaciones FK
            modelBuilder.Entity<ProyeccionMes>()
                .HasOne(p => p.Simulacion)
                .WithMany(s => s.ProyeccionesMensuales)
                .HasForeignKey(p => p.SimulacionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProyeccionAnio>()
                .HasOne(p => p.Simulacion)
                .WithMany(s => s.ProyeccionesAnuales)
                .HasForeignKey(p => p.SimulacionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
