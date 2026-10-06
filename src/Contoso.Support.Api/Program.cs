using Azure.AI.Projects;
using Azure.Identity;
using Contoso.Support.Agents;
using Contoso.Support.Application.DTOs;
using Contoso.Support.Application.Interfaces;
using Contoso.Support.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// --- Service Defaults (Aspire: OpenTelemetry, Health Checks, Resilience) ---
builder.AddServiceDefaults();

// --- OpenAPI (.NET 10 native) ---
builder.Services.AddOpenApi();

// --- Azure AI Foundry ---
var foundryEndpoint = builder.Configuration["AzureAI:Endpoint"]
    ?? throw new InvalidOperationException("AzureAI:Endpoint configuration is required.");

builder.Services.AddSingleton(_ =>
    new AIProjectClient(new Uri(foundryEndpoint), new DefaultAzureCredential()));

// --- Dependency Injection ---
builder.Services.AddSingleton<IConversationalTriageAgent, ConversationalTriageAgent>();
builder.Services.AddScoped<ISupportOrchestrator, SupportOrchestrator>();

// --- CORS (allow React dev server) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// --- Middleware Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

// --- Health Check (Aspire default) ---
app.MapDefaultEndpoints();

// ============================================================
// API Endpoints
// ============================================================

// POST /api/support/messages — Core chat endpoint
app.MapPost("/api/support/messages", async (
    SupportMessageRequest request,
    ISupportOrchestrator orchestrator,
    CancellationToken ct) =>
{
    if (string.IsNullOrWhiteSpace(request.Message))
        return Results.BadRequest(new { error = "Message cannot be empty." });

    var response = await orchestrator.ProcessMessageAsync(request, ct);
    return Results.Ok(response);
})
.WithName("SendSupportMessage")
.WithTags("Support")
.Produces<SupportMessageResponse>(200)
.Produces(400);

// GET /api/health/status — Extended health status
app.MapGet("/api/health/status", () =>
{
    return Results.Ok(new
    {
        status = "Healthy",
        version = "v0.1.0",
        timestamp = DateTime.UtcNow,
        services = new
        {
            api = "Running",
            foundry = "Connected",
            agents = new[] { "ConversationalTriageAgent" }
        }
    });
})
.WithName("GetHealthStatus")
.WithTags("Health");

// GET /api/products (stub — will be backed by SQL on Day 3)
app.MapGet("/api/products", () =>
{
    return Results.Ok(new[]
    {
        new { Name = "Laptop Pro 14", ModelNumber = "LP-14-2025", Category = "Laptops", Price = 89999m },
        new { Name = "Laptop Pro 16", ModelNumber = "LP-16-2025", Category = "Laptops", Price = 119999m },
        new { Name = "SmartPhone X1", ModelNumber = "SP-X1-2025", Category = "Smartphones", Price = 49999m },
        new { Name = "SmartPhone X2", ModelNumber = "SP-X2-2025", Category = "Smartphones", Price = 69999m },
        new { Name = "SmartWatch S2", ModelNumber = "SW-S2-2025", Category = "Wearables", Price = 19999m },
        new { Name = "Earbuds Pro", ModelNumber = "EB-PRO-2025", Category = "Audio", Price = 9999m },
    });
})
.WithName("GetProducts")
.WithTags("Products");

app.Run();
