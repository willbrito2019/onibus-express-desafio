using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Domain.Interfaces;
using OnibusExpress.Infrastructure.Persistence;

namespace OnibusExpress.Infrastructure.Repositories;

public class PassageiroRepository : IPassageiroRepository
{
    private readonly AppDbContext _context;
    public PassageiroRepository(AppDbContext context) => _context = context;

    public Task<Passageiro?> ObterPorCpfAsync(string cpf) =>
        _context.Passageiros.FirstOrDefaultAsync(p => p.Cpf == cpf);

    public Task AdicionarAsync(Passageiro passageiro)
    {
        _context.Passageiros.Add(passageiro);
        return Task.CompletedTask;
    }

    public Task SalvarAlteracoesAsync() => _context.SaveChangesAsync();
}