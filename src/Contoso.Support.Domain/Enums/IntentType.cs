namespace Contoso.Support.Domain.Enums;

/// <summary>
/// Represents the classified intent of a customer's message.
/// Used by the Conversational Triage Agent to determine routing.
/// </summary>
public enum IntentType
{
    /// <summary>Default when intent cannot be determined.</summary>
    Unknown = 0,

    /// <summary>Customer is asking about product specifications, features, or compatibility.</summary>
    ProductQuestion,

    /// <summary>Customer is asking about warranty policy, coverage, or terms.</summary>
    WarrantyQuestion,

    /// <summary>Customer wants to file a warranty claim.</summary>
    WarrantyClaim,

    /// <summary>Customer needs help troubleshooting a product issue.</summary>
    Troubleshooting,

    /// <summary>Customer wants to check the status of an existing claim or order.</summary>
    StatusCheck,

    /// <summary>Customer wants to escalate to a human support agent.</summary>
    Escalation,

    /// <summary>General greeting, feedback, or non-support conversation.</summary>
    General
}
