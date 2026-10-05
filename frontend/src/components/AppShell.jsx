import { NavLink, Outlet } from 'react-router-dom'
import { useAuth } from '../auth/AuthContext'
import { initial } from './Ui'
import logo from '../assets/bittar-logo.png'

const adminLinks = [['/admin', 'نظرة عامة', '⌂'], ['/admin/workers', 'فريق العمل', '♙'], ['/admin/products', 'الأصناف والأسعار', '◆'], ['/admin/entries', 'سجل الإنتاج', '✓'], ['/admin/reports', 'التقارير المالية', '▤']]
export default function AppShell() {
  const { user, logout } = useAuth()
  const links = user.role === 'Admin' ? adminLinks : [['/worker', 'تسجيل الإنتاج', '＋'], ['/worker/history', 'سجل أعمالي', '▤']]
  return <div className="app-shell">
    <aside className="sidebar">
      <div className="brand"><img className="brand-logo" src={logo} alt="Bittar sweets" /><div><strong>Bittar sweets</strong><small>نظام إدارة الإنتاج</small></div></div>
      <nav>{links.map(([to, label, icon]) => <NavLink key={to} to={to} end={to === '/admin' || to === '/worker'}><span>{icon}</span>{label}</NavLink>)}</nav>
      <div className="sidebar-footer"><div className="avatar">{initial(user.fullName)}</div><div><strong>{user.fullName}</strong><small>{user.role === 'Admin' ? 'مدير النظام' : 'عامل'}</small></div><button className="icon-button" onClick={logout} title="تسجيل الخروج" aria-label="تسجيل الخروج">↪</button></div>
    </aside>
    <main className="main"><header className="topbar"><div className="topbar-title"><span className="topbar-dot" /><div><small>مرحبًا، {user.fullName}</small><strong>{user.role === 'Admin' ? 'مركز إدارة المعمل' : 'مساحة الإنتاج اليومية'}</strong></div></div><span className="date-chip">{new Intl.DateTimeFormat('ar-SY', { dateStyle: 'full' }).format(new Date())}</span></header><Outlet /></main>
  </div>
}
