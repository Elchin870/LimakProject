using System.Security.Cryptography;

namespace Limak.Application.Helpers;

public class CustomerCodeGenerator
{
    public static string Generate() => RandomNumberGenerator.GetInt32(10000000, 100000000).ToString();
}
