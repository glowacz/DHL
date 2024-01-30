import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css'
import OfficeWorkerComponent from './OfficeWorkerComponent';
import CourierComponent from './CourierComponent';
import LandingPage from './LandingPage';
import LogInFormComponent from './LogInFormComponent';
import CannotDeliverComponent from './CannotDeliverComponent';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" Component={LandingPage} />
        <Route path="/login" Component={LogInFormComponent} />
        <Route path="/OfficeWorker" Component={OfficeWorkerComponent} />
        <Route path="/Courier" Component={CourierComponent} />
        {/* <Route path="/CannotDeliver" Component={CannotDeliverComponent} /> */}
      </Routes>
    </Router>
  )
}

export default App
