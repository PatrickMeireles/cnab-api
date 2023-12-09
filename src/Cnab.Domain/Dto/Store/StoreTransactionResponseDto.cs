using Cnab.Domain.Enum;
using Cnab.Domain.Model;

namespace Cnab.Domain.Dto.Store;
public record StoreTransactionResponseDto : BaseResponse
{
    public TypeMoviment TypeMoviment { get; init; }
    public string ClientDocument { get; init; } = string.Empty;
    public string Card { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public decimal Value { get; init; }
    public Guid StoreId { get; init; }

    public static StoreTransactionResponseDto FromDomain(StoreTransaction param)
    {
        return new()
        {
            Card = param.Card,
            Date = param.Date,
            Value = param.Value,
            StoreId = param.StoreId,
            ClientDocument = param.ClientDocument,
            CreatedAt = param.CreatedAt,
            Id = param.Id,
            TypeMoviment = param.TypeMoviment,
            UpdatedAt = param.UpdatedAt,
        };
    }
}
