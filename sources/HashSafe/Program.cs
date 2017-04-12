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
                    CustomConsole console = new CustomConsole();

                    Processor processor = new Processor(targetsProvider, console, md5);
                    processor.Execute();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            DisplayPause();
        }

        private static void DisplayPause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }
    }
}
