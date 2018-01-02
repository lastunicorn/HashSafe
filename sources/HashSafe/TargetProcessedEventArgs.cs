using System;

namespace DustInTheWind.HashSafe
{
    public class TargetProcessedEventArgs : EventArgs
    {
        public string Target { get; }
        public byte[] Hash { get; }

        public TargetProcessedEventArgs(string target, byte[] hash)
        {
            Target = target;
            Hash = hash;
        }
    }
}