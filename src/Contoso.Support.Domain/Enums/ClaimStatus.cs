namespace Contoso.Support.Domain.Enums;

/// <summary>
/// Represents the lifecycle status of a warranty claim.
/// </summary>
public enum ClaimStatus
{
    /// <summary>Claim has been created but not yet processed.</summary>
    Pending = 0,

    /// <summary>Claim is being investigated by the multi-agent workflow.</summary>
    Investigating,

    /// <summary>All evidence has been collected and reviewed.</summary>
    EvidenceCollected,

    /// <summary>Warranty policy has been evaluated against the claim.</summary>
    PolicyEvaluated,

    /// <summary>Claim requires human review before a decision can be made.</summary>
    HumanReviewRequired,

    /// <summary>Claim has been approved (auto or human).</summary>
    Approved,

    /// <summary>Claim has been rejected (auto or human).</summary>
    Rejected,

    /// <summary>Claim has been fully resolved and closed.</summary>
    Resolved,

    /// <summary>Claim processing failed and needs attention.</summary>
    Failed
}
