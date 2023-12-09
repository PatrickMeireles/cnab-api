namespace Cnab.Domain.Dto;

public record BaseResponse
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public record BasePaginated<T>
{
    public T[] Data { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public int Total { get; set; }

    public BasePaginated(T[] data, int page, int size, int total)
    {
        Data = data;
        Page = page;
        Size = size;
        Total = total;
    }

    public static BasePaginated<T> BuildDefault(int page, int size)
    {
        return new BasePaginated<T>(Array.Empty<T>(), page, size, 0);
    }
}