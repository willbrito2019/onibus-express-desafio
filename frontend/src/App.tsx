import { BrowserRouter, Routes, Route } from 'react-router-dom';
import BuscaPassagens from './pages/BuscaPassagens';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<BuscaPassagens />} />
        {}
      </Routes>
    </BrowserRouter>
  );
}

export default App;