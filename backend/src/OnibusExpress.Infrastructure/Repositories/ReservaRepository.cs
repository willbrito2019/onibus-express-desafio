using Microsoft.EntityFrameworkCore;
using OnibusExpress.Domain.Interfaces;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Infrastructure.Persistence;
using System;

namespace OnibusExpress.Infrastructure.Repositories;

public class ReservaRepository : IReservaRepository
{
    private readonly AppDbContext _context;

    public ReservaRepository(AppDbContext context) => _context = context;

    public Task<Reserva?> ObterPorCodigoAsync(string codigo) =>
        _context.Reservas
            .Include(r => r.Viagem)
            .Include(r => r.Passageiro)
            .FirstOrDefaultAsync(r => r.CodigoReserva == codigo);

    public Task<bool> AssentoOcupadoAsync(Guid viagemId, int numeroAssento) =>
        _context.Reservas.AnyAsync(r =>
            r.ViagemId == viagemId &&
            r.NumeroAssento == numeroAssento &&
            r.Status == StatusReserva.Confirmada);

    public Task<bool> CodigoJaExisteAsync(string codigo) =>
        _context.Reservas.AnyAsync(r => r.CodigoReserva == codigo);

    public Task AdicionarAsync(Reserva reserva)
    {
        _context.Reservas.Add(reserva);
        return Task.CompletedTask;
    }

    public Task SalvarAlteracoesAsync() => _context.SaveChangesAsync();
}