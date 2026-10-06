namespace Contoso.Support.Application.DTOs;

/// <summary>
/// Represents an incoming support message from a customer.
/// </summary>
public record SupportMessageRequest
{
    /// <summary>The customer's message text.</summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>Existing conversation session ID (null for new conversations).</summary>
    public string? SessionId { get; init; }

    /// <summary>Customer ID if authenticated (null for anonymous).</summary>
    public Guid? CustomerId { get; init; }
}
