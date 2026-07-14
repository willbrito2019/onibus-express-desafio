using OnibusExpress.Domain.Entities;
using Shouldly;
using Xunit;

namespace OnibusExpress.Tests.Domain;

public class RotaTests
{
    [Fact]
    public void Deve_Criar_Rota_Valida()
    {
        var rota = new Rota("São Paulo", "Rio de Janeiro", TimeSpan.FromHours(6));

        rota.Id.ShouldNotBe(Guid.Empty);
        rota.Origem.ShouldBe("São Paulo");
        rota.Destino.ShouldBe("Rio de Janeiro");
        rota.DuracaoEstimada.ShouldBe(TimeSpan.FromHours(6));
    }

    [Theory]
    [InlineData("", "Rio de Janeiro")]    
    [InlineData("São Paulo", "")]    
    public void Nao_Deve_Criar_Rota_Com_Origem_Ou_Destino_Vazio(string origem, string destino)
    {
        Should.Throw<ArgumentException>(() => new Rota(origem, destino, TimeSpan.FromHours(6)));
    }

    [Fact]
    public void Nao_Deve_Criar_Rota_Com_Duracao_Zero_Ou_Negativa()
    {
        Should.Throw<ArgumentException>(() => new Rota("São Paulo", "Rio de Janeiro", TimeSpan.Zero));
    }
}