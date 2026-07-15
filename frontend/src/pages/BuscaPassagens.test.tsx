import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { BrowserRouter } from 'react-router-dom';
import { vi, describe, it, expect } from 'vitest';
import BuscaPassagens from './BuscaPassagens';
import * as viagemService from '../services/viagemService';

vi.mock('../services/viagemService');

function renderComRouter() {
  return render(
    <BrowserRouter>
      <BuscaPassagens />
    </BrowserRouter>
  );
}

describe('BuscaPassagens', () => {
  it('deve buscar e listar viagens ao preencher o formulário e clicar em buscar', async () => {
    const user = userEvent.setup();

    vi.mocked(viagemService.buscarViagens).mockResolvedValue([
      {
        id: '1',
        origem: 'São Paulo',
        destino: 'Rio de Janeiro',
        dataHoraPartida: '2026-08-01T10:00:00Z',
        precoBase: 150,
        assentosDisponiveis: 40,
      },
    ]);

    renderComRouter();

    await user.type(screen.getByLabelText('Origem'), 'São Paulo');
    await user.type(screen.getByLabelText('Destino'), 'Rio de Janeiro');
    await user.click(screen.getByRole('button', { name: 'Buscar' }));

    await waitFor(() => {
      expect(screen.getByText(/São Paulo → Rio de Janeiro/)).toBeInTheDocument();
    });
  });

  it('deve exibir mensagem quando não há resultados', async () => {
    const user = userEvent.setup();
    vi.mocked(viagemService.buscarViagens).mockResolvedValue([]);

    renderComRouter();
    await user.click(screen.getByRole('button', { name: 'Buscar' }));

    await waitFor(() => {
      expect(screen.getByText(/Nenhuma viagem encontrada/)).toBeInTheDocument();
    });
  });
});