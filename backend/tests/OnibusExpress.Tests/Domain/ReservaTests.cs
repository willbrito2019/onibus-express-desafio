using OnibusExpress.Domain.Entities;
using Shouldly;
using Xunit;

namespace OnibusExpress.Tests.Domain;

public class ReservaTests
{
    [Fact]
    public void Deve_Criar_Reserva_Valida()
    {
        var reserva = new Reserva(Guid.NewGuid(), Guid.NewGuid(), numeroAssento: 15, codigoReserva: "ABC-12345");

        reserva.Id.ShouldNotBe(Guid.Empty);
        reserva.NumeroAssento.ShouldBe(15);
        reserva.CodigoReserva.ShouldBe("ABC-12345");
        reserva.Status.ShouldBe(StatusReserva.Confirmada);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Nao_Deve_Criar_Reserva_Com_Assento_Invalido(int assento)
    {
        Should.Throw<ArgumentException>(() =>
            new Reserva(Guid.NewGuid(), Guid.NewGuid(), assento, "ABC-12345"));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Nao_Deve_Criar_Reserva_Com_Codigo_Vazio(string? codigo)
    {
        Should.Throw<ArgumentException>(() =>
            new Reserva(Guid.NewGuid(), Guid.NewGuid(), 15, codigo!));
    }

    [Fact]
    public void Deve_Cancelar_Reserva_Quando_Faltam_Mais_De_2_Horas_Para_Partida()
    {
        var reserva = new Reserva(Guid.NewGuid(), Guid.NewGuid(), 15, "ABC-12345");
        var partida = DateTime.UtcNow.AddHours(3);

        reserva.Cancelar(partida);

        reserva.Status.ShouldBe(StatusReserva.Cancelada);
    }

    [Fact]
    public void Nao_Deve_Cancelar_Reserva_Quando_Faltam_Menos_De_2_Horas_Para_Partida()
    {
        var reserva = new Reserva(Guid.NewGuid(), Guid.NewGuid(), 15, "ABC-12345");
        var partida = DateTime.UtcNow.AddHours(1);

        Should.Throw<InvalidOperationException>(() => reserva.Cancelar(partida));

        reserva.Status.ShouldBe(StatusReserva.Confirmada); // garante que não mudou o estado
    }

    [Fact]
    public void Nao_Deve_Cancelar_Reserva_Exatamente_No_Limite_De_2_Horas()
    {
        var reserva = new Reserva(Guid.NewGuid(), Guid.NewGuid(), 15, "ABC-12345");
        var partida = DateTime.UtcNow.AddHours(2).AddSeconds(-1);

        Should.Throw<InvalidOperationException>(() => reserva.Cancelar(partida));
    }
}