import { useState, useEffect } from 'react'
import Cookies from 'js-cookie'
import toast from 'react-hot-toast'
import api from '../api/api'
import './TaskPage.css'
import { buildReportHtml } from '../services/reportService'

export default function TaskPage() {
  const [step, setStep] = useState(1) // 1: Builder, 2: Preview
  const [tasks, setTasks] = useState([])
  const [loading, setLoading] = useState(false)
  const [editedHtml, setEditedHtml] = useState('')

  // Load credentials from cookies
  const credentials = {
    senderEmail: Cookies.get('senderEmail'),
    smtpPassword: Cookies.get('smtpPassword'),
    receiverEmails: Cookies.get('receiverEmails')?.split(',').map(e => e.trim()) || []
  }

  useEffect(() => {
    if (tasks.length === 0) addTask()
  }, [])

  const addTask = () => {
    setTasks([...tasks, {
      taskNo: '',
      taskName: '',
      taskDetail: '',
      status: 'Complete',
      startTime: '',
      endTime: '',
      hours: '00:00',
      includeBreak: false
    }])
  }

  const removeTask = (index) => {
    setTasks(tasks.filter((_, i) => i !== index))
  }

  const calculateHours = (start, end) => {
    if (!start || !end) return '00:00'
    const [sH, sM] = start.split(':').map(Number)
    const [eH, eM] = end.split(':').map(Number)
    let diff = (eH * 60 + eM) - (sH * 60 + sM)
    if (diff < 0) diff += 1440
    return `${Math.floor(diff / 60).toString().padStart(2, '0')}:${(diff % 60).toString().padStart(2, '0')}`
  }

  const handleTaskChange = (index, field, value) => {
    const newTasks = [...tasks]
    newTasks[index][field] = value
    if (field === 'startTime' || field === 'endTime') {
      newTasks[index].hours = calculateHours(newTasks[index].startTime, newTasks[index].endTime)
    }
    setTasks(newTasks)
  }

  const handleStepChange = (newStep) => {
    if (newStep === 2) {
      const initialHtml = buildReportHtml(tasks.filter(t => t.taskNo))
      setEditedHtml(initialHtml)
    }
    setStep(newStep)
  }

  const handleCopyToClipboard = () => {
    const htmlToCopy = editedHtml || buildReportHtml(tasks.filter(t => t.taskNo))
    navigator.clipboard.writeText(htmlToCopy).then(() => {
      toast.success('HTML Report Copied to Clipboard!', {
        style: { background: 'white', color: '#0284c7', border: '1px solid #e0f2fe' }
      })
    })
  }

  const handleMailto = () => {
    const subject = `Daily Work Report of ${new Date().toLocaleDateString('en-GB')}`
    const body = "Please paste the copied HTML report table here."
    const mailto = `mailto:${credentials.receiverEmails.join(',')}?subject=${encodeURIComponent(subject)}&body=${encodeURIComponent(body)}`
    window.location.href = mailto
  }

  const handleSendReport = async () => {
    const validTasks = tasks.filter(t => t.taskNo)
    if (validTasks.length === 0) {
      toast.error('No valid tasks identified.')
      return
    }

    setLoading(true)
    console.log(`[Frontend] Sending report to ${api.defaults.baseURL}/api/task/send-report`)
    console.log('[Frontend] Payload:', { ...credentials, tasks: validTasks })
    try {
      const response = await api.post('/api/task/send-report', {
        ...credentials,
        tasks: validTasks,
        htmlBody: editedHtml
      })
      console.log('[Frontend] Dispatch SUCCESS:', response.data)
      toast.success('Enterprise Report Dispatched!')
      setStep(1)
      setTasks([]) // Clear for next rotation
    } catch (err) {
      toast.error(err.response?.data?.message || 'Dispatch Failure (Check SMTP/API)')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="task-page fade-in app-shell">
      
      <header className="dashboard-header">
         <div>
            <h1 className="text-gradient">Report Builder</h1>
            <p className="subtitle">Compile your daily achievements into a professional dispatch.</p>
         </div>
         <button className="premium-btn primary" onClick={addTask}>
            <span>+</span> NEW RECORD
         </button>
      </header>

      {step === 1 && (
        <div className="builder-grid slide-up">
           <div className="task-scroll-area">
              {tasks.map((task, index) => (
                <div key={index} className="task-record-layer glass-panel">
                  <div className="record-status-line" style={{ background: task.taskNo ? 'var(--primary)' : 'var(--surface-border)' }}></div>
                  
                  <div className="record-controls">
                    <div className="input-row main">
                       <div className="field">
                          <label className="mono">ID</label>
                          <input 
                            className="bg-less"
                            placeholder="####" 
                            value={task.taskNo} 
                            onChange={(e) => handleTaskChange(index, 'taskNo', e.target.value)} 
                          />
                       </div>
                       <div className="field grow">
                          <label className="mono">Achievment Title</label>
                          <input 
                            className="bg-less"
                            placeholder="Ex: Database Schema Finalization" 
                            value={task.taskName} 
                            onChange={(e) => handleTaskChange(index, 'taskName', e.target.value)} 
                            disabled={!task.taskNo}
                          />
                       </div>
                       <button className="action-circle remove" onClick={() => removeTask(index)}>×</button>
                    </div>

                    {task.taskNo && (
                      <div className="record-expansion fade-in">
                        <textarea 
                          placeholder="Provide deep context on the implementation or challenge..." 
                          value={task.taskDetail} 
                          onChange={(e) => handleTaskChange(index, 'taskDetail', e.target.value)} 
                          rows="2"
                        />
                        
                        <div className="input-row meta">
                          <div className="field mini">
                            <label>Timeline Status</label>
                            <select value={task.status} onChange={(e) => handleTaskChange(index, 'status', e.target.value)}>
                              <option value="Complete">Complete ✅</option>
                              <option value="In Progress">Running ⏳</option>
                            </select>
                          </div>
                          <div className="field mini">
                            <label>Start</label>
                            <input type="time" value={task.startTime} onChange={(e) => handleTaskChange(index, 'startTime', e.target.value)} />
                          </div>
                          <div className="field mini">
                            <label>End</label>
                            <input type="time" value={task.endTime} onChange={(e) => handleTaskChange(index, 'endTime', e.target.value)} />
                          </div>
                          <div className="field mini checkbox-field">
                            <label>Break</label>
                            <input 
                              type="checkbox" 
                              checked={task.includeBreak} 
                              onChange={(e) => handleTaskChange(index, 'includeBreak', e.target.checked)} 
                            />
                          </div>
                          <div className="duration-pill mono">{task.hours}</div>
                        </div>
                      </div>
                    )}
                  </div>
                </div>
              ))}
           </div>
           
            <div className="builder-actions">
               <button 
                 className="premium-btn primary large glow" 
                 onClick={() => handleStepChange(2)}
                 disabled={tasks.filter(t => t.taskNo).length === 0}
               >
                 COMPILE FINAL DRAFT <span>→</span>
               </button>
            </div>
        </div>
      )}

      {step === 2 && (
        <div className="preview-container slide-up">
           <div className="glass-panel preview-window">
              <div className="window-decor">
                 <span className="dot red"></span>
                 <span className="dot yellow"></span>
                 <span className="dot green"></span>
                 <span className="window-title">report_draft_v1.html</span>
              </div>
                            <div className="html-viewer" 
                    contentEditable 
                    suppressContentEditableWarning={true}
                    onBlur={(e) => setEditedHtml(e.currentTarget.innerHTML)}
                    dangerouslySetInnerHTML={{ __html: editedHtml }}
               />
           </div>

           <div className="dispatch-controls">
              <button className="premium-btn secondary" onClick={() => handleStepChange(1)}>
                 ← BACK TO BUILDER
               </button>
              <button className="premium-btn secondary" onClick={handleCopyToClipboard}>
                COPY HTML TO CLIPBOARD
              </button>
              <button 
                className={`premium-btn primary glow ${loading ? 'btn-loading' : ''}`} 
                onClick={handleSendReport}
                disabled={loading}
              >
                {loading ? 'DISPATCHING...' : 'INITIATE SMTP DISPATCH 🚀'}
              </button>
           </div>
        </div>
      )}
    </div>
  )
}
