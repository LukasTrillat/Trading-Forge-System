import { SubscriptionPlan } from '../../Domain/Entities/SubscriptionPlan';

export class AdminPlanService {
  public getAllPlans(): SubscriptionPlan[] {
    const plans: SubscriptionPlan[] = [
      { 
        id: '1', 
        name: 'Basic', 
        monthlyPrice: 9.99, 
        initialVirtualBalance: 10000.00, 
        maxActiveStrategies: 2, 
        maxActiveAssets: 5, 
        canModifyVirtualBalance: false 
      },
      { 
        id: '2', 
        name: 'Pro', 
        monthlyPrice: 29.99, 
        initialVirtualBalance: 50000.00, 
        maxActiveStrategies: 10, 
        maxActiveAssets: 20, 
        canModifyVirtualBalance: false 
      },
      { 
        id: '3', 
        name: 'Enterprise', 
        monthlyPrice: 99.99, 
        initialVirtualBalance: 1000000.00, 
        maxActiveStrategies: null, 
        maxActiveAssets: null, 
        canModifyVirtualBalance: true 
      }
    ];

    return plans;
  }
}
