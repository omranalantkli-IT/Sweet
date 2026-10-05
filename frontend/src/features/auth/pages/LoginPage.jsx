import { useState } from 'react'
import { Navigate, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { Alert } from '../../../shared/ui/Ui'
import logo from '../../../assets/bittar-logo.png'

export default function LoginPage() {
  const { user, login } = useAuth(); const navigate = useNavigate()
  const [form, setForm] = useState({ username: '', password: '' }); const [error, setError] = useState(''); const [busy, setBusy] = useState(false)
  if (user) return <Navigate to={user.role === 'Admin' ? '/admin' : '/worker'} replace />
  async function submit(e) { e.preventDefault(); setBusy(true); setError(''); try { const u = await login(form.username, form.password); navigate(u.role === 'Admin' ? '/admin' : '/worker') } catch (x) { setError(x.message) } finally { setBusy(false) } }
  return <div className="login-page">
    <form className="login-card" onSubmit={submit}>
      <img className="login-logo" src={logo} alt="Bittar sweets" />
      <Alert message={error} />
      <label>اسم المستخدم<input autoFocus autoComplete="username" value={form.username} onChange={e => setForm({ ...form, username: e.target.value })} placeholder="أدخل اسم المستخدم" required /></label>
      <label>كلمة المرور<input type="password" autoComplete="current-password" value={form.password} onChange={e => setForm({ ...form, password: e.target.value })} placeholder="أدخل كلمة المرور" required /></label>
      <button className="primary wide" disabled={busy}>{busy ? 'جاري تسجيل الدخول...' : 'تسجيل الدخول'}</button>
    </form>
  </div>
}
