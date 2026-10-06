namespace Contoso.Support.Application.DTOs;

/// <summary>
/// The response sent back to the customer after processing their message.
/// </summary>
public record SupportMessageResponse
{
    /// <summary>The AI-generated response text.</summary>
    public string Message { get; init; } = string.Empty;

    /// <summary>The conversation session ID for follow-up messages.</summary>
    public string SessionId { get; init; } = string.Empty;

    /// <summary>The detected intent of the customer's message.</summary>
    public string Intent { get; init; } = string.Empty;

    /// <summary>Confidence score for intent classification.</summary>
    public double Confidence { get; init; }

    /// <summary>Ticket number if a claim was created.</summary>
    public string? TicketNumber { get; init; }
}
