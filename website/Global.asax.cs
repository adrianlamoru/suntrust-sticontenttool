using st1001.website.DAL.Security;
using st1001.website.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using st1001.data;
using System.Diagnostics;
using st1001.website.Helpers;

namespace st1001.website
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.RegisterAuth();

            GlobalConfiguration.Configuration.EnsureInitialized(); 
        }

        protected void Application_PostAuthenticateRequest()
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string email = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;

                        using (st1001Entities db = new st1001Entities(ModelHelper.cnnString))
                        {
                            User user = db.Users.SingleOrDefault(u => u.Email == email && !u.Deleted);

                            CustomPrincipal principal = new CustomPrincipal(email);
                            principal.UserId = user.ID;
                            principal.Email = user.Email;
                            principal.FirstName = user.FirstName;
                            principal.LastName = user.LastName;
                            principal.Role = ModelHelper.GetRoleName(user.Role);
                            principal.UserType = ModelHelper.GetUserTypeName(user.UserType);

                            HttpContext.Current.User = principal;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log("Application_PostAuthenticateRequest - " + ex.Message);
                    }
                }
            } 
        }

        protected void Application_BeginRequest()
        {
            
        }
    }
}