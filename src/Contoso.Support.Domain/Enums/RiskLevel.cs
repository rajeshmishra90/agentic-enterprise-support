namespace Contoso.Support.Domain.Enums;

/// <summary>
/// Risk classification for a warranty claim.
/// Used by the Policy Engine to determine if auto-approval is safe.
/// </summary>
public enum RiskLevel
{
    Low = 0,
    Medium,
    High,
    Critical
}
