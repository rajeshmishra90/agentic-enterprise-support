using Contoso.Support.Domain.Entities;

namespace Contoso.Support.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Product?> GetByModelNumberAsync(string modelNumber, CancellationToken ct = default);
    Task<Product?> GetByNameAsync(string name, CancellationToken ct = default);
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);
}
