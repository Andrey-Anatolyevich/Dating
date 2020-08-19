namespace DatingCode.Infrastructure.Config.Consts
{
    public class ConfigFilesConsts
    {
        public string RootConfigFileName = "appConfig.json";
        public string EnvironmentFileNameTemplate = "appConfig.[ENVIRONMENT].json";
        public string EnvironmentPlaceholder = "[ENVIRONMENT]";
    }

    public class ConfigSession
    {
        public string SessionPinKey = "Pin";
        public string SessionPinValue = "pin";
    }
}
