import { useEffect, useState } from 'react'
import { api } from '../api/client'
import { Alert, Empty, initial, Loading, money, PageHeader } from '../components/Ui'

export default function ReportsPage() {
  const now = new Date(); const [period, setPeriod] = useState({ year: now.getFullYear(), month: now.getMonth() + 1 }); const [rows, setRows] = useState(null); const [error, setError] = useState('')
  const load = () => { setError(''); return api(`/dashboard/monthly-summary?year=${period.year}&month=${period.month}`).then(setRows).catch(x => { setError(x.message); setRows([]) }) }; useEffect(() => { load() }, [])
  if (!rows) return <Loading />
  return <section className="page"><PageHeader title="الحسابات الشهرية" subtitle="إجمالي إنتاج ومستحقات كل عامل لنهاية الشهر" /><Alert message={error} /><div className="filter-bar"><label>الشهر<select value={period.month} onChange={e => setPeriod({ ...period, month: Number(e.target.value) })}>{Array.from({ length: 12 }, (_, i) => <option value={i + 1} key={i}>{new Intl.DateTimeFormat('ar-SY', { month: 'long' }).format(new Date(2026, i, 1))}</option>)}</select></label><label>السنة<input type="number" min="2020" max="2100" value={period.year} onChange={e => setPeriod({ ...period, year: Number(e.target.value) })} /></label><button className="secondary" onClick={load}>عرض التقرير</button></div>{rows.length ? <><div className="cards-grid">{rows.map(x => <div className="report-card" key={x.workerId}><div className="avatar">{initial(x.workerName)}</div><div><h3>{x.workerName}</h3><p>إجمالي الكمية: {x.totalQuantity}</p></div><strong>{money(x.totalAmount)}</strong></div>)}</div><div className="summary-total"><span>إجمالي مستحقات العمال</span><strong>{money(rows.reduce((a, x) => a + x.totalAmount, 0))}</strong></div></> : <Empty>لا توجد حسابات مسجلة لهذه الفترة</Empty>}</section>
}
