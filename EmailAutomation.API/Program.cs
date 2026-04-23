using EmailAutomation.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmailService, EmailService>();

// Add CORS to allow requests from the frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",      // Vite dev server
                "http://localhost:3000",      // Alternative dev port
                "https://emailautomation.vercel.app" // Production frontend
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

<<<<<<< Updated upstream
app.UseCors("AllowAll");
=======
// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
>>>>>>> Stashed changes
app.UseAuthorization();
app.MapControllers();

// Log startup information
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("EmailAutomation API starting on port 5109");
logger.LogInformation($"Environment: {app.Environment.EnvironmentName}");

app.Run();
