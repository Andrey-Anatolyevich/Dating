using Autofac;
using AutoMapper;
using Dating.Infrastructure.AutoMapper;
using DatingCode.Config;
using DatingCode.Infrastructure.Assemblies;
using DatingCode.Infrastructure.Autofac;
using System;
using System.Linq;
using System.Reflection;

namespace Dating.Infrastructure.Autofac
{
    public class AutofacDependencyRegisterer
    {
        internal void RegisterDependencies(ContainerBuilder builder, ConfigValuesCollection configValuesCollection)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            RegisterModules(builder);

            builder.RegisterInstance(configValuesCollection).AsSelf().SingleInstance();
        }

        private void RegisterModules(ContainerBuilder builder)
        {
            var solutionsProvider = new SolutionAssembliesProvider();
            var solutionAssemblies = solutionsProvider.GetSolutionAssemblies();

            foreach (var solutionAssembly in solutionAssemblies)
            {
                var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                var assembly = loadedAssemblies.FirstOrDefault(asm => asm.Location == solutionAssembly.FullName);
                if(assembly == null)
                    assembly = Assembly.LoadFile(solutionAssembly.FullName);

                var allTypes = assembly.GetTypes();
                var rightTypes = allTypes
                    .Where(type => type.IsSubclassOf(typeof(AutofacProfileConfiger)))
                    .ToArray();

                foreach (var rightType in rightTypes)
                {
                    var profileConfiger = Activator.CreateInstance(rightType) as AutofacProfileConfiger;
                    profileConfiger.ConfigureMappings(builder);
                }
            }

            // Register AutoMapper
            var autoMapperTool = new AutoMapperTool();
            var mapper = autoMapperTool.GetConfiguredMapper();
            builder.RegisterInstance(mapper).As<IMapper>();
        }
    }
}
