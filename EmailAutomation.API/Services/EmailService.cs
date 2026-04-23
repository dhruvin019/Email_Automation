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
        sb.Append("<div style=\"font-family: 'Inter', sans-serif; color: #334155;\">");
        sb.Append("<p>Dear sir,</p>");
        sb.Append("<p>Please find below-mentioned tasks done today.</p>");

        sb.Append("<table style=\"width: 100%; border-collapse: collapse; border: 1px solid #e2e8f0;\">");
        sb.Append("<thead style=\"background: #f8fafc;\">");
        sb.Append("<tr>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; width: 60px;\">SR</th>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: left;\">Work Description</th>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; width: 60px;\">Start</th>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; width: 60px;\">End</th>");
        sb.Append("<th style=\"border: 1px solid #e2e8f0; padding: 12px; width: 60px;\">Hours</th>");
        sb.Append("</tr></thead><tbody>");

        for (int i = 0; i < tasks.Count; i++)
        {
            var t = tasks[i];
            sb.Append("<tr>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center;\">{i + 1}.</td>");

            var formattedDetail = string.Join(
                "<br/>", 
                t.TaskDetail
                    .Split('\n', StringSplitOptions.RemoveEmptyEntries)
                    .Select(line => $"• {line.Trim()}")
            );

            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px;\"><b>Task {t.TaskNo} {t.TaskName} ({t.Status})</b><br/>{formattedDetail}</td>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center;\">{t.StartTime}</td>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center;\">{t.EndTime}</td>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center; font-weight: bold;\">{t.Hours}</td>");
            sb.Append("</tr>");
        }

        sb.Append("</tbody></table></div>");
        return sb.ToString();
    }
}
