using Cnab.Domain.Model;
using Cnab.Domain.Repository;
using Dapper;
using System.Data;

namespace Cnab.Application.Repository;
public class StoreRepository : IStoreRepository
{
    private readonly IDbConnection _dbConnection;

    public StoreRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task AddWithTransaction(Store param, StoreTransaction transaction)
    {
        var sqlStore = "INSERT INTO Store (Id, CreatedAt, Name, Owner, Balance) " +
            "VALUES (@Id, @CreatedAt, @Name, @Owner, @Balance)";

        await _dbConnection.ExecuteAsync(
              sqlStore, param
           );

        await AddTransaction(transaction);
    }

    public async Task<int> Count(string? name, string? owner)
    {
        var parameters = new DynamicParameters();
        string whereClause = BuildWhereClause(owner, name, parameters);

        var sql = $"SELECT COUNT(*) FROM Store {whereClause}";

        var rowCount = await _dbConnection.ExecuteScalarAsync<int>(sql,
               parameters
           );

        return rowCount;
    }

    public async Task<Guid> GetByNameAndOwner(string name, string owner)
    {
        string sql = "SELECT Id FROM Store WHERE Name = @Name AND Owner = @Owner";

        var result = await _dbConnection.QueryFirstOrDefaultAsync<Guid>(
               sql,
               new { Name = name, Owner = owner }
           );

        return result;
    }

    public async Task UpdateWithTransaction(Guid id, decimal balance, StoreTransaction transaction)
    {
        var currentBalance = _dbConnection.ExecuteScalar<decimal>("SELECT Balance FROM Store WHERE Id = @StoreId", new { StoreId = id });

        var newBalance = currentBalance + balance;

        var sql = "UPDATE Store SET Balance = @NewBalance, UpdatedAt = @UpdatedAt WHERE Id = @StoreId";

        _dbConnection.Execute(sql, new { NewBalance = newBalance, StoreId = id, UpdatedAt = DateTime.Now });

        await AddTransaction(transaction);
    }

    private async Task AddTransaction(StoreTransaction transaction)
    {
        var sql = "INSERT INTO StoreTransaction (Id, CreatedAt, TypeMoviment, ClientDocument, Card, Date, Value, StoreId) " +
            "VALUES (@Id, @CreatedAt, @TypeMoviment, @ClientDocument, @Card, @Date, @Value, @StoreId)";

        await _dbConnection.ExecuteAsync(sql, transaction);
    }

    static string BuildWhereClause(string? owner, string? name, DynamicParameters parameters)
    {
        List<string> conditions = new List<string>();

        if (!string.IsNullOrEmpty(owner))
        {
            conditions.Add("Owner LIKE @Owner");
            parameters.Add("@Owner", $"%{owner}%");
        }

        if (!string.IsNullOrEmpty(name))
        {
            conditions.Add("Name LIKE @Name");
            parameters.Add("@Name", $"%{name}%");
        }

        if (conditions.Count > 0)
        {
            return "WHERE " + string.Join(" AND ", conditions);
        }

        return string.Empty;
    }

    public async Task<IEnumerable<StoreTransaction>> GetTransactions(Guid storeId)
    {
        var sql = $"SELECT * FROM StoreTransaction WHERE StoreId = @StoreId";

        var result = await _dbConnection.QueryAsync<StoreTransaction>(
        sql,
           new { StoreId = storeId }
       );

        return result;
    }

    public async Task<IEnumerable<Store>> GetPaginated(int page = 1, int size = 10, string? name = "", string? owner = "")
    {
        int offset = (page - 1) * size;

        var parameters = new DynamicParameters();
        string whereClause = BuildWhereClause(owner, name, parameters);

        var sql = $"SELECT * FROM Store {whereClause} LIMIT @PageSize OFFSET @Offset";

        var result = await _dbConnection.QueryAsync<Store>(
            sql,
            new { PageSize = size, Offset = offset }
        );

        return result;
    }

    public async Task<Store?> GetById(Guid id)
    {
        string sql = $"SELECT * FROM Store WHERE Id = @Id LIMIT 1";

        var result = await _dbConnection.QueryFirstOrDefaultAsync<Store>(sql, new { Id = id });

        return result;
    }
}
