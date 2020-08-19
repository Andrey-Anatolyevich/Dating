using System;
using System.Linq;

namespace DatingStorage.Utils
{
    public class FilePathInfo
    {
        public string[] RelativePathParts;
        public string FileName;

        internal string[] GetAllParts()
        {
            var result = RelativePathParts.ToList();
            result.Add(FileName);
            return result.ToArray();
        }
    }
}
