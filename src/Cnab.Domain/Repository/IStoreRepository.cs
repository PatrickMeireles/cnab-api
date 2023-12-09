using Cnab.Domain.Model;

namespace Cnab.Domain.Repository;
public interface IStoreRepository
{
    Task<Guid> GetByNameAndOwner(string name, string owner);

    Task AddWithTransaction(Store param, StoreTransaction transaction);

    Task UpdateWithTransaction(Guid id, decimal balance, StoreTransaction transaction);

    Task<int> Count(string? name, string? owner);

    Task<IEnumerable<Store>> GetPaginated(int page, int size, string? name, string? owner);

    Task<IEnumerable<StoreTransaction>> GetTransactions(Guid storeId);
    Task<Store?> GetById(Guid id);
}
