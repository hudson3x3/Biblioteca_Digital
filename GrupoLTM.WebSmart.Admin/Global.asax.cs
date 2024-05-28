using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using GrupoLTM.WebSmart.Infrastructure;
using GrupoLTM.WebSmart.Admin.Configuration.AutoMapper;
using GrupoLTM.WebSmart.Services.Log;

namespace GrupoLTM.WebSmart.Admin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            AutoMapperConfiguration.Initialize();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var ex = Server.GetLastError();
            var currentContext = new HttpContextWrapper(HttpContext.Current);

            var urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var routeData = urlHelper.RouteCollection.GetRouteData(currentContext);

            var ipUser = currentContext.Request.UserHostAddress.ToString();
            var action = routeData.Values["action"] as string;
            var controller = routeData.Values["controller"] as string;

            var log = new LogControllerModel
            {
                IP = ipUser,
                Error = ex.Message,
                Date = DateTime.Now,
                Class = "Global.asax",
                Method = "Application_Error",
                StackTrace = ex.StackTrace,
                Message = ex.Message,
                Source = ex.Source,
                Action = action,
                Controller = controller,
                Page = currentContext.Request.Url.ToString(),
                TargetSite = ex.TargetSite.Serialize().ToString()
            };

            //TODO: Update DataDog
            //GrayLogService.LogError(log);
        }
    }
}