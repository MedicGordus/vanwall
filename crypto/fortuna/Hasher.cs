using System.Security.Cryptography;

namespace vanwall.crypto.fortuna
{
    internal class Hasher
    {
        protected SHA256 Hash;

        internal Hasher()
        {
            Hash = SHA256.Create();
        }

        internal byte[] ComputeSha2Hash(byte[] buffer)
        {
            return Hash.ComputeHash(buffer);
        }
    }
}
