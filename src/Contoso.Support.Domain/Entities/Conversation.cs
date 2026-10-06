namespace Contoso.Support.Domain.Entities;

/// <summary>
/// Represents a customer support conversation session.
/// </summary>
public class Conversation
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }

    /// <summary>The session ID used by the AI Agent Framework.</summary>
    public string SessionId { get; set; } = string.Empty;

    /// <summary>Current detected intent of the conversation.</summary>
    public string CurrentIntent { get; set; } = string.Empty;

    /// <summary>JSON blob of extracted entities (customerId, productId, etc.).</summary>
    public string ExtractedEntities { get; set; } = "{}";

    /// <summary>Claim ID if a claim was created during this conversation.</summary>
    public Guid? ClaimId { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EndedAt { get; set; }

    // Navigation
    public Customer Customer { get; set; } = null!;
    public Claim? Claim { get; set; }
    public ICollection<ConversationMessage> Messages { get; set; } = [];
}
