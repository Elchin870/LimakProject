namespace Limak.Application.Helpers;

public static class OrderNumberGenerator
{
    public static string Generate()
    {
        var random = Random.Shared.Next(1000, 9999);
        var date = DateTime.UtcNow.ToString("yyMMdd");

        return $"LMK{date}{random}";
    }
}
