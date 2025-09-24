using System.Security.Cryptography;
using LedgerManager.Application.Interfaces.Services;

namespace LedgerManager.Infrastructure.Services;

public class AccountNumberGenerator : IAccountNumberGenerator
{
    private const long Mod = 10_000_000_000L;

    public string Generate()
    {
        var guid = Guid.NewGuid();

        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(guid.ToByteArray());

        ulong part = BitConverter.ToUInt64(hash, 0);
        long number = (long)(part % Mod);

        return number.ToString("D10");
    }
}