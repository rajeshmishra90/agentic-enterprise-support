using Contoso.Support.Application.DTOs;

namespace Contoso.Support.Application.Interfaces;

/// <summary>
/// Orchestrates the support conversation flow.
/// Acts as the mediator between the API and the Agent layer.
/// </summary>
public interface ISupportOrchestrator
{
    /// <summary>
    /// Processes a customer support message through the Triage Agent
    /// and returns the appropriate response.
    /// </summary>
    Task<SupportMessageResponse> ProcessMessageAsync(SupportMessageRequest request, CancellationToken ct = default);
}
