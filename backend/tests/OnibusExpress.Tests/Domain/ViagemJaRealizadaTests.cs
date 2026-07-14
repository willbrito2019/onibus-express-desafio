using System.Reflection;
using OnibusExpress.Domain.Entities;
using Shouldly;
using Xunit;

namespace OnibusExpress.Tests.Domain;

public class ViagemJaRealizadaTests
{
    [Fact]
    public void JaRealizada_Deve_Retornar_True_Quando_Data_Partida_Ja_Passou()
    {
        var viagem = new Viagem(Guid.NewGuid(), DateTime.UtcNow.AddDays(1), 150m, 40);

        // simula o tempo passando: usa reflection pra "voltar" a data,
        // já que a entidade legitimamente impede criar viagem com data passada
        var propriedade = typeof(Viagem).GetProperty(nameof(Viagem.DataHoraPartida))!;
        propriedade.SetValue(viagem, DateTime.UtcNow.AddHours(-1));

        viagem.JaRealizada().ShouldBeTrue();
    }
}