using Cnab.Domain.Enum;

namespace Cnab.Domain.Model;

public class Import : Entity
{
    public Import(Guid id, string fileName, ImportProccessStatus status, ICollection<ImportLine> lines) : base(id)
    {
        FileName = fileName;
        Status = status;
        Lines = lines;
    }

    public string FileName { get; protected set; } = string.Empty;
    public ImportProccessStatus Status { get; protected set; }
    public ICollection<ImportLine> Lines { get; protected set; } = new List<ImportLine>();
}
