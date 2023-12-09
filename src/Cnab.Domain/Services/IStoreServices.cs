using Ardalis.Result;
using Cnab.Domain.Dto;
using Cnab.Domain.Dto.Store;

namespace Cnab.Domain.Services;
public interface IStoreServices
{
    Task<Result<BasePaginated<StoreResponseDto>>> Get(StoreRequestParameterDto param);
    Task<Result<IEnumerable<StoreTransactionResponseDto>>> GetTransactions(Guid param);
}
