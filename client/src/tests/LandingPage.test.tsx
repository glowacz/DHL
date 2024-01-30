import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import { MemoryRouter } from 'react-router-dom';
import LandingPage from '../LandingPage';

jest.mock('xhr2', () => ({
    ...jest.requireActual('xhr2'),
    XMLHttpRequest: jest.fn(),
}));

describe('LandingPage Navigation', () => {
    test('goToCourier navigates to /Courier', () => {
        const { container } = render(
            <MemoryRouter>
                <LandingPage />
            </MemoryRouter>
        );
        const courierButton = screen.getByText('Courier');
        fireEvent.click(courierButton);
        expect(container.innerHTML).toContain('Courier Component');
    });

    test('goToOfficeWorker navigates to /OfficeWorker', () => {
        const { container } = render(
            <MemoryRouter>
                <LandingPage />
            </MemoryRouter>
        );
        const officeWorkerButton = screen.getByText('Office Worker');
        fireEvent.click(officeWorkerButton);
        expect(container.innerHTML).toContain('Office Worker Component'); 
    });
});
