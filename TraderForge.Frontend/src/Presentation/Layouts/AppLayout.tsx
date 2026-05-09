import { Outlet, NavLink } from 'react-router-dom';
import { LayoutDashboard, Briefcase, Bot, CreditCard, Clock } from 'lucide-react';
import { AlertBanner } from '../Components/Notifications/AlertBanner';

const NAV_ITEMS = [
  { to: '/dashboard', icon: LayoutDashboard, label: 'Dashboard' },
  { to: '/portfolio', icon: Briefcase, label: 'Portfolio' },
  { to: '/bots', icon: Bot, label: 'Strategies' },
  { to: '/pending', icon: Clock, label: 'Pending' },
  { to: '/subscription', icon: CreditCard, label: 'Plan' },
];

export function AppLayout() {
  return (
    <div className="flex h-screen bg-neutral-950 overflow-hidden">
      {/* Sidebar */}
      <aside className="w-56 shrink-0 flex flex-col bg-neutral-900 border-r border-neutral-800">
        <div className="px-4 py-5 border-b border-neutral-800">
          <h1 className="text-lg font-bold text-neutral-100 tracking-tight">
            Trader<span className="text-emerald-400">Forge</span>
          </h1>
        </div>

        <nav className="flex-1 py-4 flex flex-col gap-1 px-2">
          {NAV_ITEMS.map(({ to, icon: Icon, label }) => (
            <NavLink
              key={to}
              to={to}
              className={({ isActive }) =>
                `flex items-center gap-3 px-3 py-2 rounded-lg text-sm transition-colors ${
                  isActive
                    ? 'bg-emerald-500/10 text-emerald-400 font-medium'
                    : 'text-neutral-500 hover:text-neutral-200 hover:bg-neutral-800'
                }`
              }
            >
              <Icon size={16} />
              {label}
            </NavLink>
          ))}
        </nav>

      </aside>

      {/* Main content */}
      <main className="flex-1 flex flex-col min-w-0 overflow-hidden">
        <Outlet />
      </main>

      <AlertBanner />
    </div>
  );
}
