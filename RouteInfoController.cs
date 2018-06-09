using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Routing;
using System.Web.Services.Protocols;

namespace Web.Controllers.Api
{
    public class RouteInfoController : ApiController
    {
        [Route("api/route-table"), HttpGet, AllowAnonymous]
        public HttpResponseMessage GetRouteInfo()
        {
            var controllerClasses =
                Assembly.GetExecutingAssembly()
                .DefinedTypes
                .Where(type => typeof(ApiController).IsAssignableFrom(type))
                .SelectMany(type =>
                    type
                    .DeclaredMethods
                    .Select(method => new
                    {
                        Route = method.GetCustomAttribute<RouteAttribute>()?.Template,
                        ClassName = type.Name,
                        MethodName = method.Name,
                        HttpVerb = GetHttpMethod(method),
                        Authentication = method.GetCustomAttribute<AllowAnonymousAttribute>() == null,
                        Role = method.GetCustomAttribute<AuthorizeAttribute>()?.Roles
                    })
                    .Where(o => o.Route != null)
                )
                .OrderBy(t => t.Route)
                .ToList();
            return Request.CreateResponse(HttpStatusCode.OK, controllerClasses);
        }

        string GetHttpMethod(MethodInfo method)
        {
            var httpMethodAttr = method.GetCustomAttributes()
                        .SingleOrDefault(m => typeof(IActionHttpMethodProvider)
                        .IsAssignableFrom(m.GetType()));

            if (httpMethodAttr == null)
            {
                return null;
            }
            else if (httpMethodAttr is HttpGetAttribute)
            {
                return "GET";
            }
            else if (httpMethodAttr is HttpPostAttribute)
            {
                return "POST";
            }
            else if (httpMethodAttr is HttpPutAttribute)
            {
                return "PUT";
            }
            else if (httpMethodAttr is HttpDeleteAttribute)
            {
                return "DELETE";
            }
            return null;
        }
    }
}
