import { useNavigate } from 'react-router-dom'
import './SuccessPage.css'

export default function SuccessPage() {
    const navigate = useNavigate()

    return (
        <div className="success-page fade-in app-shell">
            <div className="glass-panel celebration-card slide-up">
                <div className="celebration-orb">
                   <div className="inner-glow">✓</div>
                </div>
                
                <div className="celebration-content">
                    <h1 className="text-gradient">Dispatch Prepared</h1>
                    <p className="subtitle">
                        Your report has been compiled and the system is ready for the next rotation.
                    </p>
                </div>

                <div className="dispatch-stats">
                   <div className="stat-node">
                      <span className="label">Status</span>
                      <span className="value active">Verified</span>
                   </div>
                   <div className="stat-node">
                      <span className="label">Encryption</span>
                      <span className="value">Active</span>
                   </div>
                </div>

                <div className="celebration-actions">
                    <button className="premium-btn primary glow" onClick={() => navigate('/task')}>
                        BUILD NEXT REPORT <span>+</span>
                    </button>
                    <button className="premium-btn secondary" onClick={() => navigate('/setup')}>
                        SYSTEM CONFIG <span>⚙️</span>
                    </button>
                </div>
            </div>
        </div>
    )
}
