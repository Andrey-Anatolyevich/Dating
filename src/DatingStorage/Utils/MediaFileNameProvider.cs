using System;
using System.Collections.Generic;
using System.IO;

namespace DatingStorage.Utils
{
    internal class MediaFileNameProvider
    {
        public MediaFileNameProvider(string filesRootDir)
        {
            _rndStringProvider = new RandomStringProvider();

            _filesRootDir = filesRootDir;
            _getNewNameLock = new object();
            _reservedKeys = new HashSet<string>();
        }

        private const int _nameCharsCount = 6;
        private RandomStringProvider _rndStringProvider;

        private string _filesRootDir;
        private object _getNewNameLock;
        private HashSet<string> _reservedKeys;

        internal void Unlock(string key)
        {
            _reservedKeys.Remove(key);
        }

        internal FilePathInfo GetNewFilePathInfo(long userId, string fileExtension, out string reservationKey)
        {
            lock (_getNewNameLock)
            {
                var fileExists = true;
                var pathParts = new List<string>();

                var utcNow = DateTime.UtcNow;
                pathParts.Add(utcNow.Year.ToString());
                pathParts.Add(utcNow.Month.ToString());
                pathParts.Add(utcNow.Day.ToString());
                pathParts.Add(userId.ToString());

                var dirPathRelative = Path.Combine(pathParts.ToArray());
                string filePathRelative;
                string fileNameFull;

                do
                {
                    var fileName = _rndStringProvider.GetRndString(length: _nameCharsCount);
                    fileNameFull = $"{fileName}{fileExtension}";
                    filePathRelative = Path.Combine(dirPathRelative, fileNameFull);
                    if (_reservedKeys.Contains(filePathRelative))
                        continue;

                    var filePathAbsolute = Path.Combine(_filesRootDir, filePathRelative);
                    fileExists = File.Exists(filePathAbsolute);
                } while (fileExists);

                _reservedKeys.Add(filePathRelative);
                reservationKey = filePathRelative;

                var result = new FilePathInfo()
                {
                    FileName = fileNameFull,
                    RelativePathParts = pathParts.ToArray()
                };

                return result;
            }
        }

    }
}
