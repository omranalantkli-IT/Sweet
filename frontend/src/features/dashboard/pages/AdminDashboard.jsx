import EntryTable from '../../production/components/EntryTable'
import { useEffect, useState } from 'react'
import { api } from '../../../shared/api/client'
import { Alert, Loading, money, PageHeader } from '../../../shared/ui/Ui'

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
