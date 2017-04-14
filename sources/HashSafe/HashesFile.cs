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
using System.IO;

namespace DustInTheWind.HashSafe
{
    internal class HashesFile : IDisposable
    {
        private const string HashesFileName = "hashes";
        private const string HashesTmpFileName = "hashes-tmp";
        private const string HashesBakFileName = "hashes-bak";

        private StreamWriter streamWriter;

        public void Open()
        {
            if (streamWriter != null)
                return;

            if (File.Exists(HashesTmpFileName))
                throw new Exception($"The previous hashing process was not completed. Delete the temporary {HashesTmpFileName} file and then try again.");

            streamWriter = new StreamWriter(HashesTmpFileName);
        }

        public void AddHash(string fileName, byte[] hash)
        {
            if (streamWriter == null)
                Open();

            string hex = BitConverter.ToString(hash).Replace("-", string.Empty);
            string line = $"{fileName}|{hex}";

            streamWriter.WriteLine(line);
        }

        public void AddMissingTarget(string fileName)
        {
            if (streamWriter == null)
                Open();

            string line = $"{fileName}|Missing";

            streamWriter.WriteLine(line);
        }

        public void AddErrorTarget(string fileName, Exception exception)
        {
            if (streamWriter == null)
                Open();

            string line = $"{fileName}|{exception.Message}";

            streamWriter.WriteLine(line);
        }

        public void Dispose()
        {
            streamWriter?.Dispose();
        }

        public void Close()
        {
            if (streamWriter == null)
                throw new Exception("No file is opened");

            streamWriter.Close();
            streamWriter.Dispose();
            streamWriter = null;

            if (File.Exists(HashesFileName))
                File.Replace(HashesTmpFileName, HashesFileName, HashesBakFileName);
            else
                File.Move(HashesTmpFileName, HashesFileName);
        }
    }
}