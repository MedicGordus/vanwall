using vanwall.crypto.algorithms;
using vanwall.io;

using System;

namespace vanwall.crypto.coin.bitcoin
{
    public class Bitcoin : ACoin<Bitcoin>, ICoin
    {
        public string GetCommonName => "Bitcoin";

        public string GetTicker => "BTC";

        private readonly static string SUBFOLDER = IoCore.AppendOnRootPath("attempts-bitcoin");

        public string GetAttemptPath => SUBFOLDER;

        public ByteStage GetByteStage => ByteStage.Sha256RipeMd160;

        public Bitcoin() : base(() => IoCore.GetAttemptPath(SUBFOLDER))
        { }

        public string GenerateAddressFromPublicKey(byte[] publicKeyHashed)
        {
            if(publicKeyHashed.Length != 20) throw new ArgumentException("publicKey size incorrect, must be exactly 20 bytes as it should have been hashed like so: Sha256(RipeMd160(input)).");

            // this appends the 0 byte we need for versioning mainnet
            byte[] bitcoinBytes = new byte[publicKeyHashed.Length + 1];
            Buffer.BlockCopy(publicKeyHashed, 0, bitcoinBytes, 1, publicKeyHashed.Length);
            
            // create checksum
            byte[] checkSumBytes = CryptoCore.ComputeDoubleSha256Hash(bitcoinBytes);
            
            // concatenate on the end of the ripemd160
            byte[] addressBytes = new byte[bitcoinBytes.Length + 4];
            Buffer.BlockCopy(bitcoinBytes, 0, addressBytes, 0, bitcoinBytes.Length);
            Buffer.BlockCopy(checkSumBytes, 0, addressBytes, bitcoinBytes.Length, 4);
            
            // Base58 encode
            return Base58.Encode(addressBytes);
        }
    }
}