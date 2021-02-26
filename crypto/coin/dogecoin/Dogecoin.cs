using vanwall.crypto.algorithms;
using vanwall.io;

using System;

namespace vanwall.crypto.coin.dogecoin
{
    public class Dogecoin : ACoin<Dogecoin>, ICoin
    {
        public string GetCommonName => "Dogecoin";

        public string GetTicker => "DOGE";
        
        private readonly static string SUBFOLDER = IoCore.AppendOnRootPath("attempts-dogecoin");

        public string GetAttemptPath => SUBFOLDER;

        public ByteStage GetByteStage => ByteStage.Sha256RipeMd160;

        public Dogecoin() : base(() => IoCore.GetAttemptPath(SUBFOLDER))
        { }

        public string GenerateAddressFromPublicKey(byte[] publicKeyHashed)
        {
            if(publicKeyHashed.Length != 20) throw new ArgumentException("publicKey size incorrect, must be exactly 20 bytes as it should have been hashed like so: Sha256(RipeMd160(input)).");

            // this appends the 30 byte we need for versioning mainnet
            byte[] dogecoinBytes = new byte[publicKeyHashed.Length + 1];
            dogecoinBytes[0] = 30;
            Buffer.BlockCopy(publicKeyHashed, 0, dogecoinBytes, 1, publicKeyHashed.Length);
            
            // create checksum
            byte[] checkSumBytes = CryptoCore.ComputeDoubleSha256Hash(dogecoinBytes);
            
            // concatenate on the end of the ripemd160
            byte[] addressBytes = new byte[dogecoinBytes.Length + 4];
            Buffer.BlockCopy(dogecoinBytes, 0, addressBytes, 0, dogecoinBytes.Length);
            Buffer.BlockCopy(checkSumBytes, 0, addressBytes, dogecoinBytes.Length, 4);
            
            // Base58 encode
            return Base58.Encode(addressBytes);
        }
    }
}