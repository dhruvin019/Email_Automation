namespace EmailAutomation.API.Models;

public class SendTaskRequest
{
    public int TaskNo { get; set; }
    public string TaskDetail { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public string ReceiverEmail { get; set; } = string.Empty;
}
