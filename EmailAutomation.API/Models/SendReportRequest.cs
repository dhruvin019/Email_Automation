namespace EmailAutomation.API.Models;

public class SendReportRequest
{
    public string SenderEmail { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public List<string> ReceiverEmails { get; set; } = new();
    public List<TaskItem> Tasks { get; set; } = new();
    public string? HtmlBody { get; set; }
}
