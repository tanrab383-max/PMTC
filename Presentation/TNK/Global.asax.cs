using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TNK.Core.Infrastructure;
using TNK.Core.Infrastructure.DependencyManagement;
using TNK.Themes;
using TNK.Services.Log;
using AF.Library;
namespace TNK
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new ThemeableRazorViewEngine());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterDependency();
            //MvcHandler.DisableMvcResponseHeader = true;

        }
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();            
            AF.Library.Logger.CreateInstant("Global.asax").Error("Link:" + Request.RawUrl);
            AF.Library.Logger.CreateInstant("Global.asax").Error(exception);
            if(exception.StackTrace!= null)
                AF.Library.Logger.CreateInstant("Global.asax").Error(exception.StackTrace);
            if (exception.InnerException != null)
                AF.Library.Logger.CreateInstant("Global.asax").Error(exception.InnerException);
            // Do something with the error.            
            Session["Error"] = Request.RawUrl + ":" + exception.ToString();
            Session["LastException"] = exception;
            if (!Request.Url.ToString().Contains("/Error"))
            {
                // Redirect somewhere or return an error code in case of web api
                Response.Redirect(Request.Url.Scheme + "://" + Request.Url.Authority + "/Error/Index");
               // Response.Redirect("http://tnk.phattien.net:81/Error/Index");
            }
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            var storeId = Session["StoreId"] as string;
            if (string.IsNullOrEmpty(storeId))
            {
                // Điều hướng đến trang đăng nhập hoặc thực hiện xử lý khác
                Response.Redirect("~/User/Login");
            }
        }
        protected void Session_End(object sender, EventArgs e)
        {

        }

        private ContainerManager _containerManager;
        public void RegisterDependency()
        {

            var builder = new ContainerBuilder();
            var container = builder.Build();
            this._containerManager = new ContainerManager(container);

            //we create new instance of ContainerBuilder
            //because Build() or Update() method can only be called once on a ContainerBuilder.

            //dependencies
            var typeFinder = new WebAppTypeFinder();
            builder = new ContainerBuilder();
            builder.RegisterInstance(typeFinder).As<ITypeFinder>().SingleInstance();
            builder.Update(container);

            //register dependencies provided by other assemblies
            builder = new ContainerBuilder();
            var drTypes = typeFinder.FindClassesOfType<IDependencyRegistrar>();
            var drInstances = new List<IDependencyRegistrar>();
            foreach (var drType in drTypes)
                drInstances.Add((IDependencyRegistrar)Activator.CreateInstance(drType));
            //sort
            drInstances = drInstances.AsQueryable().OrderBy(t => t.Order).ToList();
            foreach (var dependencyRegistrar in drInstances)
                dependencyRegistrar.Register(builder, typeFinder);
            builder.Update(container);

            //set dependency resolver
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
