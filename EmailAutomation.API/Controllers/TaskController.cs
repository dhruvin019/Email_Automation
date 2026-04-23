using Microsoft.AspNetCore.Mvc;
using EmailAutomation.API.Models;
using EmailAutomation.API.Services;

namespace EmailAutomation.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly IEmailService _emailService;
    private readonly ILogger<TaskController> _logger;

    public TaskController(IEmailService emailService, ILogger<TaskController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("send-report")]
    public async Task<IActionResult> SendReport([FromBody] SendReportRequest request)
    {
        _logger.LogInformation($"[API] Received report request from {request.SenderEmail}");
        Console.WriteLine($"[API] Received report request from {request.SenderEmail}");

        try
        {
            await _emailService.SendReportAsync(request);
            _logger.LogInformation("[API] Report sent successfully");
            Console.WriteLine("[API] Report sent successfully");
            return Ok(new { message = "Report sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"[API] ERROR: {ex.Message}");
            Console.WriteLine($"[API] ERROR: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("health")]
    public IActionResult Health()
    {
        _logger.LogInformation("[API] Health check");
        return Ok(new { status = "API is running", timestamp = DateTime.UtcNow });
    }
}
