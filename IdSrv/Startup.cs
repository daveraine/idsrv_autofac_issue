using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Thinktecture.IdentityServer.AccessTokenValidation;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Core.Logging;

[assembly: OwinStartup(typeof(IdSrv.Startup))]
namespace IdSrv
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            var container = builder.Build();

            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            var factory = InMemoryFactory.Create(
                users: Users.Get(),
                clients: Clients.Get(),
                scopes: Scopes.Get());

            var options = new IdentityServerOptions
            {
                Factory = factory
            };

            app.UseIdentityServer(options);

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44333",
                RequiredScopes = new[] { "api1" }
            });

            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseWebApi(config);
        }
    }
}