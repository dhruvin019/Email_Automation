# EmailAutomation.API

A .NET 9 Core Web API for sending professional daily work reports via email.

## Features

- **Report Generation**: Create formatted HTML email reports with task details
- **Gmail SMTP Integration**: Send emails through Gmail's SMTP server
- **RESTful API**: Clean API endpoints for report submission
- **CORS Support**: Configured for frontend integration
- **Comprehensive Logging**: Built-in logging for debugging

## Project Structure

```
EmailAutomation.API/
├── Controllers/
│   └── TaskController.cs      # API endpoints for report sending
├── Models/
│   ├── SendReportRequest.cs   # Request model for report submission
│   └── TaskItem.cs            # Individual task model
├── Services/
│   ├── IEmailService.cs       # Email service interface
│   └── EmailService.cs        # Email service implementation
├── Program.cs                  # Application startup and configuration
├── appsettings.json           # Configuration settings
├── appsettings.Development.json # Development-specific settings
└── EmailAutomation.API.csproj # Project file
```

## Prerequisites

- .NET 9 SDK
- Gmail account with App Password configured

## Installation

### 1. Install .NET 9 SDK
Download and install from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/9.0)

### 2. Restore NuGet Packages

Navigate to the `EmailAutomation.API` directory and run:
```bash
dotnet restore
```

### 3. Configure Gmail

1. Enable 2-Factor Authentication on your Gmail account
2. Generate an App Password: [myaccount.google.com/apppasswords](https://myaccount.google.com/apppasswords)
3. Use this App Password in the frontend setup (not your regular password)

## Running the API

### Development Mode

```bash
dotnet run
```

The API will start on `http://localhost:5109`

### View API Documentation

Open Swagger UI at: `http://localhost:5109/swagger`

## API Endpoints

### POST `/api/task/send-report`

Sends a daily work report via email.

**Request Body:**
```json
{
  "senderEmail": "your-email@gmail.com",
  "smtpPassword": "your-app-password",
  "receiverEmails": ["recipient1@example.com", "recipient2@example.com"],
  "tasks": [
    {
      "taskNo": "001",
      "taskName": "Database Schema Design",
      "taskDetail": "Created initial schema\nConfigured relationships",
      "status": "Complete",
      "startTime": "09:00",
      "endTime": "11:30",
      "hours": "02:30"
    }
  ],
  "htmlBody": null
}
```

**Response (Success):**
```json
{
  "message": "Report sent successfully"
}
```

**Response (Error):**
```json
{
  "message": "Error description"
}
```

### GET `/api/task/health`

Health check endpoint to verify API is running.

**Response:**
```json
{
  "status": "API is running",
  "timestamp": "2026-04-20T10:30:00Z"
}
```

## Configuration

### CORS Settings

The API is configured to accept requests from:
- `http://localhost:5173` (Vite dev server)
- `http://localhost:3000` (Alternative dev port)
- `https://emailautomation.vercel.app` (Production frontend)

To add more origins, modify `Program.cs`:

```csharp
policy.WithOrigins("https://your-domain.com")
```

### Logging

Logging levels can be configured in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

## Email HTML Template

The API automatically generates a professional HTML email template with:
- Task details in a formatted table
- Serial numbers and work descriptions
- Start time, end time, and hours calculation
- Styled headers and formatting

Custom HTML can be provided via the `htmlBody` field in the request.

## Dependencies

- **MailKit 4.7.1**: SMTP client for email sending
- **Microsoft.AspNetCore.OpenApi**: OpenAPI support
- **Swashbuckle.AspNetCore**: Swagger documentation

## Error Handling

The API provides detailed error messages for:
- Invalid email addresses
- SMTP connection failures
- Authentication errors
- Timeout issues (10-second limit)

## Debugging

Enable debug logging in `appsettings.Development.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "EmailAutomation.API": "Debug"
    }
  }
}
```

Console output includes timestamps for all SMTP operations.

## Troubleshooting

### "Authentication failed"
- Verify Gmail App Password (not regular password)
- Check 2FA is enabled on Gmail account
- Ensure email address matches the sender email

### "Timeout"
- Check internet connection
- Verify SMTP server is accessible
- Consider increasing timeout in EmailService.cs

### "CORS Error"
- Verify frontend URL is in CORS policy
- Check browser console for specific error
- Ensure API is running on correct port

## License

This project is part of the EmailAutomation suite.
