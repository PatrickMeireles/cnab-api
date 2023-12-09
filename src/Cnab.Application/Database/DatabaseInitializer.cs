using Dapper;
using SQLitePCL;
using System.Data;

namespace Cnab.Application.Database;

public class DatabaseInitializer
{
    private readonly IDbConnection _dbConnection;

    public DatabaseInitializer(IDbConnection dbConnection)
    {
        Batteries.Init();
        _dbConnection = dbConnection;
    }

    public void Initialize()
    {

        _dbConnection.Open();

        // Create tables if not exist
        _dbConnection.Execute(@"
           -- Create Store Table
                CREATE TABLE IF NOT EXISTS Store (
                    Id UUID PRIMARY KEY,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT NULL,
                    Name TEXT NOT NULL,
                    Owner TEXT NOT NULL,
                    Balance REAL NOT NULL
                );

                -- Create StoreTransaction Table
                CREATE TABLE IF NOT EXISTS StoreTransaction (
                    Id UUID PRIMARY KEY,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT NULL,
                    TypeMoviment TEXT NOT NULL,
                    ClientDocument TEXT NOT NULL,
                    Card TEXT NOT NULL,
                    Date TEXT NOT NULL,
                    Value REAL NOT NULL,
                    StoreId UUID NOT NULL,
                    FOREIGN KEY (StoreId) REFERENCES Store(Id)
                );

                -- Create Import Table
                CREATE TABLE IF NOT EXISTS Import (
                    Id UUID PRIMARY KEY,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT NULL,
                    FileName TEXT NOT NULL,
                    Status TEXT NOT NULL
                );

                -- Create ImportLine Table
                CREATE TABLE IF NOT EXISTS ImportLine (
                    Id UUID PRIMARY KEY,
                    CreatedAt TEXT NOT NULL,
                    UpdatedAt TEXT NULL,
                    ImportId UUID NOT NULL,
                    Text TEXT NOT NULL,
                    Processed INTEGER NOT NULL CHECK (Processed IN (0, 1)),
                    Error TEXT NOT NULL,
                    FOREIGN KEY (ImportId) REFERENCES Import(Id)
                );

        ");

        SqlMapper.AddTypeHandler(new GuidTypeHandler());

        _dbConnection.Close();
    }


    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString();
        }

        public override Guid Parse(object value)
        {
            var guid = Guid.TryParse(value.ToString(), out var guidResult);

            return guid ? guidResult : Guid.Empty;
        }
    }
}

