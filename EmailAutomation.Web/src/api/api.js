import axios from 'axios'

// Choose the backend based on where the frontend is running.
// Local frontend: http://localhost:5173 -> http://localhost:5109
// Vercel frontend: https://email-automation-livid.vercel.app -> http://emailautomationdhr2.runasp.net/
const FRONTEND_ORIGIN = window.location.origin

const API_BASE_URL =
    FRONTEND_ORIGIN === 'http://localhost:5173'
        ? 'http://localhost:5109'
        : FRONTEND_ORIGIN === 'https://email-automation-livid.vercel.app'
            ? 'http://emailautomationdhr2.runasp.net/'
            : (import.meta.env.VITE_API_BASE_URL || 'http://localhost:5109')

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: { 'Content-Type': 'application/json' },
})

export default api
