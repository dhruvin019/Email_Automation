using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using EmailAutomation.API.Models;
using System.Text;

namespace EmailAutomation.API.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendReportAsync(SendReportRequest request)
    {
        _logger.LogInformation($"[EmailService] Building email for {request.SenderEmail}");

        // Validate request
        if (string.IsNullOrWhiteSpace(request.SenderEmail))
            throw new ArgumentException("Sender email is required");

        if (request.ReceiverEmails == null || request.ReceiverEmails.Count == 0)
            throw new ArgumentException("At least one receiver email is required");

        if (request.Tasks == null || request.Tasks.Count == 0)
            throw new ArgumentException("At least one task is required");

        // Create email message
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(request.SenderEmail));

        foreach (var receiver in request.ReceiverEmails)
        {
            if (!string.IsNullOrWhiteSpace(receiver))
                email.To.Add(MailboxAddress.Parse(receiver.Trim()));
        }

        email.Subject = $"Daily Work Report of {DateTime.Now:dd/MM/yyyy}";

        // Build HTML body
        string finalHtml = string.IsNullOrEmpty(request.HtmlBody) 
            ? BuildEmailHtml(request.Tasks) 
            : request.HtmlBody;

        email.Body = new TextPart(TextFormat.Html) { Text = finalHtml };

        try
        {
            using var smtp = new SmtpClient();
            smtp.Timeout = 10000; // 10 seconds timeout

            _logger.LogInformation($"[EmailService] Connecting to SMTP server");
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            _logger.LogInformation($"[EmailService] Authenticating with {request.SenderEmail}");
            await smtp.AuthenticateAsync(request.SenderEmail, request.SmtpPassword);

            _logger.LogInformation($"[EmailService] Sending email to {string.Join(", ", request.ReceiverEmails)}");
            await smtp.SendAsync(email);

            _logger.LogInformation($"[EmailService] Disconnecting from SMTP server");
            await smtp.DisconnectAsync(true);

            _logger.LogInformation($"[EmailService] Email sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"[EmailService] SMTP Error: {ex.Message}");
            throw new InvalidOperationException(
                $"SMTP Error: Failed to send email from {request.SenderEmail}. Details: {ex.Message}", 
                ex);
        }
    }

    private string BuildEmailHtml(List<TaskItem> tasks)
    {
        var sb = new StringBuilder();
        sb.Append("<div style=\"font-family: 'Inter', sans-serif; color: #334155; max-width: 800px; margin: 0 auto;\">");
        sb.Append("<p>Dear Sir,</p>");
        sb.Append("<p>Please find the daily work report for today.</p>");

        sb.Append("<table style=\"width: 100%; border-collapse: collapse; border: 1px solid #e2e8f0; margin-bottom: 20px;\">");
        sb.Append("<thead style=\"background: #f8fafc;\">");
        sb.Append("<tr>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; width: 50px;\">SR.</th>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: left;\">Action Item</th>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; width: 80px;\">Start</th>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; width: 80px;\">End</th>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; width: 80px;\">Hours</th>");
        sb.Append("</tr></thead><tbody>");

        double totalHoursSum = 0;

        for (int i = 0; i < tasks.Count; i++)
        {
            var t = tasks[i];

            // Parse hours
            double taskHours = 0;
            if (TimeSpan.TryParse(t.Hours, out var ts))
            {
                taskHours = ts.TotalHours;
            }

            // Subtract 30 mins if IncludeBreak is checked
            if (t.IncludeBreak)
            {
                taskHours = Math.Max(0, taskHours - 0.5);
            }

            totalHoursSum += taskHours;

            // Format displayed hours (H:mm)
            var h = (int)taskHours;
            var m = (int)((taskHours - h) * 60);
            string displayHours = $"{h:D2}:{m:D2}";

            sb.Append("<tr>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center;\">{i + 1}.</td>");

            var formattedDetail = string.Join(
                "<br/>", 
                t.TaskDetail
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => $"• {line.Trim()}")
            );

            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px;\"><b>{t.TaskName}</b><br/>{formattedDetail}</td>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center;\">{t.StartTime}</td>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center;\">{t.EndTime}</td>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center; font-weight: bold;\">{displayHours}</td>");
            sb.Append("</tr>");
        }

        // Total Row
        var totalH = (int)totalHoursSum;
        var totalM = (int)((totalHoursSum - totalH) * 60);
        string displayTotal = $"{totalH:D2}:{totalM:D2}";

        sb.Append("<tr style=\"background: #f8fafc;\">");
        sb.Append("<td colspan=\"4\" style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: right; font-weight: bold;\">Total Hours:</td>");
        sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center; font-weight: bold;\">{displayTotal}</td>");
        sb.Append("</tr>");

        sb.Append("</tbody></table>");
        
        sb.Append("<p>Thanks & Regards,<br/><b>Dhruvin Virpara</b></p>");
        sb.Append("</div>");
        return sb.ToString();
    }
}
