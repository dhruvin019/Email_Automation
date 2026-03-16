import { Routes, Route, Navigate } from 'react-router-dom'
import Navbar from './components/Navbar'
import { Toaster } from 'react-hot-toast'
import SetupPage from './pages/SetupPage'
import TaskPage from './pages/TaskPage'
import SuccessPage from './pages/SuccessPage'
import Cookies from 'js-cookie'

export default function App() {
    // Logic to check if user has already set up their details
    const isSetup = !!(Cookies.get('senderEmail') && Cookies.get('smtpPassword'))

    return (
        <div className="app-shell">
            <Toaster position="top-right" containerStyle={{ zIndex: 99999 }} />
            <Navbar />
            <main className="page-content">
                <Routes>
                    <Route 
                        path="/" 
                        element={<Navigate to={isSetup ? "/task" : "/setup"} replace />} 
                    />
                    <Route path="/setup" element={<SetupPage />} />
                    <Route path="/task" element={<TaskPage />} />
                    <Route path="/success" element={<SuccessPage />} />
                </Routes>
            </main>
        </div>
    )
}
