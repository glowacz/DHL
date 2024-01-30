import { render, screen } from '@testing-library/react';
import App from '../App';
import {MemoryRouter, Route, Routes } from 'react-router-dom';
import LogInFormComponent from '../LogInFormComponent';
import OfficeWorkerComponent from '../OfficeWorkerComponent';
import CourierComponent from '../CourierComponent';

test('renders LandingPage when "/" is accessed', () => {
    render(
        <MemoryRouter>
            <Routes>
                <Route path="/" element={<App />} />
                <Route path="/login" element={<LogInFormComponent />} />
                <Route path="/OfficeWorker" element={<OfficeWorkerComponent />} />
                <Route path="/Courier" element={<CourierComponent />} />
            </Routes>
        </MemoryRouter>
    );
    const landingPageElement = screen.getByTestId('landing-page');
    expect(landingPageElement).toBeDefined();
});

test('renders LogInFormComponent when "/login" is accessed', () => {
    render(
        <MemoryRouter>
            <Routes>
                <Route path="/" element={<App/>}/>
                <Route path="/login" element={<LogInFormComponent/>}/>
                <Route path="/OfficeWorker" element={<OfficeWorkerComponent/>}/>
                <Route path="/Courier" element={<CourierComponent/>}/>
            </Routes>
        </MemoryRouter>
    );

    const loginFormElement = screen.getByTestId('login-page');
    expect(loginFormElement).toBeDefined();
});
