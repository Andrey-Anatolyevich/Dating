using DatingCode.Infrastructure.Config.Consts;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DatingCode.Infrastructure.Config
{
    public class ConfigRootProvider
    {
        public ConfigRootProvider(ConfigFilesConsts configFiles)
        {
            if (configFiles == null)
                throw new ArgumentNullException(nameof(configFiles));


            _rootConfigFileName = configFiles.RootConfigFileName;
            _environmentFileNameTemplate = configFiles.EnvironmentFileNameTemplate;
            _environmentPlaceholder = configFiles.EnvironmentPlaceholder;
        }

        private string _rootConfigFileName;
        private string _environmentFileNameTemplate;
        private string _environmentPlaceholder;

        public IConfigurationRoot GetConfig(string configFilesRootPath, string environment)
        {
            if (string.IsNullOrWhiteSpace(configFilesRootPath))
                throw new ArgumentException("String is NULL / Empty / Whitespace.", nameof(configFilesRootPath));


            var configBuilder = new ConfigurationBuilder();
            configBuilder.SetBasePath(configFilesRootPath);

            var rootConfigFilePath = Path.Combine(configFilesRootPath, _rootConfigFileName);
            configBuilder.AddJsonFile(rootConfigFilePath, optional: false);

            if (!string.IsNullOrWhiteSpace(environment))
            {
                var environmentConfigFileName = _environmentFileNameTemplate.Replace(_environmentPlaceholder, environment);
                var environmentConfigPath = Path.Combine(configFilesRootPath, environmentConfigFileName);
                configBuilder.AddJsonFile(environmentConfigPath, optional: true);
            }

            var result = configBuilder.Build();
            return result;
        }
    }
}
