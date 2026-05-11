import { SubscriptionPlan } from '../../Domain/Entities/SubscriptionPlan';
import { httpClient } from '../../Infrastructure/Http/httpClient';

export class AdminPlanService {
  public async getAllPlans(): Promise<SubscriptionPlan[]> {
    const response = await httpClient.get<SubscriptionPlan[]>('/api/admin/plans');
    return response.data;
  }
}
