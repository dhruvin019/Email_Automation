namespace EmailAutomation.API.Models;

public class TaskItem
{
    public string TaskNo { get; set; } = string.Empty;
    public string TaskName { get; set; } = string.Empty;
    public string TaskDetail { get; set; } = string.Empty;
    public string Status { get; set; } = "Complete";
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
    public string Hours { get; set; } = "00:00";
}
