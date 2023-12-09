using Cnab.Domain.Enum;

namespace Cnab.Domain.Model;

public class Cnab
{
    public TypeMoviment TypeMoviment { get; protected set; }
    public DateTime Data { get; protected set; }
    public decimal Valor { get; protected set; }
    public string CPF { get; protected set; } = string.Empty;
    public string Cartao { get; protected set; } = string.Empty;
    public TimeSpan Hour { get; protected set; }
    public string NomeLoja { get; protected set; } = string.Empty;
    public string DonoLoja { get; protected set; } = string.Empty;

    public void SetTypeMoviment(TypeMoviment value) =>
        TypeMoviment = value;

    public void SetData(DateTime value) =>
        Data = value;

    public void SetValor(decimal value) =>
        Valor = value;

    public void SetCpf(string value) =>
        CPF = value;

    public void SetCartao(string value) =>
        Cartao = value;

    public void SetHour(TimeSpan value) =>
        Hour = value;

    public void SetNomeLoja(string value) =>
        NomeLoja = value.TrimEnd();

    public void SetDonoLoja(string value) =>
        DonoLoja = value.TrimEnd();
}
