using Contoso.Support.Domain.Enums;

namespace Contoso.Support.Application.DTOs;

/// <summary>
/// The structured output the Triage Agent returns for each user message.
/// </summary>
public record TriageResult
{
    /// <summary>The classified intent of the user's message.</summary>
    public IntentType Intent { get; init; } = IntentType.Unknown;

    /// <summary>Confidence score for the intent classification (0.0 to 1.0).</summary>
    public double Confidence { get; init; }

    /// <summary>The AI-generated response to send back to the customer.</summary>
    public string Response { get; init; } = string.Empty;

    /// <summary>Extracted product name if mentioned by the customer.</summary>
    public string? ProductName { get; init; }

    /// <summary>Extracted issue description if the customer described a problem.</summary>
    public string? IssueDescription { get; init; }

    /// <summary>Whether the agent needs more information before proceeding.</summary>
    public bool NeedsMoreInfo { get; init; }

    /// <summary>The conversation session ID for continuity.</summary>
    public string? SessionId { get; init; }
}
