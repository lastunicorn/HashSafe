using System.Collections.Generic;
using System.IO;

namespace DustInTheWind.HashSafe
{
    internal class TargetsProvider
    {
        public IEnumerable<string> GetTargets()
        {
            return File.ReadAllLines("proj");
        }
    }
}