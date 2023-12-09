using Cnab.Domain.Enum;
using System.Globalization;

namespace Cnab.Domain.Builder;

public class CnabBuilder : Builder
{
    private Model.Cnab cnab = new Model.Cnab();

    private string Value { get; set; } = string.Empty;

    public CnabBuilder SetTypeMoviment()
    {
        //1

        var value = Value.Substring(0, 1);

        var parse = int.TryParse(value, out var intVal);

        if (parse && System.Enum.IsDefined(typeof(TypeMoviment), intVal))
        {
            cnab.SetTypeMoviment((TypeMoviment)intVal);
            return this;
        }

        AddDefaultPropertyError(nameof(TypeMoviment));
        return this;
    }

    public CnabBuilder SetData()
    {
        var value = Value.Substring(1, 8);
        // 2 - 9

        var parse = DateTime.TryParseExact(value, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateVal);

        if (parse)
        {
            cnab.SetData(dateVal);
            return this;
        }

        AddDefaultPropertyError(nameof(cnab.Data));
        return this;
    }

    public CnabBuilder SetValor()
    {
        var value = Value.Substring(9, 10);

        var parse = decimal.TryParse(value, out var decimaVal);

        if (parse)
        {
            var result = decimaVal / 100;
            cnab.SetValor(result);
            return this;
        }

        AddDefaultPropertyError(nameof(cnab.Valor));
        return this;
    }

    public CnabBuilder SetCPF()
    {
        var value = Value.Substring(19, 11);

        if (!string.IsNullOrWhiteSpace(value))
        {
            cnab.SetCpf(value);
            return this;
        }

        AddDefaultPropertyError(nameof(cnab.CPF));
        return this;
    }

    public CnabBuilder SetCartao()
    {
        var value = Value.Substring(30, 12);

        if (!string.IsNullOrWhiteSpace(value))
        {
            cnab.SetCartao(value);
            return this;
        }

        AddDefaultPropertyError(nameof(cnab.Cartao));
        return this;
    }

    public CnabBuilder SetHour()
    {
        // 43 - 48
        var value = Value.Substring(42, 6);

        var parse = int.TryParse(value, out var decimaVal);

        if (parse)
        {
            int horas = decimaVal / 10000;
            int minutos = (decimaVal % 10000) / 100;
            int segundos = decimaVal % 100;

            var result = new TimeSpan(horas, minutos, segundos);

            cnab.SetHour(result);

            return this;
        }

        AddDefaultPropertyError(nameof(cnab.Valor));
        //cnab.Hour = hour;
        return this;
    }
    public CnabBuilder SetDonoLoja()
    {
        // 49 - 62
        var value = Value.Substring(48, 14);

        if (!string.IsNullOrWhiteSpace(value))
        {
            cnab.SetDonoLoja(value);
            return this;
        }

        AddDefaultPropertyError(nameof(cnab.DonoLoja));
        return this;
    }

    public CnabBuilder SetNomeLoja()
    {
        // 63 - 81 - 19
        var value = Value.Substring(62, 18);

        // validar se é vazio

        if (!string.IsNullOrWhiteSpace(value))
        {
            cnab.SetNomeLoja(value);
            return this;
        }

        AddDefaultPropertyError(nameof(cnab.NomeLoja));
        return this;
    }


    public Model.Cnab Build()
    {
        // Aqui, você pode adicionar lógica de validação ou ajustes adicionais antes de construir o objeto final
        return cnab;
    }

    public Model.Cnab Build(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 81)
        {

        }

        Value = value;

        // Aqui, você pode adicionar lógica de validação ou ajustes adicionais antes de construir o objeto final
        return cnab;
    }

    public CnabBuilder(string value)
    {
        if (IsValid(value))
        {

        }

        Value = value;
    }

    public static bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value) && value.Length == 80;
}
