import { useState, type FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useReservaStore } from '../store/reservaStore';
import { criarReserva } from '../services/reservaService';

export default function ConfirmacaoReserva() {
  const navigate = useNavigate();
  const viagem = useReservaStore((s) => s.viagemSelecionada);
  const assento = useReservaStore((s) => s.assentoSelecionado);
  const limpar = useReservaStore((s) => s.limpar);

  const [nome, setNome] = useState('');
  const [cpf, setCpf] = useState('');
  const [email, setEmail] = useState('');
  const [dataNascimento, setDataNascimento] = useState('');
  const [erros, setErros] = useState<Record<string, string>>({});
  const [enviando, setEnviando] = useState(false);
  const [erroApi, setErroApi] = useState('');
  const [codigoReserva, setCodigoReserva] = useState<string | null>(null);

  if (!viagem || assento === null) {
    navigate('/');
    return null;
  }

  function validar(): boolean {
    const novosErros: Record<string, string> = {};

    if (!nome.trim() || nome.trim().length < 3) novosErros.nome = 'Nome completo é obrigatório.';

    const cpfDigitos = cpf.replace(/\D/g, '');
    if (cpfDigitos.length !== 11) novosErros.cpf = 'CPF deve ter 11 dígitos.';

    if (!email.includes('@') || !email.includes('.')) novosErros.email = 'E-mail inválido.';

    if (!dataNascimento) novosErros.dataNascimento = 'Data de nascimento é obrigatória.';

    setErros(novosErros);
    return Object.keys(novosErros).length === 0;
  }

  async function handleConfirmar(e: FormEvent) {
    e.preventDefault();
    setErroApi('');

    if (!validar()) return;

    setEnviando(true);
    try {
      const reserva = await criarReserva({
        nome,
        cpf,
        email,
        dataNascimento,
        viagemId: viagem!.id,
        numeroAssento: assento!,
      });
      setCodigoReserva(reserva.codigoReserva);
      limpar();
    } catch (err: any) {
      const mensagem = err?.response?.data?.mensagem || 'Erro ao criar reserva. Tente novamente.';
      setErroApi(mensagem);
    } finally {
      setEnviando(false);
    }
  }

  if (codigoReserva) {
    return (
      <div style={{ maxWidth: 600, margin: '0 auto', padding: 24 }}>
        <h1>Reserva Confirmada!</h1>
        <p>Seu código de reserva é:</p>
        <h2 data-testid="codigo-reserva">{codigoReserva}</h2>
        <button onClick={() => navigate('/consulta-reserva')}>Consultar Reserva</button>
        <button onClick={() => navigate('/')}>Nova Busca</button>
      </div>
    );
  }

  return (
    <div style={{ maxWidth: 600, margin: '0 auto', padding: 24 }}>
      <h1>Dados do Passageiro</h1>

      <div style={{ marginBottom: 16, padding: 12, background: '#f5f5f5' }}>
        <strong>Resumo da compra</strong>
        <div>{viagem.origem} → {viagem.destino}</div>
        <div>{new Date(viagem.dataHoraPartida).toLocaleString('pt-BR')}</div>
        <div>Assento: {assento}</div>
        <div>R$ {viagem.precoBase.toFixed(2)}</div>
      </div>

      <form onSubmit={handleConfirmar} data-testid="form-passageiro">
        <div>
          <label htmlFor="nome">Nome completo</label>
          <input id="nome" value={nome} onChange={(e) => setNome(e.target.value)} />
          {erros.nome && <p role="alert">{erros.nome}</p>}
        </div>

        <div>
          <label htmlFor="cpf">CPF</label>
          <input id="cpf" value={cpf} onChange={(e) => setCpf(e.target.value)} />
          {erros.cpf && <p role="alert">{erros.cpf}</p>}
        </div>

        <div>
          <label htmlFor="email">E-mail</label>
          <input id="email" value={email} onChange={(e) => setEmail(e.target.value)} />
          {erros.email && <p role="alert">{erros.email}</p>}
        </div>

        <div>
          <label htmlFor="dataNascimento">Data de nascimento</label>
          <input
            id="dataNascimento"
            type="date"
            value={dataNascimento}
            onChange={(e) => setDataNascimento(e.target.value)}
          />
          {erros.dataNascimento && <p role="alert">{erros.dataNascimento}</p>}
        </div>

        {erroApi && <p role="alert">{erroApi}</p>}

        <button type="submit" disabled={enviando}>
          {enviando ? 'Confirmando...' : 'Confirmar Reserva'}
        </button>
      </form>
    </div>
  );
}