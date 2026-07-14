namespace OnibusExpress.Domain.Entities;

public enum StatusReserva
{
    Confirmada = 1,
    Cancelada = 2
}

public class Reserva
{
    public Guid Id { get; private set; }
    public Guid ViagemId { get; private set; }
    public Viagem Viagem { get; private set; } = null!;
    public Guid PassageiroId { get; private set; }
    public Passageiro Passageiro { get; private set; } = null!;
    public int NumeroAssento { get; private set; }
    public StatusReserva Status { get; private set; }
    public string CodigoReserva { get; private set; } = string.Empty;
    public DateTime CriadaEm { get; private set; }

    protected Reserva() { } // EF

    public Reserva(Guid viagemId, Guid passageiroId, int numeroAssento, string codigoReserva)
    {
        if (numeroAssento <= 0)
            throw new ArgumentException("Número do assento inválido.");
        if (string.IsNullOrWhiteSpace(codigoReserva))
            throw new ArgumentException("Código de reserva é obrigatório.");

        Id = Guid.NewGuid();
        ViagemId = viagemId;
        PassageiroId = passageiroId;
        NumeroAssento = numeroAssento;
        CodigoReserva = codigoReserva;
        Status = StatusReserva.Confirmada;
        CriadaEm = DateTime.UtcNow;
    }

    public void Cancelar(DateTime dataHoraPartidaViagem)
    {
        if (dataHoraPartidaViagem - DateTime.UtcNow < TimeSpan.FromHours(2))
            throw new InvalidOperationException("Cancelamento só é permitido até 2 horas antes da partida.");

        Status = StatusReserva.Cancelada;
    }
}