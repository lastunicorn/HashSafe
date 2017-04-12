using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace DustInTheWind.HashSafe
{
    internal class Processor
    {
        private readonly TargetsProvider targetsProvider;
        private readonly CustomConsole console;
        private readonly HashAlgorithm hashAlgorithm;
        private readonly HashesFile hashesFile;

        public Processor(TargetsProvider targetsProvider, CustomConsole console, HashAlgorithm hashAlgorithm)
        {
            if (targetsProvider == null) throw new ArgumentNullException(nameof(targetsProvider));
            if (console == null) throw new ArgumentNullException(nameof(console));
            if (hashAlgorithm == null) throw new ArgumentNullException(nameof(hashAlgorithm));

            this.targetsProvider = targetsProvider;
            this.console = console;
            this.hashAlgorithm = hashAlgorithm;

            hashesFile = new HashesFile();
        }

        public void Execute()
        {
            Stopwatch sw = Stopwatch.StartNew();

            hashesFile.Open();

            IEnumerable<string> targets = targetsProvider.GetTargets();

            foreach (string filename in targets)
                ProcessTarget(filename);

            hashesFile.Close();

            sw.Stop();
            console.WriteLine($" Elapsed time: {sw.Elapsed}");
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

        private void ProcessFile(string fileName)
        {
            try
            {
                using (Stream stream = File.OpenRead(fileName))
                {
                    byte[] hash = hashAlgorithm.ComputeHash(stream);
                    DisplayFileHash(fileName, hash);
                    hashesFile.AddHash(fileName, hash);
                }
            }
            catch (Exception ex)
            {
                hashesFile.AddErrorTarget(fileName, ex);
            }
        }

        private void DisplayFileHash(string fileName, byte[] hash)
        {
            string hex = BitConverter.ToString(hash);
            console.WriteLine("{0} - {1}", hex, fileName);
        }

        private void ProcessDirectory(string directoryPath)
        {
            string[] filenames = Directory.GetFileSystemEntries(directoryPath);

            foreach (string filename in filenames)
                ProcessTarget(filename);
        }

        private void ProcessInexistentTarget(string target)
        {
            console.WriteLine($"target not found: {target}");
            hashesFile.AddMissingTarget(target);
        }
    }
}