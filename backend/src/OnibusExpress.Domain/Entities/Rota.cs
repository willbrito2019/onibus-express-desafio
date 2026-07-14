namespace OnibusExpress.Domain.Entities;

public class Rota
{
    public Guid Id { get; private set; }
    public string Origem { get; private set; } = string.Empty;
    public string Destino { get; private set; } = string.Empty;
    public TimeSpan DuracaoEstimada { get; private set; }

    public ICollection<Viagem> Viagens { get; private set; } = new List<Viagem>();

    protected Rota() { } // EF

    public Rota(string origem, string destino, TimeSpan duracaoEstimada)
    {
        if (string.IsNullOrWhiteSpace(origem))
            throw new ArgumentException("Origem é obrigatória.");
        if (string.IsNullOrWhiteSpace(destino))
            throw new ArgumentException("Destino é obrigatório.");
        if (duracaoEstimada <= TimeSpan.Zero)
            throw new ArgumentException("Duração estimada deve ser maior que zero.");

        Id = Guid.NewGuid();
        Origem = origem;
        Destino = destino;
        DuracaoEstimada = duracaoEstimada;
    }
}