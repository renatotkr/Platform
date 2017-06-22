using System.Threading.Tasks;

namespace Carbon.Kms
{
    public interface IKeyProtectorProvider
    {
        ValueTask<IKeyProtector> GetAsync(long id);
    }
}

// Does the KMS device generate better key source material than .NET?