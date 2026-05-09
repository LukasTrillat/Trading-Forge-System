import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AppLayout } from '../Layouts/AppLayout';
import { DashboardPage } from '../Pages/Dashboard/DashboardPage';
import { PortfolioPage } from '../Pages/Portfolio/PortfolioPage';

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<AppLayout />}>
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/portfolio" element={<PortfolioPage />} />
          <Route path="/bots" element={<div className="p-6 text-neutral-400">Bot Builder — coming soon</div>} />
          <Route path="/pending" element={<div className="p-6 text-neutral-400">Pending Operations — coming soon</div>} />
          <Route path="/subscription" element={<div className="p-6 text-neutral-400">Subscription — coming soon</div>} />
        </Route>

        <Route path="*" element={<Navigate to="/dashboard" replace />} />
      </Routes>
    </BrowserRouter>
  );
}
