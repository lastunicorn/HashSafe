using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace DustInTheWind.HashSafe
{
    internal class Processor
    {
        private readonly TargetsProvider targetsProvider;
        private readonly CustomConsole console;
        private readonly HashAlgorithm hashAlgorithm;

        public Processor(TargetsProvider targetsProvider, CustomConsole console, HashAlgorithm hashAlgorithm)
        {
            if (targetsProvider == null) throw new ArgumentNullException(nameof(targetsProvider));
            if (console == null) throw new ArgumentNullException(nameof(console));
            if (hashAlgorithm == null) throw new ArgumentNullException(nameof(hashAlgorithm));

            this.targetsProvider = targetsProvider;
            this.console = console;
            this.hashAlgorithm = hashAlgorithm;
        }

        public void Execute()
        {
            IEnumerable<string> targets = targetsProvider.GetTargets();

            foreach (string filename in targets)
                ProcessTarget(filename);
        }

        private void ProcessTarget(string target)
        {
            if (File.Exists(target))
                ProcessFile(target);
            else if (Directory.Exists(target))
                ProcessDirectory(target);
            else
                ProcessInexistentTarget(target);
        }

        private void ProcessFile(string filename)
        {
            using (Stream stream = File.OpenRead(filename))
            {
                byte[] hash = hashAlgorithm.ComputeHash(stream);
                DisplayFileHash(filename, hash);
            }
        }

        private void DisplayFileHash(string filename, byte[] hash)
        {
            string hex = BitConverter.ToString(hash);
            console.WriteLine("{0} - {1}", hex, filename);
        }

        private void ProcessDirectory(string directoryPath)
        {
            string[] filenames = Directory.GetFiles(directoryPath);

            foreach (string filename in filenames)
                ProcessTarget(filename);
        }

        private void ProcessInexistentTarget(string target)
        {
            console.WriteLine("target not found: {0}", target);
        }
    }
}