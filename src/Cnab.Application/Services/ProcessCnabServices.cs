using Cnab.Application.UoW;
using Cnab.Domain.Builder;
using Cnab.Domain.Model;
using Cnab.Domain.Services;
using Microsoft.Extensions.Configuration;

namespace Cnab.Application.Services;
public class ProcessCnabServices : IProcessCnabServices
{
  private readonly UnitOfWork _unitOfWork;
  private readonly IConfiguration _configuration;

  public ProcessCnabServices(UnitOfWork unitOfWork, IConfiguration configuration)
  {
    _unitOfWork = unitOfWork;
    _configuration = configuration;
  }

  public async Task Execute()
  {
    var ids = new List<Guid>();

    var count = int.TryParse(_configuration["GetItemsToProcess"], out var result) ? result : 5;

    var items = await _unitOfWork.ImportRepository.GetUnprocessedImportLines(2);

    foreach (var item in items)
    {
      ids.Add(item.ImportId);
      if (CnabBuilder.IsValid(item.Text))
      {
        var builder = BuildCnab(item.Text);

        if (!builder.IsValid())
        {
          item.UnprocessLine(builder.Errors);
          await ProcessLine(item);
          continue;
        }

        var cnab = builder.Build();

        var transaction = new StoreTransaction(cnab.TypeMoviment, cnab.CPF, cnab.Cartao, cnab.Data.Add(cnab.Hour), cnab.Valor);

        await BuildStore(cnab.NomeLoja, cnab.DonoLoja, transaction);

        item.SuccessProcess();
        await ProcessLine(item);
      }
      else
      {
        item.UnprocessLine("Cannot possible process this line, may the formart is Isvalid");
        await ProcessLine(item);
      }
    }

    ids.ForEach(async c => await _unitOfWork.ImportRepository.ProcessImport(c));

    _unitOfWork.Commit();
  }

  private CnabBuilder BuildCnab(string item)
  {
    var builder = new CnabBuilder(item)
            .SetTypeMoviment()
            .SetData()
            .SetValor()
            .SetCPF()
            .SetCartao()
            .SetHour()
            .SetNomeLoja()
            .SetDonoLoja();

    return builder;
  }

  private async Task BuildStore(string storeName, string storeOwner, StoreTransaction transaction)
  {
    var existStore = await _unitOfWork.StoreRepository.GetByNameAndOwner(storeName, storeOwner);

    if (existStore == Guid.Empty)
    {
      var store = new Store(storeName, storeOwner, transaction.GetValue());
      transaction.SetStoreId(store.Id);

      await _unitOfWork.StoreRepository.AddWithTransaction(store, transaction);
      return;
    }

    transaction.SetStoreId(existStore);
    await _unitOfWork.StoreRepository.UpdateWithTransaction(existStore, transaction.GetValue(), transaction);
  }

  private async Task ProcessLine(ImportLine param) =>
      await _unitOfWork.ImportRepository.ProcessLine(param);
}
