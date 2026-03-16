export const buildReportHtml = (tasks, date) => {
  const rows = tasks.map((task, index) => `
    <tr>
      <td style="border: 1px solid #e2e8f0; padding: 12px; text-align: center; color: #64748b;">${index + 1}.</td>
      <td style="border: 1px solid #e2e8f0; padding: 12px;">
        <div style="font-weight: 700; color: #1e293b; margin-bottom: 4px;">Task ${task.taskNo} ${task.taskName} (${task.status})</div>
        <div style="font-size: 13px; color: #475569; font-style: italic;">
          ${task.taskDetail.split('\n').filter(line => line.trim()).map(line => `• ${line.trim()}`).join('<br/>')}
        </div>
      </td>
      <td style="border: 1px solid #e2e8f0; padding: 12px; text-align: center; color: #1e293b; font-weight: 600;">${task.startTime}</td>
      <td style="border: 1px solid #e2e8f0; padding: 12px; text-align: center; color: #1e293b; font-weight: 600;">${task.endTime}</td>
      <td style="border: 1px solid #e2e8f0; padding: 12px; text-align: center; font-weight: 800; color: #0ea5e9;">${task.hours}</td>
    </tr>
  `).join('');

  return `
    <div style="font-family: 'Inter', system-ui, -apple-system, sans-serif; max-width: 800px; margin: 0 auto; color: #334155;">
      <h2 style="color: #0f172a; border-bottom: 2px solid #0ea5e9; padding-bottom: 12px; margin-bottom: 24px;">Daily Work Report of ${date}</h2>
      <p style="margin-bottom: 8px;">Dear sir,</p>
      <p style="margin-bottom: 24px;">Please find below-mentioned tasks done today.</p>
      
      <table style="width: 100%; border-collapse: collapse; background: white; border: 1px solid #e2e8f0; border-radius: 8px; overflow: hidden;">
        <thead>
          <tr style="background: #f8fafc; color: #64748b; font-size: 11px; text-transform: uppercase; letter-spacing: 0.05em;">
            <th style="border: 1px solid #e2e8f0; padding: 12px; width: 60px;">SR</th>
            <th style="border: 1px solid #e2e8f0; padding: 12px; text-align: left;">Work Description</th>
            <th style="border: 1px solid #e2e8f0; padding: 12px; width: 60px;">Start</th>
            <th style="border: 1px solid #e2e8f0; padding: 12px; width: 60px;">End</th>
            <th style="border: 1px solid #e2e8f0; padding: 12px; width: 60px;">Hours</th>
          </tr>
        </thead>
        <tbody>
          ${rows}
        </tbody>
      </table>
      
      <p style="margin-top: 32px; font-size: 12px; color: #94a3b8; text-align: center;">
        Generated via ReportOS • Professional Efficiency
      </p>
    </div>
  `;
};

export const generateMailtoLink = (receivers, subject, htmlBody) => {
    // Note: Mailto only supports plain text well. 
    // For HTML, we usually recommend 'Copy HTML' + pasting in client.
    // However, we can provide a plain text version for the body.
    const body = "Please find the attached work report table (Sent via ReportOS).";
    return `mailto:${receivers.join(',')}?subject=${encodeURIComponent(subject)}&body=${encodeURIComponent(body)}`;
};
