// src/Infrastructure/Services/PortfolioService.ts
import type { Portfolio, SimulationSnapshot } from '../../Domain/Entities/Portfolio';
import { Result } from '../../Application/Common/Result';
import { httpClient } from '../Http/httpClient';

export class PortfolioService {
  async getPortfolio(_traderId: string): Promise<Result<Portfolio>> {
    try {
      const { data } = await httpClient.get<Portfolio>('/api/portfolio/active');
      return Result.ok(data);
    } catch (error) {
      return Result.fail('Failed to fetch real portfolio data.');
    }
  }

  async getSimulationHistory(_traderId: string): Promise<Result<SimulationSnapshot[]>> {
    try {
      const { data } = await httpClient.get<SimulationSnapshot[]>('/api/portfolio/simulation-history');
      return Result.ok(data);
    } catch (error) {
      return Result.ok([]); // Return empty list instead of failing if history is empty
    }
  }

  async resetSimulation(_traderId: string): Promise<Result<void>> {
    try {
      await httpClient.post('/api/portfolio/reset');
      return Result.ok(undefined);
    } catch (error) {
      return Result.fail('Failed to reset simulation.');
    }
  }
}
