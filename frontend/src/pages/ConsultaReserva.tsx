import { useState, type FormEvent } from 'react';
import { consultarReserva, cancelarReserva } from '../services/reservaService';
import type { Reserva } from '../types';

export default function ConsultaReserva() {
  const [codigo, setCodigo] = useState('');
  const [reserva, setReserva] = useState<Reserva | null>(null);
  const [erro, setErro] = useState('');
  const [carregando, setCarregando] = useState(false);
  const [mensagemCancelamento, setMensagemCancelamento] = useState('');

  async function handleConsultar(e: FormEvent) {
    e.preventDefault();
    setErro('');
    setMensagemCancelamento('');
    setCarregando(true);

    try {
      const resultado = await consultarReserva(codigo);
      setReserva(resultado);
    } catch {
      setErro('Reserva não encontrada.');
      setReserva(null);
    } finally {
      setCarregando(false);
    }
  }

  async function handleCancelar() {
    if (!reserva) return;

    try {
      await cancelarReserva(reserva.codigoReserva);
      setMensagemCancelamento('Reserva cancelada com sucesso.');
      setReserva({ ...reserva, status: 'Cancelada' });
    } catch (err: any) {
      const mensagem = err?.response?.data?.mensagem || 'Erro ao cancelar reserva.';
      setErro(mensagem);
    }
  }

  return (
    <div style={{ maxWidth: 600, margin: '0 auto', padding: 24 }}>
      <h1>Consulta de Reserva</h1>

      <form onSubmit={handleConsultar}>
        <label htmlFor="codigo">Código da reserva</label>
        <input id="codigo" value={codigo} onChange={(e) => setCodigo(e.target.value)} />
        <button type="submit" disabled={carregando}>Consultar</button>
      </form>

      {erro && <p role="alert">{erro}</p>}
      {mensagemCancelamento && <p>{mensagemCancelamento}</p>}

      {reserva && (
        <div style={{ marginTop: 16, padding: 12, background: '#f5f5f5' }}>
          <div>Código: {reserva.codigoReserva}</div>
          <div>Status: {reserva.status}</div>
          <div>Passageiro: {reserva.passageiroNome}</div>
          <div>Assento: {reserva.numeroAssento}</div>
          <div>Partida: {new Date(reserva.dataHoraPartidaViagem).toLocaleString('pt-BR')}</div>

          {reserva.status === 'Confirmada' && (
            <button onClick={handleCancelar}>Cancelar Reserva</button>
          )}
        </div>
      )}
    </div>
  );
}