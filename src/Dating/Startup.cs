using Autofac;
using Autofac.Extensions.DependencyInjection;
using Dating.Infrastructure.Autofac;
using DatingCode.Config;
using DatingCode.Infrastructure.Di;
using DatingCode.Mvc.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace Dating
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
            _environment = _configuration.GetValue<string>("ENVIRONMENT");
        }

        public IConfiguration _configuration { get; }
        public string _environment;

        private IContainer _diContainer;

        /// <summary> This method gets called by the runtime. Use this method to add services to the container. </summary>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDistributedMemoryCache()
                .AddSession(options =>
                    {
                        //TODO: Andrey Yurchik: Hardcode
                        options.IdleTimeout = TimeSpan.FromMinutes(30);
                        options.Cookie.HttpOnly = false;
                        options.Cookie.Name = "Session";
                    })
                .AddAutofac()
                .AddMvc();


            // CONFIGURATION
            var rootDir = Environment.CurrentDirectory;
            var configFilesDir = Path.Combine(rootDir, "Infrastructure", "ConfigJsons");
            var configHelper = new ConfigHelper();
            var configValuesCollection = configHelper.GetConfigValues(configFilesDir, _environment);

            // AUTOFAC
            var dependencyRegisterer = new AutofacDependencyRegisterer();
            var builder = new ContainerBuilder();
            dependencyRegisterer.RegisterDependencies(builder, configValuesCollection);
            builder.Populate(services);
            _diContainer = builder.Build();
            _diContainer.Resolve<DiProxy>();
            //Create the IServiceProvider based on the container.
            var result = new AutofacServiceProvider(_diContainer);
            return result;
        }

        /// <summary> This method gets called by the runtime. PIPELINE configuration is done here. </summary>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (string.Equals(env.EnvironmentName, "Development", StringComparison.InvariantCultureIgnoreCase))
                app.UseDeveloperExceptionPage();
            else
            {
                app.UseExceptionHandler("/Home/Error");

                // Do not allow HTTP. Require HTTPS
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            var configValues = _diContainer.Resolve<ConfigValuesCollection>();
            var userFilesDir = configValues.GetUserFilesDirectory();
            var userFilesAccessUrlPrefix = configValues.GetUserFilesUrlPrefix();

            app.UseStaticFiles()
                .UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(userFilesDir),
                    RequestPath = userFilesAccessUrlPrefix
                })
                .UseCookiePolicy()
                .UseSession()
                .UseMiddleware<SessionPinnerMiddleware>()
                .UseMiddleware<UserInfoAssignerMiddleware>()
                .UseMiddleware<LocaleAssignerMiddleware>()
                .UseMiddleware<NotFoundRedirectorMiddleware>()
                .UseMiddleware<UnauthorizedRedirectorMiddleware>()
                .UseRouting();

            var routesRegisterer = new RoutesRegisterer();
            app.UseEndpoints(routesRegisterer.RegisterRoutes);
        }
    }
}
