import { render, screen, fireEvent } from '@testing-library/react';
import '@testing-library/jest-dom';
import CourierComponent from '../CourierComponent';

describe('CourierComponent', () => {
    const handleTake = jest.fn();
    const handlePickup = jest.fn();
    const handleDeliver = jest.fn();
    const handleCannotDeliver = jest.fn();

    beforeEach(() => {
        jest.clearAllMocks();
    });

    test('renders buttons for order with status 1', () => {
        const order = { id: 1, status: 1 };
        render(<CourierComponent />);
        const takeButton = screen.getByText('Take');
        fireEvent.click(takeButton);
        expect(handleTake).toHaveBeenCalledWith(order.id);
    });

    test('renders buttons for order with status 3', () => {
        const order = { id: 2, status: 3 };
        render(<CourierComponent />);
        const pickupButton = screen.getByText('Pickup');
        const cannotDeliverButton = screen.getByText('Cannot deliver');
        fireEvent.click(pickupButton);
        fireEvent.click(cannotDeliverButton);
        expect(handlePickup).toHaveBeenCalledWith(order.id);
        expect(handleCannotDeliver).toHaveBeenCalledWith(order.id);
    });

    test('renders buttons for order with status 4', () => {
        const order = { id: 3, status: 4 };
        render(<CourierComponent />);
        const deliverButton = screen.getByText('Deliver');
        const cannotDeliverButton = screen.getByText('Cannot deliver');
        fireEvent.click(deliverButton);
        fireEvent.click(cannotDeliverButton);
        expect(handleDeliver).toHaveBeenCalledWith(order.id);
        expect(handleCannotDeliver).toHaveBeenCalledWith(order.id);
    });
});
