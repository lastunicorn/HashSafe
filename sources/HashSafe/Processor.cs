// HashSafe
// Copyright (C) 2017 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using DustInTheWind.HashSafe.UI;

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