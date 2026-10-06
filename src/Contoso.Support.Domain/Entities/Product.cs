namespace Contoso.Support.Domain.Entities;

/// <summary>
/// Represents a Contoso Electronics product.
/// </summary>
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ModelNumber { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Specifications { get; set; } = string.Empty;
    public string Features { get; set; } = string.Empty;

    /// <summary>Standard warranty duration in months.</summary>
    public int WarrantyMonths { get; set; } = 12;

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Warranty> Warranties { get; set; } = [];
}
