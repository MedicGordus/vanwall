using Fortuna.rand;

using vanwall.io;
using vanwall.crypto.fortuna.entropy;
using vanwall.ux;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace vanwall.crypto.fortuna
{
    public static class FortunaInstance
    {
        internal static Prng Rng;
        
        private static readonly List<EntropyCollector> Entropies = new List<EntropyCollector>();

        internal static readonly string SEED_PATH = IoCore.AppendOnRootPath("fortuna.seed");

        internal static readonly string IO_PATH = IoCore.AppendOnRootPath("fortuna.io");

        private static async Task<Prng> PreparePrngAsync()
        {
            Prng output = Prng.InitializeBlank();

            byte sourceId = 0;

            UxCore.ShareMessage(MessageType.WishToShare, "Adding Keyboard Entropy Source...");
            Entropies.Add(new KeyboardEntropySource(output, sourceId)); sourceId++;

            UxCore.ShareMessage(MessageType.WishToShare, "Adding Mouse Entropy Source...");
            Entropies.Add(new MouseEntropySource(output, sourceId)); sourceId++;

            UxCore.ShareMessage(MessageType.WishToShare, "Adding Internet Entropy Source...");
            Entropies.Add(new InternetEntropySource(output, sourceId, "bing.co.uk")); sourceId++;

            UxCore.ShareMessage(MessageType.WishToShare, "Adding Internet Entropy Source...");
            Entropies.Add(new InternetEntropySource(output, sourceId, "twitter.com")); sourceId++;

            UxCore.ShareMessage(MessageType.WishToShare, "Adding Clock Entropy Source...");
            Entropies.Add(new ClockEntropySource(output, sourceId)); sourceId++;

/* removed because of SSD
            UxCore.ShareMessage(MessageType.WishToShare, "Adding File Entropy Source...");
            Entropies.Add(new FileEntropySource(output, sourceId, IO_PATH)); sourceId++;
*/
            UxCore.ShareMessage(MessageType.WishToShare, "Adding Insecure Random Entropy Source...");
            Entropies.Add(new InsecureRandomEntropySource(output, sourceId)); sourceId++;

            UxCore.ShareMessage(MessageType.WishToShare, "Adding Secure Random Entropy Source...");
            Entropies.Add(new SecureRandomEntropySource(output, sourceId)); sourceId++;

            await Prng.SeedFromFileAsync(SEED_PATH, output).ConfigureAwait(false);

            return output;
        }

        internal static async Task InitializePrngAsync()
        {
            UxCore.ShareMessage(MessageType.WishToShare, "Initializing PRNG, stand by...");
            Rng = await PreparePrngAsync().ConfigureAwait(false);
            UxCore.ShareMessage(MessageType.WishToShare, "PRNG initialized.");

            UxCore.ShareMessage(MessageType.WishToShare, "Waiting for entropy to collect...");
            do
            {
                await Task.Delay(100);
            } while (!Rng.ReadyToGenerateRandomData());
            UxCore.ShareMessage(MessageType.WishToShare, "Ready to generate random data");
        }
    }
}