import { BrowserRouter, Routes, Route } from 'react-router-dom';
import BuscaPassagens from './pages/BuscaPassagens';
import SelecaoAssento from './pages/SelecaoAssento';
import ConfirmacaoReserva from './pages/ConfirmacaoReserva';
import ConsultaReserva from './pages/ConsultaReserva';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<BuscaPassagens />} />
        <Route path="/selecao-assento" element={<SelecaoAssento />} />   
        <Route path="/confirmacao" element={<ConfirmacaoReserva />} />     
        <Route path="/consulta-reserva" element={<ConsultaReserva />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;