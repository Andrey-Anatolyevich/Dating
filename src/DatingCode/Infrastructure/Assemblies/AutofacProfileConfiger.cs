using Autofac;

namespace DatingCode.Infrastructure.Autofac
{
    public abstract class AutofacProfileConfiger
    {
        public virtual void ConfigureMappings(ContainerBuilder builder)
        {
        }
    }
}
