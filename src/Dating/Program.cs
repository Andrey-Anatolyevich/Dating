using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Dating
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var hostBuilder = WebHost.CreateDefaultBuilder(args);
            hostBuilder.UseStartup<Startup>();

            var builtHost = hostBuilder.Build();
            builtHost.Run();
        }
    }
}
