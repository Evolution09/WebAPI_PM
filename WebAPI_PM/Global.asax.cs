using System.Web.Http;
using System.Web.Routing;

namespace WebAPI_PM
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        public static void RegisterRoutes(RouteCollection Routes)
        {
        }
    }
}
