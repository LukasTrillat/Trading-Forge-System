import { Outlet } from 'react-router-dom';
import { motion } from 'framer-motion';

export function AuthLayout() {
  return (
    <div className="min-h-screen bg-neutral-950 flex items-center justify-center p-4">
      <div className="w-full max-w-sm">
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.3 }}
        >
          <div className="text-center mb-8">
            <h1 className="text-2xl font-bold text-neutral-100 tracking-tight">
              Trader<span className="text-emerald-400">Forge</span>
            </h1>
            <p className="text-sm text-neutral-500 mt-1">Trading Simulation Platform</p>
          </div>
          <Outlet />
        </motion.div>
      </div>
    </div>
  );
}
