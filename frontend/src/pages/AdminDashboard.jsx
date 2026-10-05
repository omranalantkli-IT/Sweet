import { useEffect, useState } from 'react'
import { api } from '../api/client'
import { Alert, formatDate, Loading, money, PageHeader } from '../components/Ui'

export default function AdminDashboard() {
  const [data, setData] = useState(null)
  const [error, setError] = useState('')
  useEffect(() => { api('/dashboard').then(setData).catch(x => setError(x.message)) }, [])
  if (error) return <section className="page"><Alert message={error} /></section>
  if (!data) return <Loading />
  return <section className="page"><PageHeader title="نظرة عامة على المعمل" subtitle="تابع الإنتاج والمستحقات وحركة الفريق من مكان واحد" />
    <div className="stats-grid"><Stat icon="♙" label="العمال النشطون" value={data.activeWorkers} hint="عامل جاهز للإنتاج" /><Stat icon="◇" label="إنتاج اليوم" value={Number(data.todayQuantity).toLocaleString('ar-SY')} hint="إجمالي الوحدات" /><Stat icon="✓" label="حركات الشهر" value={data.monthEntries} hint="عملية إنتاج مسجلة" /><Stat icon="ر.س" label="مستحقات الشهر" value={money(data.monthTotal)} hint="الإجمالي حتى اليوم" accent /></div>
    <div className="card"><div className="card-title"><div><h2>آخر الإنتاجات المسجلة</h2><p>آخر نشاطات العمال في المعمل</p></div></div><EntryTable entries={data.recentEntries} /></div>
  </section>
}
function Stat({ icon, label, value, hint, accent }) { return <div className={`stat-card ${accent ? 'accent' : ''}`}><span className="stat-icon">{icon}</span><div><small>{label}</small><strong>{value}</strong><p>{hint}</p></div></div> }
export function EntryTable({ entries = [], onDelete }) { return entries.length ? <div className="table-wrap"><table><thead><tr><th>العامل</th><th>نوع العمل</th><th>التاريخ</th><th>الكمية</th><th>المستحق</th>{onDelete && <th>إجراء</th>}</tr></thead><tbody>{entries.map(x => <tr key={x.id}><td data-label="العامل"><b>{x.workerName}</b></td><td data-label="نوع العمل">{x.productName}</td><td data-label="التاريخ">{formatDate(x.workDate)}</td><td data-label="الكمية">{x.quantity} {x.unitName}</td><td data-label="المستحق"><b>{money(x.totalAmount)}</b></td>{onDelete && <td data-label="إجراء"><button className="danger-link" onClick={() => onDelete(x.id)}>حذف</button></td>}</tr>)}</tbody></table></div> : <div className="state empty"><span>◇</span>لم يُسجّل أي إنتاج بعد</div> }
