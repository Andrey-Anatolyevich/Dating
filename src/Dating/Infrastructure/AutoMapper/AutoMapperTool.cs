using AutoMapper;
using DatingCode.Infrastructure.Assemblies;
using DatingCode.Infrastructure.Automapper;
using System;
using System.Linq;
using System.Reflection;

namespace Dating.Infrastructure.AutoMapper
{
    public class AutoMapperTool
    {
        public IMapper GetConfiguredMapper()
        {
            var config = new MapperConfiguration(ConfigureAllProfiles);
            config.AssertConfigurationIsValid();
            var mapper = new Mapper(config);
            return mapper;
        }

        private void ConfigureAllProfiles(IMapperConfigurationExpression expression)
        {
            var solutionsProvider = new SolutionAssembliesProvider();
            var escortAssemblyFiles = solutionsProvider.GetSolutionAssemblies();

            foreach (var assemblyFile in escortAssemblyFiles)
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                var assembly = loadedAssemblies.FirstOrDefault(asm => asm.Location == assemblyFile.FullName);
                if (assembly == null)
                    assembly = Assembly.LoadFile(assemblyFile.FullName);

                var allTypes = assembly.GetTypes();
                foreach(var type in allTypes)
                {
                    if (type.BaseType?.FullName != typeof(AutomapperProfileConfiger).FullName)
                        continue;
                    
                    var profileConfigerObj = Activator.CreateInstance(type);
                    var profileConfiger = profileConfigerObj as AutomapperProfileConfiger;
                    profileConfiger.ConfigureMappings(expression);
                }
            }
        }
    }
}
