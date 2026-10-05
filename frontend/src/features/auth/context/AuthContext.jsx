import { createContext, useContext, useMemo, useState } from 'react'
import { api } from '../../../shared/api/client'

const AuthContext = createContext(null)
export function AuthProvider({ children }) {
  const [user, setUser] = useState(() => {
    try { return JSON.parse(localStorage.getItem('sweet_factory_user') || 'null') }
    catch { localStorage.removeItem('sweet_factory_user'); return null }
  })
  async function login(username, password) {
    const result = await api('/auth/login', { method: 'POST', body: JSON.stringify({ username, password }) })
    localStorage.setItem('sweet_factory_token', result.token)
    localStorage.setItem('sweet_factory_user', JSON.stringify(result.user)); setUser(result.user)
    return result.user
  }
  function logout() { localStorage.removeItem('sweet_factory_token'); localStorage.removeItem('sweet_factory_user'); setUser(null) }
  const value = useMemo(() => ({ user, login, logout }), [user])
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}
export const useAuth = () => useContext(AuthContext)
