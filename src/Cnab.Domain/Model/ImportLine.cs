using System.Text.Json;

namespace Cnab.Domain.Model;

public class ImportLine : Entity
{
    public Guid ImportId { get; protected set; }
    public string Text { get; protected set; } = string.Empty;

    public bool Processed { get; protected set; }
    public string Error { get; protected set; } = string.Empty;

    public ImportLine() : base()
    {

    }

    public void SuccessProcess() =>
        this.Processed = true;

    public void UnprocessLine(string error)
    {
        var errors = new List<string> { error };
        UnprocessLine(errors);
    }

    public void UnprocessLine(IEnumerable<string> errors)
    {
        Error = JsonSerializer.Serialize(errors);
        Processed = false;
    }

    public ImportLine(Guid importId, string text)
    {
        ImportId = importId;
        Text = text;
    }
}
