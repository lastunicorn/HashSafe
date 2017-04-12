using System;

namespace DustInTheWind.HashSafe
{
    internal class Display
    {
        public void DisplayFileHash(string fileName, byte[] hash)
        {
            string hex = BitConverter.ToString(hash).Replace("-", string.Empty);
            CustomConsole.WriteEmphasies(fileName);
            CustomConsole.Write(" - ");
            CustomConsole.WriteLine(hex);
        }

        public void DisplayTargetNotFound(string target)
        {
            CustomConsole.WriteError($"target not found: {target}");
        }

        public void Summary(TimeSpan elapsedTime)
        {
            CustomConsole.WriteLine();
            CustomConsole.WriteEmphasies(" Elapsed time: ");
            CustomConsole.WriteLine(elapsedTime.ToString());
        }
    }
}