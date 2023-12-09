using Cnab.Domain.Enum;

namespace Cnab.Domain.Dto.Import;
public record ImportResponseDto : BaseResponse
{
    public string FileName { get; set; } = string.Empty;
    public ImportProccessStatus Status { get; set; }

    public static ImportResponseDto FromDomain(Model.Import param)
    {
        return new()
        {
            CreatedAt = param.CreatedAt,
            FileName = param.FileName,
            Status = param.Status,
            Id = param.Id,
            UpdatedAt = param.UpdatedAt
        };
    }
}
