using System;

namespace vanwall.crypto.algorithms.secp256k1
{
    class SymbolNameAttribute : Attribute
    {
        public readonly string Name;

        public SymbolNameAttribute(string name)
        {
            Name = name;
        }
    }


}