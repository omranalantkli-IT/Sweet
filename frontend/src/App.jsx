import { Navigate, Route, Routes } from 'react-router-dom'
import ProtectedRoute from './components/ProtectedRoute'
import AppShell from './components/AppShell'
import LoginPage from './pages/LoginPage'
import AdminDashboard from './pages/AdminDashboard'
import WorkersPage from './pages/WorkersPage'
import ProductsPage from './pages/ProductsPage'
import EntriesPage from './pages/EntriesPage'
import ReportsPage from './pages/ReportsPage'
import WorkerDashboard from './pages/WorkerDashboard'
import ErrorBoundary from './components/ErrorBoundary'

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
