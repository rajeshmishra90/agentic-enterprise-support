using Contoso.Support.Domain.Enums;

namespace Contoso.Support.Domain.Entities;

/// <summary>
/// Represents a warranty claim filed by a customer.
/// This is the central entity in the claims workflow.
/// </summary>
public class Claim
{
    public Guid Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public Guid? OrderItemId { get; set; }
    public Guid? WarrantyId { get; set; }

    /// <summary>Customer's description of the issue.</summary>
    public string IssueDescription { get; set; } = string.Empty;

    /// <summary>AI-classified damage type.</summary>
    public DamageType DamageType { get; set; } = DamageType.Unknown;

    /// <summary>Current status of the claim lifecycle.</summary>
    public ClaimStatus Status { get; set; } = ClaimStatus.Pending;

    /// <summary>Final decision from the Policy Engine.</summary>
    public ClaimDecision Decision { get; set; } = ClaimDecision.Pending;

    /// <summary>Risk level assessed during investigation.</summary>
    public RiskLevel RiskLevel { get; set; } = RiskLevel.Low;

    /// <summary>AI confidence score (0.0 to 1.0).</summary>
    public double Confidence { get; set; }

    /// <summary>Estimated claim value for risk assessment.</summary>
    public decimal ClaimValue { get; set; }

    /// <summary>Reason for the decision (human-readable).</summary>
    public string DecisionReason { get; set; } = string.Empty;

    /// <summary>AI-generated investigation summary.</summary>
    public string InvestigationSummary { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; set; }

    // Navigation
    public Customer Customer { get; set; } = null!;
    public Product Product { get; set; } = null!;
    public OrderItem? OrderItem { get; set; }
    public Warranty? Warranty { get; set; }
    public ICollection<ClaimEvent> Events { get; set; } = [];
}
