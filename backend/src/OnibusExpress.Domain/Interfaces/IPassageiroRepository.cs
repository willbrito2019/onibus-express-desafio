using OnibusExpress.Domain.Entities;

namespace OnibusExpress.Domain.Interfaces;

public interface IPassageiroRepository
{
    Task<Passageiro?> ObterPorCpfAsync(string cpf);
    Task AdicionarAsync(Passageiro passageiro);
    Task SalvarAlteracoesAsync();
}