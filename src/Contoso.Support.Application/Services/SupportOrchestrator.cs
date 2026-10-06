using Contoso.Support.Application.DTOs;
using Contoso.Support.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Contoso.Support.Application.Services;

/// <summary>
/// Orchestrates the support conversation flow by delegating to the Triage Agent.
/// In later phases, this will also coordinate with the claims workflow and Service Bus.
/// </summary>
public class SupportOrchestrator : ISupportOrchestrator
{
    private readonly IConversationalTriageAgent _triageAgent;
    private readonly ILogger<SupportOrchestrator> _logger;

    public SupportOrchestrator(
        IConversationalTriageAgent triageAgent,
        ILogger<SupportOrchestrator> logger)
    {
        _triageAgent = triageAgent;
        _logger = logger;
    }

    public async Task<SupportMessageResponse> ProcessMessageAsync(
        SupportMessageRequest request,
        CancellationToken ct = default)
    {
        _logger.LogInformation(
            "Processing support message. SessionId: {SessionId}, CustomerId: {CustomerId}",
            request.SessionId ?? "new",
            request.CustomerId);

        var triageResult = await _triageAgent.ProcessUserMessageAsync(
            request.Message,
            request.SessionId);

        return new SupportMessageResponse
        {
            Message = triageResult.Response,
            SessionId = triageResult.SessionId ?? Guid.NewGuid().ToString(),
            Intent = triageResult.Intent.ToString(),
            Confidence = triageResult.Confidence
        };
    }
}
