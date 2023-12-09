using Cnab.Domain.Enum;

namespace Cnab.Domain.Model;

public class StoreTransaction : Entity
{
    public TypeMoviment TypeMoviment { get; init; }
    public string ClientDocument { get; init; } = string.Empty;
    public string Card { get; init; } = string.Empty;
    public DateTime Date { get; init; }
    public decimal Value { get; init; }
    public Guid StoreId { get; protected set; }

    public StoreTransaction()
    {

    }

    public StoreTransaction(TypeMoviment typeMoviment, string clientDocument, string card, DateTime date, decimal value)
    {
        TypeMoviment = typeMoviment;
        ClientDocument = clientDocument;
        Card = card;
        Date = date;
        Value = value;
    }

    public void SetStoreId(Guid storeId) =>
        StoreId = storeId;

    public decimal GetValue()
    {
        if (IsDeposit(TypeMoviment))
            return Value;

        return -Value;
    }

    private bool IsDeposit(TypeMoviment typeMoviment) =>
        TypeDeposits.Contains(typeMoviment);

    private TypeMoviment[] TypeDeposits =>
        new TypeMoviment[]
        {
            TypeMoviment.Debito,
            TypeMoviment.Credito,
            TypeMoviment.RecebimentoEmprestimo,
            TypeMoviment.Vendas,
            TypeMoviment.RecebimentoTED,
            TypeMoviment.RecebimentoDOC
        };
}
