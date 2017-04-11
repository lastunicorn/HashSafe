using System.Collections.Generic;

namespace DustInTheWind.HashSafe
{
    internal class TargetsProvider
    {
        public IEnumerable<string> GetTargets()
        {
            return new[]
            {
                @"d:\temp\CollectionDiagrams.zip",
                @"d:\temp\Debug"
            };
        }
    }
}