using Ardalis.Result;
using Cnab.Domain.Dto.Import;

namespace Cnab.Domain.Services;

public interface IImportServices
{
    Task<Result> Import(ImportRequestDto param, CancellationToken cancellationToken = default);

    Task<Result<ImportResponseDto>> Get(Guid id);
}
