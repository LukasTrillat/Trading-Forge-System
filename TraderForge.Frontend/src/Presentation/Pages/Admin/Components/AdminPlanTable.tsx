import { SubscriptionPlan } from '../../../../Domain/Entities/SubscriptionPlan';

interface AdminPlanTableProps {
  plansToDisplay: SubscriptionPlan[];
}

export function AdminPlanTable(props: AdminPlanTableProps) {
  const plans = props.plansToDisplay;

  function formatLimits(strategiesLimit: number | null, assetsLimit: number | null): string {
    const strategiesText = strategiesLimit === null ? 'No limit' : strategiesLimit.toString();
    const assetsText = assetsLimit === null ? 'No limit' : assetsLimit.toString();

    return strategiesText + ' / ' + assetsText;
  }

  return (
    <div className="overflow-x-auto border border-neutral-800 rounded-lg">
      <table className="w-full text-left text-sm text-neutral-400">
        <thead className="border-b border-neutral-800 text-neutral-400">
          <tr>
            <th className="px-4 py-3 font-medium rounded-tl-lg">Plan Name</th>
            <th className="px-4 py-3 font-medium">Monthly Price</th>
            <th className="px-4 py-3 font-medium">Initial Balance</th>
            <th className="px-4 py-3 font-medium">Limits (Bot / Asset)</th>
            <th className="px-4 py-3 font-medium rounded-tr-lg text-right">Actions</th>
          </tr>
        </thead>
        <tbody className="divide-y divide-neutral-800">
          {plans.map(function(plan: SubscriptionPlan) {
            return (
              <tr key={plan.id} className="hover:bg-neutral-800/30 transition-colors">
                <td className="px-4 py-3 font-medium text-neutral-200">{plan.name}</td>
                <td className="px-4 py-3">${plan.monthlyPrice}</td>
                <td className="px-4 py-3">${plan.initialVirtualBalance.toLocaleString()}</td>
                <td className="px-4 py-3">
                  {formatLimits(plan.maxActiveStrategies, plan.maxActiveAssets)}
                </td>
                <td className="px-4 py-3 text-right space-x-3">
                  <button className="text-red-400 hover:text-red-300 transition-colors">Edit</button>
                  <button className="text-neutral-400 hover:text-neutral-300 transition-colors">Delete</button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
