import { render, fireEvent, screen, waitFor } from '@testing-library/react';
import '@testing-library/jest-dom';
import axios from 'axios';
import CannotDeliverComponent from '../CannotDeliverComponent';

jest.mock('axios');

test('renders CannotDeliverComponent', () => {
    render(<CannotDeliverComponent orderId={1} />);
    const nameInput = screen.getByLabelText(/Name/i);
    const reasonInput = screen.getByLabelText(/Reason for not delivering/i);
    const submitButton = screen.getByRole('button', { name: /Submit/i });

    expect(nameInput).toBeDefined();
    expect(reasonInput).toBeDefined();
    expect(submitButton).toBeDefined();
});

test('submits form data', async () => {
    const mockedAxios = axios as jest.Mocked<typeof axios>;
    render(<CannotDeliverComponent orderId={1} />);
    const nameInput = screen.getByLabelText(/Name/i);
    const reasonInput = screen.getByLabelText(/Reason for not delivering/i);
    const submitButton = screen.getByRole('button', { name: /Submit/i });

    fireEvent.change(nameInput, { target: { value: 'John Pablo' } });
    fireEvent.change(reasonInput, { target: { value: 'Out of stock' } });
    fireEvent.click(submitButton);

    await waitFor(() => {
        expect(mockedAxios.post).toHaveBeenCalledWith(
            'https://localhost:5001/api/CannotDeliverOrder/1',
            {
                name: 'John Pablo',
                reason: 'Out of stock',
            }
        );
    });

});
