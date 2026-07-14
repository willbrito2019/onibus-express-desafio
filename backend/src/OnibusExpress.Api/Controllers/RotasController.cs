using Microsoft.AspNetCore.Mvc;
using OnibusExpress.Api.Dtos;
using OnibusExpress.Domain.Interfaces;

namespace OnibusExpress.Api.Controllers;

[ApiController]
[Route("rotas")]
public class RotasController : ControllerBase
{
    private readonly IRotaRepository _rotaRepository;

    public RotasController(IRotaRepository rotaRepository) => _rotaRepository = rotaRepository;

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var rotas = await _rotaRepository.ObterTodasAsync();

        var response = rotas.Select(r => new RotaResponse(r.Id, r.Origem, r.Destino, r.DuracaoEstimada));

        return Ok(response);
    }
}