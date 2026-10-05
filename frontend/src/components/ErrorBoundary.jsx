import { Component } from 'react'

export default class ErrorBoundary extends Component {
  state = { error: null }

  static getDerivedStateFromError(error) { return { error } }

  render() {
    if (!this.state.error) return this.props.children
    return <div className="fatal-state" role="alert">
      <div className="fatal-icon">!</div>
      <h1>حدث خطأ غير متوقع</h1>
      <p>لم نتمكن من عرض هذه الصفحة. أعد المحاولة، وإن تكرر الخطأ تأكد أن الخادم يعمل.</p>
      <button className="primary" onClick={() => location.reload()}>إعادة تحميل الصفحة</button>
    </div>
  }
}
