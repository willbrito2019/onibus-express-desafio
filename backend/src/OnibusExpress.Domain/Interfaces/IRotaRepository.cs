using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Domain.Interfaces;

public interface IRotaRepository
{
    Task<List<Rota>> ObterTodasAsync();
    Task<Rota?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Rota rota);
    Task SalvarAlteracoesAsync();
}