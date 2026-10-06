namespace Contoso.Support.Domain.Entities;

/// <summary>
/// Immutable event log entry for claim lifecycle events.
/// Every state change is captured for full auditability.
/// </summary>
public class ClaimEvent
{
    public Guid Id { get; set; }
    public Guid ClaimId { get; set; }

    /// <summary>Event type (e.g., "ClaimCreated", "InvestigationCompleted", "DecisionGenerated").</summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>JSON payload with event-specific data.</summary>
    public string Data { get; set; } = string.Empty;

    /// <summary>Who or what triggered this event (e.g., "TriageAgent", "PolicyEngine", "HumanReviewer").</summary>
    public string Source { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation
    public Claim Claim { get; set; } = null!;
}
