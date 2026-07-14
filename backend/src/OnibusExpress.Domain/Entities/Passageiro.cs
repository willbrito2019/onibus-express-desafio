namespace OnibusExpress.Domain.Entities;

public class Passageiro
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public DateOnly DataNascimento { get; private set; }

    protected Passageiro() { } // EF

    public Passageiro(string nome, string cpf, string email, DateOnly dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("E-mail inválido.");

        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf; // validação de CPF fica no Application (CpfValidator), entidade só guarda
        Email = email;
        DataNascimento = dataNascimento;
    }
}