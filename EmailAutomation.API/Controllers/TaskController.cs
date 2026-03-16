using Microsoft.AspNetCore.Mvc;
using EmailAutomation.API.Models;
using EmailAutomation.API.Services;

namespace EmailAutomation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly IEmailService _emailService;

    public TaskController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send-report")]
    public async Task<IActionResult> SendReport([FromBody] SendReportRequest request)
    {
        Console.WriteLine($"[API] Received report request from {request.SenderEmail}");
        try
        {
            await _emailService.SendReportAsync(request);
            Console.WriteLine("[API] Report sent successfully");
            return Ok(new { message = "Report sent successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API] ERROR: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
    }
}
