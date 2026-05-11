import type { Portfolio, SimulationSnapshot } from '../../Domain/Entities/Portfolio';
import { Result } from '../../Application/Common/Result';
import { httpClient } from '../Http/httpClient';

export class PortfolioService {
  
  // Gets the active portfolio using the Bearer token injected by httpClient
  async getPortfolio(traderId: string): Promise<Result<Portfolio>> {
    try {
      const response = await httpClient.get<Portfolio>('/api/portfolio/active');
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Failed to fetch portfolio data');
    }
  }

  // Gets the user's historical snapshots
  async getSimulationHistory(traderId: string): Promise<Result<SimulationSnapshot[]>> {
    try {
      const response = await httpClient.get<SimulationSnapshot[]>('/api/portfolio/simulation-history');
      return Result.ok(response.data);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Failed to fetch simulation history');
    }
  }

  // Calls the backend to reset the account balance and wipe positions
  async resetSimulation(traderId: string): Promise<Result<void>> {
    try {
      await httpClient.post('/api/portfolio/reset');
      return Result.ok(undefined);
    } catch (error: any) {
      return Result.fail(error.response?.data?.message || 'Failed to reset simulation');
    }
  }
}
