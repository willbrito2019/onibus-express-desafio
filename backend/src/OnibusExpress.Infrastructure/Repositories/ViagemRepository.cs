using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Domain.Interfaces;
using OnibusExpress.Infrastructure.Persistence;

namespace OnibusExpress.Infrastructure.Repositories;

public class ViagemRepository : IViagemRepository
{
    private readonly AppDbContext _context;
    public ViagemRepository(AppDbContext context) => _context = context;

    public async Task<List<Viagem>> BuscarAsync(string? origem, string? destino, DateTime? data)
    {
        var query = _context.Viagens
            .Include(v => v.Rota)
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(origem))
            query = query.Where(v => v.Rota.Origem.Contains(origem));

        if (!string.IsNullOrWhiteSpace(destino))
            query = query.Where(v => v.Rota.Destino.Contains(destino));

        if (data.HasValue)
            query = query.Where(v => v.DataHoraPartida.Date == data.Value.Date);

        return await query.ToListAsync();
    }

    public Task<Viagem?> ObterPorIdAsync(Guid id) =>
        _context.Viagens
            .Include(v => v.Rota)
            .Include(v => v.Reservas)
            .FirstOrDefaultAsync(v => v.Id == id);

    public Task AdicionarAsync(Viagem viagem)
    {
        _context.Viagens.Add(viagem);
        return Task.CompletedTask;
    }

    public Task SalvarAlteracoesAsync() => _context.SaveChangesAsync();
}