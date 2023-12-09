using Ardalis.Result;
using Cnab.Application.UoW;
using Cnab.Domain.Dto;
using Cnab.Domain.Dto.Store;
using Cnab.Domain.Services;

namespace Cnab.Application.Services;
public class StoreServices : IStoreServices
{
    private readonly UnitOfWork _unitOfWork;

    public StoreServices(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BasePaginated<StoreResponseDto>>> Get(StoreRequestParameterDto param)
    {
        var count = await _unitOfWork.StoreRepository.Count(param.Name, param.Name);

        if (count == 0)
            return Result.Success(BasePaginated<StoreResponseDto>.BuildDefault(param.Page, param.Size));

        var data = await _unitOfWork.StoreRepository.GetPaginated(param.Page, param.Size, param.Name, param.Owner);

        return Result.Success(new BasePaginated<StoreResponseDto>(data.Select(StoreResponseDto.FromDomain).ToArray(), param.Page, param.Size, count));
    }

    public async Task<Result<IEnumerable<StoreTransactionResponseDto>>> GetTransactions(Guid param)
    {
        var store = await _unitOfWork.StoreRepository.GetById(param);

        if (store is null)
            return Result.NotFound();

        var data = await _unitOfWork.StoreRepository.GetTransactions(param);

        return Result.Success(data.Select(StoreTransactionResponseDto.FromDomain));
    }
}
