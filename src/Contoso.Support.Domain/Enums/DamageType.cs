namespace Contoso.Support.Domain.Enums;

/// <summary>
/// Categorizes the type of damage reported in a warranty claim.
/// Some types are covered by standard warranty; others are excluded.
/// </summary>
public enum DamageType
{
    Unknown = 0,

    /// <summary>Manufacturing defect — covered by standard warranty.</summary>
    ManufacturingDefect,

    /// <summary>Hardware failure under normal use — covered by standard warranty.</summary>
    HardwareFailure,

    /// <summary>Software issue — may be covered depending on policy.</summary>
    SoftwareIssue,

    /// <summary>Accidental damage — excluded from standard warranty.</summary>
    AccidentalDamage,

    /// <summary>Liquid damage — excluded from standard warranty.</summary>
    LiquidDamage,

    /// <summary>Unauthorized repair — excluded from standard warranty.</summary>
    UnauthorizedRepair,

    /// <summary>Physical abuse — excluded from standard warranty.</summary>
    PhysicalAbuse,

    /// <summary>Modified hardware — excluded from standard warranty.</summary>
    ModifiedHardware,

    /// <summary>Normal wear and tear — excluded from standard warranty.</summary>
    WearAndTear
}
