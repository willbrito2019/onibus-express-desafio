using Microsoft.AspNetCore.Mvc;
using OnibusExpress.Api.Dtos;
using OnibusExpress.Domain.Entities;
using OnibusExpress.Domain.Interfaces;

namespace OnibusExpress.Api.Controllers;

[ApiController]
[Route("viagens")]
public class ViagensController : ControllerBase
{
    private readonly IViagemRepository _viagemRepository;

    public ViagensController(IViagemRepository viagemRepository) => _viagemRepository = viagemRepository;

    [HttpGet]
    public async Task<IActionResult> Buscar([FromQuery] string? origem, [FromQuery] string? destino, [FromQuery] DateTime? data)
    {
        var viagens = await _viagemRepository.BuscarAsync(origem, destino, data);

        var response = viagens.Select(v => new ViagemResumoResponse(
            v.Id, v.Rota.Origem, v.Rota.Destino, v.DataHoraPartida, v.PrecoBase, v.AssentosDisponiveis));

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterDetalhes(Guid id)
    {
        var viagem = await _viagemRepository.ObterPorIdAsync(id);
        if (viagem is null)
            return NotFound(new { mensagem = "Viagem não encontrada." });

        var ocupados = viagem.Reservas
            .Where(r => r.Status == StatusReserva.Confirmada)
            .Select(r => r.NumeroAssento)
            .OrderBy(n => n)
            .ToList();

        var livres = Enumerable.Range(1, viagem.AssentosDisponiveis)
            .Except(ocupados)
            .ToList();

        var response = new ViagemDetalhesResponse(
            viagem.Id, viagem.Rota.Origem, viagem.Rota.Destino, viagem.DataHoraPartida,
            viagem.PrecoBase, viagem.AssentosDisponiveis, ocupados, livres);

        return Ok(response);
    }
}