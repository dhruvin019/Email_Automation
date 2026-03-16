import axios from 'axios'

const api = axios.create({
    baseURL: '/api',
    headers: { 'Content-Type': 'application/json' },
})

/**
 * Send a task email.
 * Credentials are passed directly from browser localStorage.
 */
export const sendTaskEmail = ({ taskNo, taskDetail }) => {
    const senderEmail = localStorage.getItem('senderEmail') || ''
    const smtpPassword = localStorage.getItem('smtpPassword') || ''
    const receiverEmail = localStorage.getItem('receiverEmail') || ''

    return api.post('/task/send', {
        taskNo,
        taskDetail,
        senderEmail,
        smtpPassword,
        receiverEmail,
    })
}
