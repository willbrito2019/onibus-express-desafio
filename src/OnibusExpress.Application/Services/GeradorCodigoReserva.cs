namespace OnibusExpress.Application.Services;

public interface IGeradorCodigoReserva
{
    string Gerar();
}

public class GeradorCodigoReserva : IGeradorCodigoReserva
{
    private const string Letras = "ABCDEFGHJKLMNPQRSTUVWXYZ"; // sem I, O (confundem com 1, 0)
    private static readonly Random Random = new();

    public string Gerar()
    {
        var prefixo = new string(Enumerable.Range(0, 3)
            .Select(_ => Letras[Random.Next(Letras.Length)])
            .ToArray());

        var numero = Random.Next(0, 100000).ToString("D5");

        return $"{prefixo}-{numero}";
    }
}