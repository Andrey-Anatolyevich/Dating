using System;
using System.Text;

namespace DatingStorage.Utils
{
    internal class RandomStringProvider
    {
        public RandomStringProvider()
        {
            _fileNameRandom = new Random();
            _rndMaxIndex = _allwedNameChars.Length - 1;
        }


        private const string _allwedNameChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private Random _fileNameRandom;
        private int _rndMaxIndex;


        internal string GetRndString(int length)
        {
            var nameBuilder = new StringBuilder(capacity: length);
            while (nameBuilder.Length < length)
            {
                var nextCharIndex = _fileNameRandom.Next(0, _rndMaxIndex);
                var nextChar = _allwedNameChars[nextCharIndex];
                nameBuilder.Append(nextChar);
            }
            return nameBuilder.ToString();
        }
    }
}
