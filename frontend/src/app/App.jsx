import { Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from '../features/auth/components/ProtectedRoute'
import AppShell from '../shared/layout/AppShell'
import LoginPage from '../features/auth/pages/LoginPage'
import AdminDashboard from '../features/dashboard/pages/AdminDashboard'
import WorkersPage from '../features/workers/pages/WorkersPage'
import ProductsPage from '../features/products/pages/ProductsPage'
import EntriesPage from '../features/production/pages/EntriesPage'
import ReportsPage from '../features/reports/pages/ReportsPage'
import WorkerDashboard from '../features/production/pages/WorkerDashboard'
import ErrorBoundary from '../shared/components/ErrorBoundary'

export default function App() { return <ErrorBoundary><Routes>
  <Route path="/login" element={<LoginPage />} />
  <Route element={<ProtectedRoute role="Admin" />}><Route element={<AppShell />}>
    <Route path="/admin" element={<AdminDashboard />} /><Route path="/admin/workers" element={<WorkersPage />} />
    <Route path="/admin/products" element={<ProductsPage />} /><Route path="/admin/entries" element={<EntriesPage />} />
    <Route path="/admin/reports" element={<ReportsPage />} />
  </Route></Route>
  <Route element={<ProtectedRoute role="Worker" />}><Route element={<AppShell />}>
    <Route path="/worker" element={<WorkerDashboard />} /><Route path="/worker/history" element={<EntriesPage workerMode />} />
  </Route></Route>
  <Route path="*" element={<Navigate to="/login" replace />} />
  </Routes></ErrorBoundary> }
