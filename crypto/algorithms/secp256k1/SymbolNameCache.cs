using System.Reflection;

namespace vanwall.crypto.algorithms.secp256k1
{
    static class SymbolNameCache<TDelegate>
    {
        public static readonly string SymbolName;

        static SymbolNameCache()
        {
            SymbolName = typeof(TDelegate).GetCustomAttribute<SymbolNameAttribute>().Name;
        }
    }


}