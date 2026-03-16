import { useState, useEffect } from 'react'
import Cookies from 'js-cookie'
import toast from 'react-hot-toast'
import './SetupPage.css'

export default function SetupPage() {
  const [form, setForm] = useState({
    senderEmail: '',
    smtpPassword: '',
    receiverEmails: ''
  })

  useEffect(() => {
    const saved = {
      senderEmail: Cookies.get('senderEmail') || '',
      smtpPassword: Cookies.get('smtpPassword') || '',
      receiverEmails: Cookies.get('receiverEmails') || ''
    }
    setForm(saved)
  }, [])

  const handleChange = (e) =>
    setForm(prev => ({ ...prev, [e.target.name]: e.target.value }))

  const handleSave = (e) => {
    e.preventDefault()
    if (!form.senderEmail || !form.smtpPassword || !form.receiverEmails) {
      toast.error('All configurations are mandatory.', {
        style: { background: 'white', color: '#dc2626', border: '1px solid #fee2e2' }
      })
      return
    }

    Cookies.set('senderEmail', form.senderEmail, { expires: 365 })
    Cookies.set('smtpPassword', form.smtpPassword, { expires: 365 })
    Cookies.set('receiverEmails', form.receiverEmails, { expires: 365 })

    toast.success('Configuration Synchronized', {
      style: { background: 'white', color: '#0284c7', border: '1px solid #e0f2fe' }
    })
  }

  const handleClear = () => {
    ['senderEmail', 'smtpPassword', 'receiverEmails'].forEach(k => Cookies.remove(k))
    setForm({ senderEmail: '', smtpPassword: '', receiverEmails: '' })
    toast('Vault Cleared', { icon: '🗑️' })
  }

  return (
    <div className="setup-page fade-in">
      
      <div className="setup-grid app-shell">
        <div className="setup-info slide-up">
           <h1 className="text-gradient">System Configuration</h1>
           <p className="description">Securely configure your reporting infrastructure. All data is encrypted at rest in your browser's persistent cookie storage.</p>
           
        </div>

        <div className="setup-vault-container slide-up">
          <form onSubmit={handleSave} className="glass-panel vault-card">
            <div className="vault-header">
               <div className="vault-line"></div>
               <h3>Configuration Vault</h3>
            </div>

            <div className="form-sections">
              <div className="form-group-premium">
                <label>System Sender</label>
                <div className="input-block">
                   <div className="input-icon">@</div>
                   <input
                     type="email"
                     name="senderEmail"
                     value={form.senderEmail}
                     onChange={handleChange}
                     placeholder="identity@gmail.com"
                     required
                   />
                </div>
              </div>

              <div className="form-group-premium">
                <label>SMTP Access Key</label>
                <div className="input-block">
                   <div className="input-icon">🔑</div>
                   <input
                     type="password"
                     name="smtpPassword"
                     value={form.smtpPassword}
                     onChange={handleChange}
                     placeholder="•••• •••• •••• ••••"
                     required
                   />
                </div>
                <p className="hint">Use a dedicated 16-character App Password.</p>
              </div>

              <div className="form-group-premium">
                <label>Reporting Nodes (Recipients)</label>
                <textarea
                  name="receiverEmails"
                  value={form.receiverEmails}
                  onChange={handleChange}
                  placeholder="separate@node.com, multiple@recipients.net"
                  rows="4"
                  required
                />
              </div>
            </div>

            <div className="vault-actions">
              <button type="submit" className="premium-btn primary full-width">
                SYNC RUNTIME
              </button>
              <button type="button" onClick={handleClear} className="premium-btn secondary full-width">
                PURGE STORAGE
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  )
}
