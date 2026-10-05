export function PageHeader({ title, subtitle, action }) { return <div className="page-header"><div><h1>{title}</h1><p>{subtitle}</p></div>{action}</div> }
export function Loading() { return <div className="state loading"><span className="spinner" />جاري تحميل البيانات...</div> }
export function Empty({ children = 'لا توجد بيانات بعد' }) { return <div className="state empty"><span>◇</span>{children}</div> }
export function Alert({ message, type = 'error' }) { return message ? <div className={`alert ${type}`} role="alert">{message}</div> : null }
export const money = value => `${Number(value || 0).toLocaleString('ar-SY', { maximumFractionDigits: 2 })} ر.س`
export const initial = value => String(value || '؟').trim().charAt(0) || '؟'
export const formatDate = value => value ? new Intl.DateTimeFormat('ar-SY').format(new Date(`${value}T00:00:00`)) : '—'
