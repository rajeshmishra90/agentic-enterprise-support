namespace Contoso.Support.Domain.Enums;

/// <summary>
/// Represents the status of a support ticket.
/// </summary>
public enum TicketStatus
{
    Open = 0,
    InProgress,
    WaitingOnCustomer,
    WaitingOnAgent,
    Resolved,
    Closed
}
