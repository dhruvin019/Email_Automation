import axios from 'axios'

// Direct connection to the deployed Render backend
const API_BASE_URL = 'https://email-automation-2-ruj3.onrender.com'

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: { 'Content-Type': 'application/json' },
})

export default api
