namespace Contoso.Support.Domain.Enums;

/// <summary>
/// The deterministic decision output from the Policy Engine.
/// The LLM never directly sets this — only the C# rules engine does.
/// </summary>
public enum ClaimDecision
{
    /// <summary>No decision has been made yet.</summary>
    Pending = 0,

    /// <summary>Claim meets all auto-approval criteria.</summary>
    AutoApproved,

    /// <summary>Claim requires human review due to risk, ambiguity, or policy exception.</summary>
    HumanReviewRequired,

    /// <summary>Claim is automatically rejected (expired warranty, excluded damage type).</summary>
    AutoRejected,

    /// <summary>Approved by a human reviewer.</summary>
    HumanApproved,

    /// <summary>Rejected by a human reviewer.</summary>
    HumanRejected
}
