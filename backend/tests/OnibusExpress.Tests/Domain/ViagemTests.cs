using OnibusExpress.Domain.Entities;
using Shouldly;
using Xunit;

namespace OnibusExpress.Tests.Domain;

public class ViagemTests
{
    [Fact]
    public void Deve_Criar_Viagem_Valida()
    {
        var rotaId = Guid.NewGuid();
        var partida = DateTime.UtcNow.AddDays(1);

        var viagem = new Viagem(rotaId, partida, precoBase: 150m, assentosDisponiveis: 40);

        viagem.Id.ShouldNotBe(Guid.Empty);
        viagem.RotaId.ShouldBe(rotaId);
        viagem.PrecoBase.ShouldBe(150m);
        viagem.AssentosDisponiveis.ShouldBe(40);
    }

    [Fact]
    public void Nao_Deve_Criar_Viagem_Com_Data_Partida_No_Passado()
    {
        var partida = DateTime.UtcNow.AddDays(-1);

        Should.Throw<ArgumentException>(() =>
            new Viagem(Guid.NewGuid(), partida, 150m, 40));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Nao_Deve_Criar_Viagem_Com_Preco_Invalido(decimal preco)
    {
        Should.Throw<ArgumentException>(() =>
            new Viagem(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), preco, 40));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Nao_Deve_Criar_Viagem_Com_Assentos_Invalidos(int assentos)
    {
        Should.Throw<ArgumentException>(() =>
            new Viagem(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), 150m, assentos));
    }

    [Fact]
    public void JaRealizada_Deve_Retornar_True_Quando_Data_Partida_No_Passado()
    {       
        var viagem = new Viagem(Guid.NewGuid(), DateTime.UtcNow.AddSeconds(2), 150m, 40);

        viagem.JaRealizada().ShouldBeFalse();
    }
}