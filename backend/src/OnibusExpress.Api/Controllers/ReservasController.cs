using Microsoft.AspNetCore.Mvc;
using OnibusExpress.Api.Dtos;
using OnibusExpress.Application.Services;
using OnibusExpress.Application.Validators;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Domain.Interfaces;

namespace OnibusExpress.Api.Controllers;

[ApiController]
[Route("reservas")]
public class ReservasController : ControllerBase
{
    private readonly IReservaRepository _reservaRepository;
    private readonly IViagemRepository _viagemRepository;
    private readonly IPassageiroRepository _passageiroRepository;
    private readonly IGeradorCodigoReserva _geradorCodigo;

    public ReservasController(
        IReservaRepository reservaRepository,
        IViagemRepository viagemRepository,
        IPassageiroRepository passageiroRepository,
        IGeradorCodigoReserva geradorCodigo)
    {
        _reservaRepository = reservaRepository;
        _viagemRepository = viagemRepository;
        _passageiroRepository = passageiroRepository;
        _geradorCodigo = geradorCodigo;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarReservaRequest request)
    {
        if (!CpfValidator.EhValido(request.Cpf))
            return BadRequest(new { mensagem = "CPF inválido." });

        var cpfLimpo = new string(request.Cpf.Where(char.IsDigit).ToArray());

        var viagem = await _viagemRepository.ObterPorIdAsync(request.ViagemId);
        if (viagem is null)
            return NotFound(new { mensagem = "Viagem não encontrada." });

        if (viagem.JaRealizada())
            return BadRequest(new { mensagem = "Não é possível reservar passagem para viagem já realizada." });

        if (request.NumeroAssento < 1 || request.NumeroAssento > viagem.AssentosDisponiveis)
            return BadRequest(new { mensagem = "Número de assento fora do intervalo da viagem." });

        var assentoOcupado = await _reservaRepository.AssentoOcupadoAsync(viagem.Id, request.NumeroAssento);
        if (assentoOcupado)
            return Conflict(new { mensagem = "Assento já ocupado." });

        var passageiroExistente = await _passageiroRepository.ObterPorCpfAsync(cpfLimpo);
        var passageiro = passageiroExistente
            ?? new Passageiro(request.Nome, cpfLimpo, request.Email, request.DataNascimento);

        if (passageiroExistente is null)
            await _passageiroRepository.AdicionarAsync(passageiro);

        var codigo = await GerarCodigoUnicoAsync();
        var reserva = new Reserva(viagem.Id, passageiro.Id, request.NumeroAssento, codigo);

        await _reservaRepository.AdicionarAsync(reserva);
        await _reservaRepository.SalvarAlteracoesAsync();

        var response = new ReservaResponse(
            reserva.CodigoReserva, reserva.Status.ToString(), reserva.NumeroAssento,
            passageiro.Nome, viagem.Id, viagem.DataHoraPartida, reserva.CriadaEm);

        return CreatedAtAction(nameof(ObterPorCodigo), new { codigo = reserva.CodigoReserva }, response);
    }

    [HttpGet("{codigo}")]
    public async Task<IActionResult> ObterPorCodigo(string codigo)
    {
        var reserva = await _reservaRepository.ObterPorCodigoAsync(codigo);
        if (reserva is null)
            return NotFound(new { mensagem = "Reserva não encontrada." });

        var response = new ReservaResponse(
            reserva.CodigoReserva, reserva.Status.ToString(), reserva.NumeroAssento,
            reserva.Passageiro.Nome, reserva.ViagemId, reserva.Viagem.DataHoraPartida, reserva.CriadaEm);

        return Ok(response);
    }

    [HttpDelete("{codigo}")]
    public async Task<IActionResult> Cancelar(string codigo)
    {
        var reserva = await _reservaRepository.ObterPorCodigoAsync(codigo);
        if (reserva is null)
            return NotFound(new { mensagem = "Reserva não encontrada." });

        try
        {
            reserva.Cancelar(reserva.Viagem.DataHoraPartida);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }

        await _reservaRepository.SalvarAlteracoesAsync();
        return NoContent();
    }

    private async Task<string> GerarCodigoUnicoAsync()
    {
        var tentativas = 0;
        string codigo;

        do
        {
            codigo = _geradorCodigo.Gerar();
            tentativas++;
            if (tentativas > 10)
                throw new InvalidOperationException("Não foi possível gerar um código de reserva único.");
        }
        while (await _reservaRepository.CodigoJaExisteAsync(codigo));

        return codigo;
    }
}