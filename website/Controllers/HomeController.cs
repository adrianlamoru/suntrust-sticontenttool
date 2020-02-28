using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using st1001.website.DAL.Security;
using st1001.data;
using st1001.website.Models;
using PagedList;
using System.Web.Configuration;

namespace st1001.website.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private st1001Entities db = new st1001Entities();

        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        public ActionResult Index()
        {
            return View("Dashboard");    
        }


        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        public ActionResult Dashboard()
        {
            return View();
        }

        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        public ActionResult OfferProjectLayout()
        {
            return View();
        }

        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        public ActionResult TestWebAPI()
        {
            return View();
        }
    }
}
