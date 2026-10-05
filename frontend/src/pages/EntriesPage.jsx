import { useEffect, useState } from 'react'
import { api } from '../api/client'
import { EntryTable } from './AdminDashboard'
import { Alert, Loading, PageHeader } from '../components/Ui'

export default function EntriesPage({ workerMode = false }) {
  const now = new Date(); const first = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}-01`
  const [filter, setFilter] = useState({ from: first, to: now.toISOString().slice(0, 10) }); const [entries, setEntries] = useState(null); const [error, setError] = useState('')
  const load = () => api(`/work-entries?from=${filter.from}&to=${filter.to}`).then(setEntries).catch(x => setError(x.message)); useEffect(() => { load() }, [])
  async function remove(id) { if (!confirm('هل تريد حذف هذا السجل؟ لا يمكن التراجع عن العملية.')) return; setError(''); try { await api(`/work-entries/${id}`, { method: 'DELETE' }); await load() } catch (x) { setError(x.message) } }
  if (!entries) return <Loading />
  return <section className="page"><PageHeader title={workerMode ? 'سجل أعمالي' : 'سجل الإنتاج'} subtitle={workerMode ? 'راجع كل ما سجلته من إنتاج' : 'راجع إنتاج جميع العمال بالتاريخ والكمية والمستحق'} /><Alert message={error} /><div className="filter-bar"><label>من<input type="date" value={filter.from} onChange={e => setFilter({ ...filter, from: e.target.value })} /></label><label>إلى<input type="date" value={filter.to} onChange={e => setFilter({ ...filter, to: e.target.value })} /></label><button className="secondary" onClick={load}>تطبيق</button></div><div className="card"><EntryTable entries={entries} onDelete={workerMode ? null : remove} /></div></section>
}
