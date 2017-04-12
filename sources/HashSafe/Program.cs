using System;
using System.Security.Cryptography;

namespace DustInTheWind.HashSafe
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    TargetsProvider targetsProvider = new TargetsProvider();
                    Display display = new Display();

                    Processor processor = new Processor(targetsProvider, md5, display);
                    processor.Execute();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            CustomConsole.Pause();
        }
    }
}
