using System;

namespace DustInTheWind.HashSafe
{
    internal class CustomConsole
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        internal void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }
    }
}
