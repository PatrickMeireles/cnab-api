namespace Cnab.Domain.Dto;
public record BaseRequest
{
    public int Page { get; init; } = 1;
    public int Size { get; init; } = 50;
}
