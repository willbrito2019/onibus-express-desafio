namespace OnibusExpress.Domain.Entities;

public class Viagem
{
    public Guid Id { get; private set; }
    public Guid RotaId { get; private set; }
    public Rota Rota { get; private set; } = null!;
    public DateTime DataHoraPartida { get; private set; }
    public decimal PrecoBase { get; private set; }
    public int AssentosDisponiveis { get; private set; }

    public ICollection<Reserva> Reservas { get; private set; } = new List<Reserva>();

    protected Viagem() { } // EF

    public Viagem(Guid rotaId, DateTime dataHoraPartida, decimal precoBase, int assentosDisponiveis)
    {
        if (dataHoraPartida <= DateTime.UtcNow)
            throw new ArgumentException("Data/hora de partida deve ser futura.");
        if (precoBase <= 0)
            throw new ArgumentException("Preço base deve ser maior que zero.");
        if (assentosDisponiveis <= 0)
            throw new ArgumentException("Deve haver ao menos 1 assento disponível.");

        Id = Guid.NewGuid();
        RotaId = rotaId;
        DataHoraPartida = dataHoraPartida;
        PrecoBase = precoBase;
        AssentosDisponiveis = assentosDisponiveis;
    }

    public bool JaRealizada() => DataHoraPartida <= DateTime.UtcNow;
}