using EmailAutomation.API.Models;

namespace EmailAutomation.API.Services;

public interface IEmailService
{
    Task SendReportAsync(SendReportRequest request);
}
