using OnibusExpress.Domain.Entities;
using Shouldly;
using Xunit;

namespace OnibusExpress.Tests.Domain;

public class PassageiroTests
{
    [Fact]
    public void Deve_Criar_Passageiro_Valido()
    {
        var passageiro = new Passageiro(
            "João da Silva",
            "12345678909",
            "joao@email.com",
            new DateOnly(1990, 5, 10));

        passageiro.Id.ShouldNotBe(Guid.Empty);
        passageiro.Nome.ShouldBe("João da Silva");
        passageiro.Cpf.ShouldBe("12345678909");
        passageiro.Email.ShouldBe("joao@email.com");
    }

    [Theory]
    [InlineData("")]    
    public void Nao_Deve_Criar_Passageiro_Com_Nome_Vazio(string nome)
    {
        Should.Throw<ArgumentException>(() =>
            new Passageiro(nome, "12345678909", "joao@email.com", new DateOnly(1990, 5, 10)));
    }

    [Theory]
    [InlineData("")]
    [InlineData("email-invalido")]    
    public void Nao_Deve_Criar_Passageiro_Com_Email_Invalido(string email)
    {
        Should.Throw<ArgumentException>(() =>
            new Passageiro("João da Silva", "12345678909", email, new DateOnly(1990, 5, 10)));
    }
}