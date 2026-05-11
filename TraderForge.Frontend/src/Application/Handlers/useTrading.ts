import { useState, useCallback } from 'react';
import { TradingService } from '../../Infrastructure/Services/TradingService';
import type { PlaceOrderCommand } from '../DTOs/Commands/PlaceOrderCommand';
import { usePortfolio } from './usePortfolio';

const tradingService = new TradingService();

export function useTrading() {
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const { refreshPortfolio } = usePortfolio();

    const placeOrder = useCallback(async (command: PlaceOrderCommand) => {
        setIsLoading(true);
        setError(null);

        try {
            const result = await tradingService.placeOrder(command);

            if (result.isSuccess) {
                // Refresh balance immediately after a successful trade
                await refreshPortfolio();
                return true;
            } else {
                setError(result.error || 'Transaction failed');
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