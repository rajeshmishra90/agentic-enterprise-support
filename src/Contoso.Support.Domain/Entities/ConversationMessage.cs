namespace Contoso.Support.Domain.Entities;

/// <summary>
/// A single message within a conversation.
/// </summary>
public class ConversationMessage
{
    public Guid Id { get; set; }
    public Guid ConversationId { get; set; }

    /// <summary>Who sent the message: "Customer", "Agent", "System".</summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>The message content.</summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>Optional: the detected intent for this specific message.</summary>
    public string? Intent { get; set; }

    /// <summary>Optional: confidence score for intent detection.</summary>
    public double? Confidence { get; set; }

    /// <summary>Optional: token count for this message (for cost tracking).</summary>
    public int? TokenCount { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation
    public Conversation Conversation { get; set; } = null!;
}
