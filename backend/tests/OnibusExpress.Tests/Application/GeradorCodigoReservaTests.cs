using System.Text.RegularExpressions;
using OnibusExpress.Application.Services;
using Shouldly;
using Xunit;

namespace OnibusExpress.Tests.Application;

public class GeradorCodigoReservaTests
{
    private readonly GeradorCodigoReserva _gerador = new();

    [Fact]
    public void Deve_Gerar_Codigo_No_Formato_Esperado()
    {
        var codigo = _gerador.Gerar();

        Regex.IsMatch(codigo, @"^[A-Z]{3}-\d{5}$").ShouldBeTrue();
    }

    [Fact]
    public void Deve_Gerar_Codigos_Diferentes_Em_Chamadas_Sucessivas()
    {
        var codigos = Enumerable.Range(0, 100)
            .Select(_ => _gerador.Gerar())
            .ToList();

        // não garante unicidade absoluta (isso é responsabilidade do banco/índice único),
        // mas garante que o gerador não é determinístico/travado num valor fixo
        codigos.Distinct().Count().ShouldBeGreaterThan(1);
    }
}