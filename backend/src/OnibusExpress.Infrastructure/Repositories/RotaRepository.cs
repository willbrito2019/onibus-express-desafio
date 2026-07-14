using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Interfaces;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Infrastructure.Persistence;

namespace OnibusExpress.Infrastructure.Repositories;

public class RotaRepository : IRotaRepository
{
    private readonly AppDbContext _context;
    public RotaRepository(AppDbContext context) => _context = context;

    public Task<List<Rota>> ObterTodasAsync() =>
        _context.Rotas.AsNoTracking().ToListAsync();

    public Task<Rota?> ObterPorIdAsync(Guid id) =>
        _context.Rotas.FirstOrDefaultAsync(r => r.Id == id);

    public Task AdicionarAsync(Rota rota)
    {
        _context.Rotas.Add(rota);
        return Task.CompletedTask;
    }

    public Task SalvarAlteracoesAsync() => _context.SaveChangesAsync();
}