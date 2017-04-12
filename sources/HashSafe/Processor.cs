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
        private readonly Display display;
        private readonly HashAlgorithm hashAlgorithm;
        private readonly HashesFile hashesFile;

        public Processor(TargetsProvider targetsProvider, HashAlgorithm hashAlgorithm, Display display)
        {
            if (targetsProvider == null) throw new ArgumentNullException(nameof(targetsProvider));
            if (hashAlgorithm == null) throw new ArgumentNullException(nameof(hashAlgorithm));
            if (display == null) throw new ArgumentNullException(nameof(display));

            this.targetsProvider = targetsProvider;
            this.hashAlgorithm = hashAlgorithm;
            this.display = display;

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
            display.Summary(sw.Elapsed);
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
            byte[] hash;

            try
            {
                using (Stream stream = File.OpenRead(fileName))
                    hash = hashAlgorithm.ComputeHash(stream);
            }
            catch (Exception ex)
            {
                hashesFile.AddErrorTarget(fileName, ex);
                return;
            }

            display.DisplayFileHash(fileName, hash);
            hashesFile.AddHash(fileName, hash);
        }

        private void ProcessDirectory(string directoryPath)
        {
            string[] filenames = Directory.GetFileSystemEntries(directoryPath);

            foreach (string filename in filenames)
                ProcessTarget(filename);
        }

        private void ProcessInexistentTarget(string target)
        {
            display.DisplayTargetNotFound(target);
            hashesFile.AddMissingTarget(target);
        }
    }
}