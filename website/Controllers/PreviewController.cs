using st1001.data;
using st1001.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace st1001.website.Controllers
{
    public class PreviewController : Controller
    {
        private st1001Entities db = new st1001Entities();

        [AllowAnonymous]
        public ActionResult Layouts(int projectID, string cids)
        {
            ViewBag.projectID = projectID;
            Project project = db.Projects.Find(projectID);

            if (project != null)
            {
                var pVM = new ProjectLayoutCreateEditViewModel();
                pVM.CopyFromModel(project);

                if (!string.IsNullOrWhiteSpace(cids))
                {
                    var array = cids.Split(',');
                    IList<LayoutViewModel> layouts = pVM.Layouts.Where(l => array.Contains(l.ID)).ToList();

                    pVM.Layouts = layouts;
                }

                ViewBag.project = JsonConvert.SerializeObject(pVM);
            }
            else
            {
                ViewBag.project = "{}";
            }
            
            return View();
        }

        [AllowAnonymous]
        [Route("Preview/Project/{id}")]
        public ActionResult Project(string id)
        {
            var project = db.Projects.FirstOrDefault(p => p.GUID.ToString() == id);

            if (project != null)
            {
                ViewBag.projectID = project.ID;
                var pVM = new ProjectLayoutCreateEditViewModel();
                pVM.CopyFromModel(project);

                IList<LayoutViewModel> layouts = pVM.Layouts.ToList();
                pVM.Layouts = layouts;

                ViewBag.project = JsonConvert.SerializeObject(pVM);
            }
            else
            {
                ViewBag.project = "{}";
            }
            switch (project.Offer.OfferType)
            {
                case 0:
                    return View("Layouts");
                case 1:
                   return View("Email-Template");
                default:
                    return View("Layouts");
            }
            
        }
    }
}