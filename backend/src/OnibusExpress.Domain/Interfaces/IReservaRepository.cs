using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Domain.Interfaces;

public interface IReservaRepository
{
    Task<Reserva?> ObterPorCodigoAsync(string codigo);
    Task<bool> AssentoOcupadoAsync(Guid viagemId, int numeroAssento);
    Task<bool> CodigoJaExisteAsync(string codigo);
    Task AdicionarAsync(Reserva reserva);
    Task SalvarAlteracoesAsync();
}