
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
        static async Task Main(string[] args)
        {
            UxCore.ShareMessage(MessageType.WishToShare, "Hello, vanwall starting up...");
            
            if(!EnsurePathExists())
            {
                Terminate();
            }

            await FortunaInstance.InitializePrngAsync();
            

            Task t = Task.Run(() => CoinCore.StartGenerationAsync(8));

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
