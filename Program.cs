
using vanwall.crypto.coin;
using vanwall.crypto.fortuna;
using vanwall.io;
using vanwall.ux;


using System;
using System.Threading;
using System.Threading.Tasks;

namespace vanwall
{
    class Program
    {
        const int DEFAULT_PARALLEL_THREADS = 10;
        static async Task Main(string[] args)
        {
            UxCore.ShareMessage(MessageType.WishToShare, "Hello, vanwall starting up...");
            
            if(!EnsurePathExists())
            {
                Terminate();
            }

            await FortunaInstance.InitializePrngAsync();
            

            Task t = Task.Run(() => CoinCore.StartGenerationAsync(DEFAULT_PARALLEL_THREADS));
            UxCore.ShareMessage(
                MessageType.WishToShare,
                string.Format(
                    "Keys are now generating on {0} parallel threads.",
                    DEFAULT_PARALLEL_THREADS
                )
            );

/* for debugging we give it 30 seconds
            // UxCore.WaitForExit();
            await Task.Delay(1000 * 30);
*/
            UxCore.WaitForExit();

            CoinCore.StopGeneration();

            UxCore.ShareMessage(MessageType.ResponseAsRequested, "Shutting down, please wait...");

            await t;

            Terminate();
        }

        static void Terminate()
        {
            UxCore.ShareMessage(MessageType.WishToShare, "Vanwall exiting, goodbye!");
        }

        static bool EnsurePathExists()
        {
            if(!IoCore.EnsureAllPathsExist())
            {
                return false;
            }

            if(!CoinCore.EnsureAllPathsExist())
            {
                return false;
            }

            return true;
        }
    }
}
