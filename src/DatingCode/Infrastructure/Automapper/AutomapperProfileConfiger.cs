using AutoMapper;

namespace DatingCode.Infrastructure.Automapper
{
    public abstract class AutomapperProfileConfiger
    {
        public virtual void ConfigureMappings(IProfileExpression profile)
        {
        }
    }
}
