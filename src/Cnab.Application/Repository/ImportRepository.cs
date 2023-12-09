using Cnab.Domain.Model;
using Cnab.Domain.Repository;
using Dapper;
using System.Data;

namespace Cnab.Application.Repository;

public class ImportRepository : IImportRepository
{
    private readonly IDbConnection _dbConnection;

    public ImportRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task Add(Import data)
    {
        string sql = "INSERT INTO Import (Id, CreatedAt, UpdatedAt, FileName, Status) " +
                     "VALUES (@Id, @CreatedAt, @UpdatedAt, @FileName, @Status)";

        await _dbConnection.ExecuteAsync(sql, data);

        foreach (var importLine in data.Lines)
        {
            string lineSql = "INSERT INTO ImportLine (Id, CreatedAt, UpdatedAt, ImportId, Text, Processed, Error) " +
                             "VALUES (@Id, @CreatedAt, @UpdatedAt, @ImportId, @Text, @Processed, @Error)";

            await _dbConnection.ExecuteAsync(lineSql, importLine);
        }
    }

    public async Task<bool> ExistByName(string name)
    {
        string sql = "SELECT COUNT(*) FROM Import WHERE FileName = @FileName";

        int count = await _dbConnection.ExecuteScalarAsync<int>(sql, new { FileName = name });

        return count > 0;
    }

    public async Task<Import?> GetById(Guid id)
    {
        string sql = "SELECT * FROM Import WHERE Id = @Id";

        var result = await _dbConnection.QueryFirstOrDefaultAsync<Import>(sql, new { Id = id });

        return result;
    }

    public async Task<IEnumerable<ImportLine>> GetUnprocessedImportLines(int limit)
    {
        string sql = "SELECT * FROM ImportLine WHERE Processed = 0 LIMIT (@Quantity)";

        var result = await _dbConnection.QueryAsync<ImportLine>(sql, new { Quantity = limit });

        return result;
    }

    public async Task ProcessImport(Guid id)
    {
        string sql = @"
                UPDATE Import
                SET Status = CASE
                    WHEN NOT EXISTS (
                        SELECT 1
                        FROM ImportLine
                        WHERE ImportLine.ImportId = Import.Id
                              AND ImportLine.Processed = 0
                              AND ImportLine.ImportId = @ImportId
                    ) THEN '@Finished'
                    ELSE '@Started'
                END,
                UpdatedAt = @UpdatedAt
                WHERE Import.Id = @ImportId;"
        ;

        await _dbConnection.ExecuteAsync(sql, new { ImportId = id, UpdatedAt = DateTime.Now, Domain.Enum.ImportProccessStatus.Finished, Domain.Enum.ImportProccessStatus.Started });

    }

    public async Task ProcessLine(ImportLine data)
    {
        string sql = @"UPDATE ImportLine
                SET Processed = @NewProcessedValue, UpdatedAt = @NewUpdatedAtValue, Error = @NewErrorValue
                WHERE Id = @ImportLineId";

        await _dbConnection.ExecuteAsync(sql,
                new
                {
                    NewProcessedValue = data.Processed,
                    NewUpdatedAtValue = data.UpdatedAt,
                    NewErrorValue = data.Error,
                    ImportLineId = data.Id
                }
            );
    }
}
