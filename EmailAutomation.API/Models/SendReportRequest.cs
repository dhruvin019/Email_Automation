namespace EmailAutomation.API.Models;

public class TaskEntry
{
    public string TaskNo { get; set; } = string.Empty;
    public string TaskName { get; set; } = string.Empty;
    public string TaskDetail { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public string Hours { get; set; } = string.Empty;
}

public class SendReportRequest
{
    public string SenderEmail { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public List<string> ReceiverEmails { get; set; } = new();
    public List<TaskEntry> Tasks { get; set; } = new();
}
