using Cnab.Domain.Repository;
using System.Data;

namespace Cnab.Application.UoW;

public class UnitOfWork : IDisposable
{
    private readonly IDbConnection _dbConnection;
    private IDbTransaction _dbTransaction;
    private bool _disposed;

    public UnitOfWork(IDbConnection dbConnection,
                      IImportRepository importRepository,
                      IStoreRepository storeRepository
    )
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        _importRepository = importRepository ?? throw new ArgumentNullException(nameof(importRepository));
        _storeRepository = storeRepository ?? throw new ArgumentNullException(nameof(storeRepository));

        _dbConnection.Open();

        _dbTransaction = _dbConnection.BeginTransaction();
    }

    private IImportRepository _importRepository;
    private IStoreRepository _storeRepository;

    public IImportRepository ImportRepository => _importRepository;
    public IStoreRepository StoreRepository => _storeRepository;

    public void Commit()
    {
        try
        {
            _dbTransaction.Commit();
        }
        catch
        {
            _dbTransaction.Rollback();
            throw;
        }
        finally
        {
            _dbTransaction.Dispose();
            _dbTransaction = _dbConnection.BeginTransaction();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (_dbTransaction != null)
                {
                    _dbTransaction.Dispose();
                }

                if (_dbConnection != null)
                {
                    _dbConnection.Close();
                    _dbConnection.Dispose();
                }
            }

            _disposed = true;
        }
    }
}
