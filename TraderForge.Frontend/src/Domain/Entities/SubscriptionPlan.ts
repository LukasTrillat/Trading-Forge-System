export interface SubscriptionPlan {
  id: string;
  name: string;
  monthlyPrice: number;
  initialVirtualBalance: number;
  maxActiveStrategies: number | null;
  maxActiveAssets: number | null;
  canModifyVirtualBalance: boolean;
}
