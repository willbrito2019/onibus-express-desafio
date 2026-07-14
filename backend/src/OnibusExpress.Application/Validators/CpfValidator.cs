using System.Text.RegularExpressions;

namespace OnibusExpress.Application.Validators;

public static class CpfValidator
{
    public static bool EhValido(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var digitos = Regex.Replace(cpf, @"[^\d]", "");

        if (digitos.Length != 11)
            return false;
       
        if (digitos.Distinct().Count() == 1)
            return false;

        return DigitosVerificadoresValidos(digitos);
    }

    private static bool DigitosVerificadoresValidos(string cpf)
    {
        var numeros = cpf.Select(c => c - '0').ToArray();

        var primeiroDigito = CalcularDigito(numeros, 9);
        if (primeiroDigito != numeros[9])
            return false;

        var segundoDigito = CalcularDigito(numeros, 10);
        if (segundoDigito != numeros[10])
            return false;

        return true;
    }

    private static int CalcularDigito(int[] numeros, int quantidade)
    {
        var soma = 0;
        var multiplicador = quantidade + 1;

        for (var i = 0; i < quantidade; i++)
        {
            soma += numeros[i] * multiplicador;
            multiplicador--;
        }

        var resto = soma % 11;
        return resto < 2 ? 0 : 11 - resto;
    }
}