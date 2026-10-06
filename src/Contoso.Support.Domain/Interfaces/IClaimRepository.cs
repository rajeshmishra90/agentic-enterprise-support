using Contoso.Support.Domain.Entities;

namespace Contoso.Support.Domain.Interfaces;

public interface IClaimRepository
{
    Task<Claim?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Claim?> GetByTicketNumberAsync(string ticketNumber, CancellationToken ct = default);
    Task<IReadOnlyList<Claim>> GetByCustomerIdAsync(Guid customerId, CancellationToken ct = default);
    Task<Claim> CreateAsync(Claim claim, CancellationToken ct = default);
    Task UpdateAsync(Claim claim, CancellationToken ct = default);
    Task AddEventAsync(ClaimEvent claimEvent, CancellationToken ct = default);
}
