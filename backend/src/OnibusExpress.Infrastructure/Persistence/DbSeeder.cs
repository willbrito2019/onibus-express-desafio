using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Infrastructure.Persistence;

public static class DbSeeder
{
    public static void Seed(AppDbContext context)
    {
        if (context.Rotas.Any()) return; // já populado

        var rota1 = new Rota("São Paulo", "Rio de Janeiro", TimeSpan.FromHours(6));
        var rota2 = new Rota("São Paulo", "Curitiba", TimeSpan.FromHours(7));

        context.Rotas.AddRange(rota1, rota2);
        context.SaveChanges();

        context.Viagens.AddRange(
            new Viagem(rota1.Id, DateTime.UtcNow.AddDays(2), 150m, 40),
            new Viagem(rota2.Id, DateTime.UtcNow.AddDays(3), 120m, 30)
        );
        context.SaveChanges();
    }
}