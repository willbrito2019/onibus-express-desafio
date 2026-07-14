using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Domain.Interfaces;

public interface IViagemRepository
{
    Task<List<Viagem>> BuscarAsync(string? origem, string? destino, DateTime? data);
    Task<Viagem?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Viagem viagem);
    Task SalvarAlteracoesAsync();
}