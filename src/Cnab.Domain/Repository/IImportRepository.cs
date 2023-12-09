using Cnab.Domain.Model;

namespace Cnab.Domain.Repository;

public interface IImportRepository
{
    Task Add(Import data);
    Task<bool> ExistByName(string name);
    Task<IEnumerable<ImportLine>> GetUnprocessedImportLines(int limit);
    Task ProcessLine(ImportLine data);
    Task ProcessImport(Guid id);
    Task<Import?> GetById(Guid id);
}
