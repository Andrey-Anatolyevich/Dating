using System.IO;
using System.Reflection;

namespace DatingCode.Infrastructure.Assemblies
{
    public class SolutionAssembliesProvider
    {
        private const string _myAssembliesFilesMask = "*Dating*.dll";

        public FileInfo[] GetSolutionAssemblies()
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            var currentAssemblyFilePath = currentAssembly.Location;
            var currentAssemblyDirPath = Path.GetDirectoryName(currentAssemblyFilePath);
            var currentAssemblyDir = new DirectoryInfo(currentAssemblyDirPath);
            var solutionAssemblyFiles = currentAssemblyDir.GetFiles(_myAssembliesFilesMask, SearchOption.TopDirectoryOnly);
            return solutionAssemblyFiles;
        }
    }
}
