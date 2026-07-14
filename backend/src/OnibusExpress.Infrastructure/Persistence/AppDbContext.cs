using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Rota> Rotas => Set<Rota>();
    public DbSet<Viagem> Viagens => Set<Viagem>();
    public DbSet<Passageiro> Passageiros => Set<Passageiro>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rota>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.Origem).IsRequired().HasMaxLength(100);
            e.Property(r => r.Destino).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Viagem>(e =>
        {
            e.HasKey(v => v.Id);
            e.Property(v => v.PrecoBase).HasColumnType("decimal(10,2)");
            e.HasOne(v => v.Rota)
                .WithMany(r => r.Viagens)
                .HasForeignKey(v => v.RotaId);
        });

        modelBuilder.Entity<Passageiro>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Cpf).IsRequired().HasMaxLength(11);
            e.HasIndex(p => p.Cpf).IsUnique();
        });

        modelBuilder.Entity<Reserva>(e =>
        {
            e.HasKey(r => r.Id);
            e.Property(r => r.CodigoReserva).IsRequired().HasMaxLength(20);
            e.HasIndex(r => r.CodigoReserva).IsUnique();
            e.HasOne(r => r.Viagem)
                .WithMany(v => v.Reservas)
                .HasForeignKey(r => r.ViagemId);
            e.HasOne(r => r.Passageiro)
                .WithMany()
                .HasForeignKey(r => r.PassageiroId);

            // regra: mesmo assento não pode ser reservado 2x na mesma viagem
            e.HasIndex(r => new { r.ViagemId, r.NumeroAssento }).IsUnique();
        });
    }
}