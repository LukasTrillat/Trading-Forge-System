import { Shield, MessageSquare } from 'lucide-react';
import { useState, useEffect } from 'react';
import { AdminPlanService } from '../../../Application/Services/AdminPlanService';
import { AdminSupportService } from '../../../Application/Services/AdminSupportService';
import { SubscriptionPlan } from '../../../Domain/Entities/SubscriptionPlan';
import { SupportMessage } from '../../../Domain/Entities/SupportMessage';
import { AdminPlanTable } from './Components/AdminPlanTable';
import { AdminSupportTable } from './Components/AdminSupportTable';

export function AdminPage() {
  const [plans, setPlans] = useState<SubscriptionPlan[]>([]);
  const [messages, setMessages] = useState<SupportMessage[]>([]);

  useEffect(() => {
    async function loadData() {
      const planService = new AdminPlanService();
      const supportService = new AdminSupportService();
      
      try {
        const fetchedPlans = await planService.getAllPlans();
        setPlans(fetchedPlans);
      } catch (error) {
        console.error("Failed to load plans:", error);
      }

      try {
        const fetchedMessages = await supportService.getAllMessages();
        setMessages(fetchedMessages);
      } catch (error) {
        console.error("Failed to load support messages:", error);
      }
    }
    
    loadData();
  }, []);

  return (
    <div className="p-6 h-full overflow-y-auto w-full">
      <div className="flex items-center gap-3 mb-6">
        <div className="p-2 bg-red-500/10 rounded-lg text-red-400">
          <Shield size={24} />
        </div>
        <h1 className="text-2xl font-bold text-neutral-100">Administrator Console</h1>
      </div>

      <div className="space-y-6">
        {/* Subscription Plans Section */}
        <section className="bg-neutral-900 border border-neutral-800 rounded-xl p-6">
          <div className="flex justify-between items-center mb-4">
            <h2 className="text-lg font-semibold text-neutral-200">Subscription Plans</h2>
            <button className="px-4 py-2 bg-red-500/10 text-red-400 hover:bg-red-500/20 rounded-lg text-sm font-medium transition-colors">
              Create Plan
            </button>
          </div>
          
          <AdminPlanTable plansToDisplay={plans} />
        </section>

        {/* Support Messages Section */}
        <section className="bg-neutral-900 border border-neutral-800 rounded-xl p-6">
          <div className="flex justify-between items-center mb-4">
            <div className="flex items-center gap-2">
              <MessageSquare size={18} className="text-neutral-400" />
              <h2 className="text-lg font-semibold text-neutral-200">Support Messages</h2>
            </div>
            <button className="px-4 py-2 bg-neutral-800 hover:bg-neutral-700 text-neutral-200 rounded-lg text-sm font-medium transition-colors">
              Filter Active
            </button>
          </div>
          
          <AdminSupportTable messagesToDisplay={messages} />
        </section>
      </div>
    </div>
  );
}
