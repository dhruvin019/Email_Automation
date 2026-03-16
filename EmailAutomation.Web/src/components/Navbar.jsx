import { useNavigate, useLocation } from 'react-router-dom'
import './Navbar.css'

export default function Navbar() {
  const navigate = useNavigate()
  const location = useLocation()

  return (
    <nav className="navbar slide-down">
      <div className="nav-container glass-panel">
        <div className="nav-logo" onClick={() => navigate('/')}>
          <div className="logo-orb"></div>
          <span className="logo-text">REPORT<span className="accent">OS</span></span>
        </div>
        
        <div className="nav-links">
          <button 
            className={`nav-link ${location.pathname === '/setup' ? 'active' : ''}`}
            onClick={() => navigate('/setup')}
          >
            Config
          </button>
          <button 
            className={`nav-link ${location.pathname === '/task' ? 'active' : ''}`}
            onClick={() => navigate('/task')}
          >
            Dashboard
          </button>
        </div>
        
      </div>
    </nav>
  )
}
