namespace Limak.Application.Helpers;

public static class TrackingNumberGenerator
{
    public static string Generate()
    {
        var random = Random.Shared.Next(100000, 999999);
        return random.ToString();
    }
}
