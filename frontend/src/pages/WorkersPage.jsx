import { useEffect, useState } from 'react'
import { api } from '../api/client'
import { Alert, Empty, initial, Loading, PageHeader } from '../components/Ui'

export default function WorkersPage() {
  const [workers, setWorkers] = useState(null); const [show, setShow] = useState(false); const [error, setError] = useState('')
  const load = () => api('/workers').then(setWorkers).catch(x => { setError(x.message); setWorkers([]) })
  useEffect(() => { load() }, [])
  async function toggle(w) { setError(''); try { await api(`/workers/${w.id}`, { method: 'PUT', body: JSON.stringify({ fullName: w.fullName, isActive: !w.isActive, newPassword: null }) }); await load() } catch (x) { setError(x.message) } }
  if (!workers) return <Loading />
  return <section className="page"><PageHeader title="إدارة العمال" subtitle="أنشئ حسابات العمال وتابع حالة كل حساب" action={<button className="primary" onClick={() => setShow(true)}>+ إضافة عامل</button>} /><Alert message={error} />
    <div className="cards-grid">{workers.map(w => <div className="person-card" key={w.id}><div className="avatar large-avatar">{initial(w.fullName)}</div><div className="person-main"><h3>{w.fullName || 'عامل بلا اسم'}</h3><p>@{w.username}</p></div><span className={`badge ${w.isActive ? 'success' : 'muted'}`}>{w.isActive ? 'نشط' : 'موقوف'}</span><button className="secondary" onClick={() => toggle(w)}>{w.isActive ? 'إيقاف الحساب' : 'تفعيل الحساب'}</button></div>)}</div>{!workers.length && <Empty>لم تتم إضافة أي عامل بعد</Empty>}
    {show && <WorkerModal close={() => setShow(false)} saved={() => { setShow(false); load() }} setError={setError} />}</section>
}
function WorkerModal({ close, saved, setError }) { const [f, setF] = useState({ fullName: '', username: '', password: '' }); const [busy, setBusy] = useState(false); async function submit(e) { e.preventDefault(); setBusy(true); try { await api('/workers', { method: 'POST', body: JSON.stringify(f) }); saved() } catch (x) { setError(x.message); close() } finally { setBusy(false) } } return <div className="modal-backdrop" onMouseDown={close}><form className="modal" onSubmit={submit} onMouseDown={e => e.stopPropagation()}><div className="modal-head"><div><small>حساب جديد</small><h2>إضافة عامل</h2></div><button type="button" onClick={close} aria-label="إغلاق">×</button></div><label>اسم العامل<input value={f.fullName} onChange={e => setF({ ...f, fullName: e.target.value })} placeholder="الاسم الكامل" required /></label><label>اسم المستخدم<input value={f.username} onChange={e => setF({ ...f, username: e.target.value })} placeholder="أحرف إنجليزية بدون مسافات" minLength="3" required /></label><label>كلمة المرور<input type="password" minLength="8" value={f.password} onChange={e => setF({ ...f, password: e.target.value })} placeholder="8 أحرف على الأقل" required /></label><button className="primary wide" disabled={busy}>{busy ? 'جاري الإنشاء...' : 'إنشاء الحساب'}</button></form></div> }
