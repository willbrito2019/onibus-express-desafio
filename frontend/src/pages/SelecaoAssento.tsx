import { useNavigate } from 'react-router-dom';
import { useEffect } from 'react';
import { useReservaStore } from '../store/reservaStore';

export default function SelecaoAssento() {
  const navigate = useNavigate();
  const viagem = useReservaStore((s) => s.viagemSelecionada);
  const assentoSelecionado = useReservaStore((s) => s.assentoSelecionado);
  const setAssentoSelecionado = useReservaStore((s) => s.setAssentoSelecionado);

  useEffect(() => {
    if (!viagem) navigate('/');
  }, [viagem, navigate]);

  if (!viagem) return null;

  function handleSelecionarAssento(numero: number) {
    if (viagem!.assentosOcupados.includes(numero)) return;
    setAssentoSelecionado(numero);
  }

  function getStatusAssento(numero: number): 'ocupado' | 'selecionado' | 'livre' {
    if (viagem!.assentosOcupados.includes(numero)) return 'ocupado';
    if (numero === assentoSelecionado) return 'selecionado';
    return 'livre';
  }

  return (
    <div style={{ maxWidth: 600, margin: '0 auto', padding: 24 }}>
      <h1>Seleção de Assento</h1>

      <div style={{ marginBottom: 16 }}>
        <strong>{viagem.origem} → {viagem.destino}</strong>
        <div>{new Date(viagem.dataHoraPartida).toLocaleString('pt-BR')}</div>
        <div>R$ {viagem.precoBase.toFixed(2)}</div>
      </div>

      <div
        data-testid="mapa-assentos"
        style={{ display: 'grid', gridTemplateColumns: 'repeat(5, 1fr)', gap: 8, marginBottom: 16 }}
      >
        {Array.from({ length: viagem.totalAssentos }, (_, i) => i + 1).map((numero) => {
          const status = getStatusAssento(numero);
          return (
            <button
              key={numero}
              data-testid={`assento-${numero}`}
              disabled={status === 'ocupado'}
              onClick={() => handleSelecionarAssento(numero)}
              style={{
                padding: 8,
                background: status === 'ocupado' ? '#e53935' : status === 'selecionado' ? '#4caf50' : '#fff',
                color: status === 'ocupado' ? '#fff' : '#000',
                fontWeight: 600,
                cursor: status === 'ocupado' ? 'not-allowed' : 'pointer',
                border: '1px solid #999',
              }}
            >
              {numero}
            </button>
          );
        })}
      </div>

      <button disabled={assentoSelecionado === null} onClick={() => navigate('/confirmacao')}>
        Prosseguir
      </button>
    </div>
  );
}