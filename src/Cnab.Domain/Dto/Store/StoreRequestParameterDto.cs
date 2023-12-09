namespace Cnab.Domain.Dto.Store;
public record StoreRequestParameterDto : BaseRequest
{
    public string? Name { get; set; }
    public string? Owner { get; set; }
}
