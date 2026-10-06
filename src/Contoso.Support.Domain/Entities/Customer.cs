namespace Contoso.Support.Domain.Entities;

/// <summary>
/// Represents a Contoso customer.
/// </summary>
public class Customer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string FullName => $"{FirstName} {LastName}";

    // Navigation properties
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<Claim> Claims { get; set; } = [];
    public ICollection<Conversation> Conversations { get; set; } = [];
}
