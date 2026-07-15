import { useState } from 'react';
import type { FormEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { buscarViagens } from '../services/viagemService';
import { obterDetalhesViagem } from '../services/viagemService';
import { useReservaStore } from '../store/reservaStore';
import type { Viagem } from '../types';

export default function BuscaPassagens() {
  const [origem, setOrigem] = useState('');
  const [destino, setDestino] = useState('');
  const [data, setData] = useState('');
  const [viagens, setViagens] = useState<Viagem[]>([]);
  const [carregando, setCarregando] = useState(false);
  const [buscou, setBuscou] = useState(false);
  const [erro, setErro] = useState('');

  const navigate = useNavigate();
  const setViagemSelecionada = useReservaStore((s) => s.setViagemSelecionada);

  async function handleBuscar(e: FormEvent) {
    e.preventDefault();
    setCarregando(true);
    setErro('');
    setBuscou(true);

    try {
      const resultado = await buscarViagens(origem || undefined, destino || undefined, data || undefined);
      setViagens(resultado);
    } catch {
      setErro('Erro ao buscar viagens. Tente novamente.');
    } finally {
      setCarregando(false);
    }
  }

  async function handleSelecionarViagem(viagemId: string) {
    try {
      const detalhes = await obterDetalhesViagem(viagemId);
      setViagemSelecionada(detalhes);
      navigate('/selecao-assento');
    } catch {
      setErro('Erro ao carregar detalhes da viagem.');
    }
  }

  return (
    <div style={{ maxWidth: 600, margin: '0 auto', padding: 24 }}>
      <h1>Busca de Passagens</h1>

      <form onSubmit={handleBuscar} data-testid="form-busca">
        <div>
          <label htmlFor="origem">Origem</label>
          <input id="origem" value={origem} onChange={(e) => setOrigem(e.target.value)} />
        </div>
        <div>
          <label htmlFor="destino">Destino</label>
          <input id="destino" value={destino} onChange={(e) => setDestino(e.target.value)} />
        </div>
        <div>
          <label htmlFor="data">Data de ida</label>
          <input id="data" type="date" value={data} onChange={(e) => setData(e.target.value)} />
        </div>
        <button type="submit">Buscar</button>
      </form>

      {carregando && <p>Carregando...</p>}
      {erro && <p role="alert">{erro}</p>}

      {!carregando && buscou && viagens.length === 0 && !erro && (
        <p>Nenhuma viagem encontrada para os critérios informados.</p>
      )}

      <ul>
        {viagens.map((v) => (
          <li key={v.id} data-testid="viagem-item">
            <strong>{v.origem} → {v.destino}</strong>
            <div>{new Date(v.dataHoraPartida).toLocaleString('pt-BR')}</div>
            <div>R$ {v.precoBase.toFixed(2)} — {v.assentosDisponiveis} vagas</div>
            <button onClick={() => handleSelecionarViagem(v.id)}>Selecionar</button>
          </li>
        ))}
      </ul>
    </div>
  );
}