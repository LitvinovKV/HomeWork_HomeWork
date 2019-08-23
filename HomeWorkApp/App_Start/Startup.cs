using HomeWorkApp.App_Start;
using HomeWorkApp.Models;
using HomeWorkApp.Util;
using HomeWorkApp.Util.IoC_Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System.Web.Mvc;

[assembly: OwinStartup(typeof(Startup))]

namespace HomeWorkApp.App_Start
{
    public class Startup
    {
        IDataBaseConnection connection;
        public Startup()
        {
            connection = DependencyResolver.Current.GetService<IDataBaseConnection>();
        }

        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new UserManager(connection.CreateIdentityStoreUsers));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider()
            });
        }
    }
}