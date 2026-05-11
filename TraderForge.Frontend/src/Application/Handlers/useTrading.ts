import { useState, useCallback } from 'react';
import { TradingService } from '../../Infrastructure/Services/TradingService';
import type { PlaceOrderCommand } from '../DTOs/Commands/PlaceOrderCommand';
import { usePortfolio } from './usePortfolio';

const tradingService = new TradingService();

export function useTrading() {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const { refreshPortfolio } = usePortfolio();

    const placeOrder = useCallback(async (command: PlaceOrderCommand, currentPrice: number) => {
        setIsLoading(true);
        setError(null);

        try {
            const result = await tradingService.placeOrder(command, currentPrice);

            if (result.isSuccess) {
                await refreshPortfolio();
                return true;
            } else {
                setError(result.errorMessage || 'Transaction failed');
                return false;
            }
        } catch (err) {
            setError('A network error occurred during execution.');
            return false;
        } finally {
            setIsLoading(false);
        }
    }, [refreshPortfolio]);

    return {
        placeOrder,
        isLoading,
        error
    };
}
