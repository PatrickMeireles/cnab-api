namespace Cnab.Domain.Dto.Store;
public record StoreResponseDto : BaseResponse
{
    public string Name { get; init; } = string.Empty;
    public string Owner { get; init; } = string.Empty;
    public decimal Balance { get; init; }


    public static StoreResponseDto FromDomain(Model.Store param)
    {
        return new()
        {
            Balance = param.Balance,
            Name = param.Name,
            Owner = param.Owner,
            CreatedAt = param.CreatedAt,
            Id = param.Id,
            UpdatedAt = param.UpdatedAt
        };
    }
}
