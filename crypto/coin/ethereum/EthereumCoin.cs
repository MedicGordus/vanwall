using vanwall.crypto.algorithms;
using vanwall.io;

using System;

namespace vanwall.crypto.coin.ethereum
{
    public class EthereumCoin : ACoin<EthereumCoin>, ICoin
    {
        public string GetCommonName => "Ethereum";

        public string GetTicker => "ETH";

        private readonly static string SUBFOLDER = IoCore.AppendOnRootPath("attempts-ethereum");

        public string GetAttemptPath => SUBFOLDER;

        public ByteStage GetByteStage => ByteStage.RawBytes;

        public EthereumCoin() : base(() => IoCore.GetAttemptPath(SUBFOLDER))
        { }
        
        public string GenerateAddressFromPublicKey(byte[] publicKeyBytes)
        {
            if(publicKeyBytes.Length != 65) throw new ArgumentException("publicKey size incorrect, must be exactly 65 bytes as it should be serialized from a secp256k1 eliptical curve public key.");

            byte[] ethereumRelevantBytes = new byte[publicKeyBytes.Length - 1];
            Buffer.BlockCopy(publicKeyBytes, 1, ethereumRelevantBytes, 0 , ethereumRelevantBytes.Length);
            
            string keccakString = CryptoCore.ComputeKeccak256Hash(ethereumRelevantBytes);

            return "0x" + keccakString.Substring(keccakString.Length - 40);
        }
    }
}