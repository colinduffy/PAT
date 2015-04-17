using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Helpers;
using System.Data.Entity;

namespace PAT_ELAC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<PAT_ELAC.Models.TopicContext>(null); //Bug workaround
            Database.SetInitializer<PAT_ELAC.Models.TestContext>(null); //Bug workaround
            Database.SetInitializer<PAT_ELAC.Models.SubjectContext>(null); //Bug workaround
            Database.SetInitializer<PAT_ELAC.Models.TestTopicsContext>(null); //Bug workaround
            Database.SetInitializer<PAT_ELAC.Models.QuestionContext>(null); //Bug workaround
            Database.SetInitializer<PAT_ELAC.Models.AnswerContext>(null); //Bug workaround
            Database.SetInitializer<PAT_ELAC.Models.AnsweredQuestionsContext>(null); //Bug workaround
            Database.SetInitializer<PAT_ELAC.Models.TakenContext>(null); //Bug workaround
            Database.SetInitializer<PAT_ELAC.Models.ResourceContext>(null); //Bug workaround

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;

        }

        private void Application_Error(object sender, EventArgs e)
        {
            /*Exception ex = Server.GetLastError();

            //if (ex is HttpAntiForgeryException)
            //{
                Response.Clear();
                Server.ClearError(); //make sure you log the exception first
                Response.Redirect("~/Shared/Error", true);
            //}*/

                Exception exception = Server.GetLastError();
                HttpException httpException = exception as HttpException;
        }
    }
}