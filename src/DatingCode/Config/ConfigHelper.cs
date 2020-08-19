using DatingCode.Infrastructure.Config;
using DatingCode.Infrastructure.Config.Consts;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace DatingCode.Config
{
    public class ConfigHelper
    {
        public ConfigValuesCollection GetConfigValues(string configFilesDir, string environment)
        {
            var configProvider = new ConfigRootProvider(new ConfigFilesConsts());
            var configRoot = configProvider.GetConfig(configFilesRootPath: configFilesDir, environment: environment);
            var configValuesCollection = new ConfigValuesCollection();
            var roots = configRoot.GetChildren().ToArray();
            foreach( var section in roots)
                FillValuesRec(configValuesCollection, section);

            return configValuesCollection;
        }

        private void FillValuesRec(ConfigValuesCollection configValues, IConfigurationSection configElement)
        {
            if(configElement.Value != null)
            {
                configValues.AddValue(configElement.Path, configElement.Value);
            }

            var children = configElement.GetChildren().ToArray();
            if (children.Any())
            {
                foreach (var child in children)
                    FillValuesRec(configValues, child);
            }
        }
    }
}
