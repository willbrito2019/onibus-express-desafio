using OnibusExpress.Application.Validators;
using Shouldly;
using Xunit;

namespace OnibusExpress.Tests.Application;

public class CpfValidatorTests
{
    [Theory]
    [InlineData("11144477735")]   // CPF válido
    [InlineData("111.444.777-35")] // formatado
    public void Deve_Validar_Cpf_Correto(string cpf)
    {
        CpfValidator.EhValido(cpf).ShouldBeTrue();
    }

    [Theory]
    [InlineData("12345678900")]     // dígitos verificadores errados
    [InlineData("11111111111")]     // todos iguais
    [InlineData("123")]             // tamanho errado
    [InlineData("")]
    [InlineData(null)]
    [InlineData("abc.def.ghi-jk")]  // não numérico
    public void Nao_Deve_Validar_Cpf_Invalido(string? cpf)
    {
        CpfValidator.EhValido(cpf).ShouldBeFalse();
    }
}