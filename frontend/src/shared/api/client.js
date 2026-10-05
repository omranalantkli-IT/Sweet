const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5184/api'

export async function api(path, options = {}) {
  const token = localStorage.getItem('sweet_factory_token')
  let response
  try {
    response = await fetch(`${API_URL}${path}`, {
      ...options,
      headers: { 'Content-Type': 'application/json', ...(token ? { Authorization: `Bearer ${token}` } : {}), ...options.headers },
    })
  } catch {
    throw new Error('تعذّر الاتصال بالخادم. تأكد أن الباك إند يعمل ثم حاول مجددًا.')
  }
  if (response.status === 401) {
    localStorage.removeItem('sweet_factory_token'); localStorage.removeItem('sweet_factory_user')
    if (!location.pathname.includes('/login')) location.assign('/login')
  }
  if (!response.ok) {
    const error = await response.json().catch(() => ({}))
    throw new Error(error.message || error.title || 'تعذر تنفيذ الطلب')
  }
  return response.status === 204 ? null : response.json()
}
