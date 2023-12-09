using Ardalis.Result;
using Cnab.Application.UoW;
using Cnab.Domain.Dto.Import;
using Cnab.Domain.Services;

namespace Cnab.Application.Services;

public class ImportServices : IImportServices
{
    private readonly UnitOfWork _unitOfWork;

    public ImportServices(UnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ImportResponseDto>> Get(Guid id)
    {
        var result = await _unitOfWork.ImportRepository.GetById(id);

        if (result is null)
            return Result.NotFound();

        return Result.Success(ImportResponseDto.FromDomain(result));
    }

    public async Task<Result> Import(ImportRequestDto param, CancellationToken cancellationToken = default)
    {
        var exist = await _unitOfWork.ImportRepository.ExistByName(param.FileName);

        if (exist)
            return Result.Conflict();

        var import = ImportAdapter.ToDomain(param);

        await _unitOfWork.ImportRepository.Add(import);

        _unitOfWork.Commit();

        return Result.Success();
    }
}
