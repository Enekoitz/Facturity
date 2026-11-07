namespace Facturity.Modules.SharedKernel.Infra;

public static class DbUtilities
{
    public static string BuildPostgresConcatExpression(string separator = " ", params string?[] columnNames)
    {
        var coalesced = columnNames.Where(c => c != null).Select(c => $"COALESCE({c}, '')");
        return string.Join($" || '{separator}' || ", coalesced);
    }
}