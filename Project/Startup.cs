using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Project.Models;

[assembly: OwinStartup(typeof(Project.Startup))]
namespace Project
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(LibraryDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(
                ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(
                ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider()
            });
        }
    }
}