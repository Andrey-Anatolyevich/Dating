using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Dating
{
    internal class RoutesRegisterer
    {
        internal void RegisterRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(name: "Area_default", template: "{area:exists}/{controller}/{action=Index}/{id?}");
            routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
            routes.MapRoute(
                    name: "fallback",
                    template: "{*url}",
                    defaults: new { controller = "Home", action = "Index" });
        }

        internal void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllers();
            endpointRouteBuilder.MapRazorPages();

            endpointRouteBuilder.MapAreaControllerRoute(name: "Area: Admin", areaName: "Admin",
                pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
            endpointRouteBuilder.MapAreaControllerRoute(name: "Area: Api", areaName: "Api",
                pattern: "Api/{controller=Home}/{action=Index}/{id?}");
            endpointRouteBuilder.MapAreaControllerRoute(name: "Area: Auth", areaName: "Auth",
                pattern: "Auth/{controller=Home}/{action=Index}/{id?}");
            endpointRouteBuilder.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
            endpointRouteBuilder.MapControllerRoute(
                    name: "fallback",
                    pattern: "{*url}",
                    defaults: new { controller = "Home", action = "Index" });
        }
    }
}
