using System;
using System.IO;

namespace DustInTheWind.HashSafe
{
    internal class HashesFile : IDisposable
    {
        private const string HashesFileName = "hashes";
        private const string HashesNewFileName = "hashes-new";
        private const string HashesBakFileName = "hashes-bak";

        private StreamWriter streamWriter;

        public void Open()
        {
            if (streamWriter != null)
                return;

            if (File.Exists(HashesNewFileName))
                throw new Exception("The previous hashing process was not completed. Delete the temporary hashes-new file and then try again.");

            streamWriter = new StreamWriter(HashesNewFileName);
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
            {
                File.Replace(HashesNewFileName, HashesFileName, HashesBakFileName);
            }
            else
            {
                File.Move(HashesNewFileName, HashesFileName);
                File.Delete(HashesBakFileName);
            }
        }
    }
}