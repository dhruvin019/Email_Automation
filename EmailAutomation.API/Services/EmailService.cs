using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using EmailAutomation.API.Models;
using System.Text;

namespace EmailAutomation.API.Services;

public interface IEmailService
{
    Task SendReportAsync(SendReportRequest request);
}

public class EmailService : IEmailService
{
    public async Task SendReportAsync(SendReportRequest request)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(request.SenderEmail));
        foreach (var receiver in request.ReceiverEmails)
        {
            email.To.Add(MailboxAddress.Parse(receiver));
        }
        
        email.Subject = $"Daily Work Report of {DateTime.Now:dd/MM/yyyy}";

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

        for (int i = 0; i < request.Tasks.Count; i++)
        {
            var t = request.Tasks[i];
            sb.Append("<tr>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center;\">{i + 1}.</td>");
            var formattedDetail = string.Join("<br/>", t.TaskDetail.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(line => $"• {line.Trim()}"));
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px;\"><b>Task {t.TaskNo} {t.TaskName} ({t.Status})</b><br/>{formattedDetail}</td>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center;\">{t.StartTime}</td>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center;\">{t.EndTime}</td>");
            sb.Append($"<td style=\"border: 1px solid #e2e8f0; padding: 12px; text-align: center; font-weight: bold;\">{t.Hours}</td>");
            sb.Append("</tr>");
        }

        sb.Append("</tbody></table></div>");

        email.Body = new TextPart(TextFormat.Html) { Text = sb.ToString() };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(request.SenderEmail, request.SmtpPassword);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
