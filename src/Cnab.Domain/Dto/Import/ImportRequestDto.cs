namespace Cnab.Domain.Dto.Import;

public record ImportRequestDto(Guid Id, string FileName, IEnumerable<string> Lines);

public static class ImportAdapter
{
    public static Model.Import ToDomain(ImportRequestDto param)
    {
        return new Model.Import(param.Id,
            param.FileName,
            Enum.ImportProccessStatus.Received,
            param.Lines.Select(c => ToDomainLines(param.Id, c)).ToList());
    }

    public static Model.ImportLine ToDomainLines(Guid ImportId, string line)
    {
        return new Model.ImportLine(ImportId, line);
    }
}