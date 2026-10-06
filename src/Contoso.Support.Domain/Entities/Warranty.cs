namespace Contoso.Support.Domain.Entities;

/// <summary>
/// Represents a warranty associated with a product purchase.
/// Warranty is per OrderItem (i.e., per specific device purchased).
/// </summary>
public class Warranty
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid OrderItemId { get; set; }
    public Guid CustomerId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    /// <summary>Type of warranty (Standard, Extended, Premium).</summary>
    public string Type { get; set; } = "Standard";

    /// <summary>Covers manufacturing defects and hardware failures under normal use.</summary>
    public string CoveredItems { get; set; } = "Manufacturing defects, Hardware failures under normal use";

    /// <summary>Accidental damage, liquid damage, unauthorized repair, physical abuse, modified hardware.</summary>
    public string Exclusions { get; set; } = "Accidental damage, Liquid damage, Unauthorized repair, Physical abuse, Modified hardware";

    public bool IsActive => DateTime.UtcNow >= StartDate && DateTime.UtcNow <= EndDate;

    // Navigation
    public Product Product { get; set; } = null!;
    public OrderItem OrderItem { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
}
