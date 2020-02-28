using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using st1001.data;
using st1001.website.Models;
using st1001.website.ActionResults;
using Ionic.Zip;
using System.Net;
using Newtonsoft.Json.Linq;

namespace st1001.website.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        //
        // GET: /Project/

        [HttpPost, ValidateInput(false)]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        [Route("Project/Export")]
        public ActionResult ExportProject()
        {
            string cIDs = Request["cids"];
            string pID = Request["pid"];
            
            if (string.IsNullOrWhiteSpace(pID) || string.IsNullOrWhiteSpace(cIDs))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "pid and cids params are required");
            }

            st1001Entities db = new st1001Entities();

            Project projectModel = db.Projects.Find(int.Parse(pID));

            if (projectModel == null)
            {
                return HttpNotFound();
            }

            var result = new ProjectExportZipResult(projectModel, cIDs.Split(','));

            return result;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ExportProjects()
        {
            var pIDs = Request["pids"];

            if (string.IsNullOrWhiteSpace(pIDs))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "pids parameter is required.");
            }

            var ids = pIDs.Split(',');
            if (ids.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "pids parameter must has value.");
            }

            st1001Entities db = new st1001Entities();

            var projects = db.Projects.Where(p => ids.Contains(p.ID.ToString())).ToList();

            if (projects.Count == 0)
            {
                return HttpNotFound();
            }

            var result = new ProjectExportZipResult(projects);

            return result;
        }

        [Authorize(Roles = "Administrator, Project Manager,Super Admin")]
        [Route("Project/ExportContents/{id}")]
        public ActionResult ExportContents(int id)
        {
            var db = new st1001Entities();
            var project = db.Projects.Find(id);

            if (project != null) {
                var csv = "";
                var fileName = string.Format("ProjectContents-{0}.csv", project.ID);
                var fields = new string[] { "buttonText", "Description", "Headline", "Conditions" }; 

                foreach (var comp in project.Components) {
                    var jObt = JObject.Parse(comp.Data);
                    foreach (var field in fields) {
                        if (jObt[field] != null) {
                            csv = csv + comp.ID + "," + field + "," + jObt[field] + Environment.NewLine;
                        }
                    }
                }

                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", fileName);
            }

            return HttpNotFound();            
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrator, Project Manager,Super Admin")]
        [Route("Project/ImportContents")]
        public JsonResult ImportContents(HttpPostedFileBase upload)
        {
            var db = new st1001Entities();

            if (ModelState.IsValid && upload != null && upload.ContentLength > 0)
            {
                if (upload.FileName.EndsWith(".csv"))
                {
                    using (var csvReader = new System.IO.StreamReader(upload.InputStream))
                    {
                        var inputLine = "";
                        while ((inputLine = csvReader.ReadLine()) != null) //read each line
                        {
                            var columns = inputLine.Split(new char[] { ',' }); //get lines values

                            var compId = int.Parse(columns[0]);
                            var field = columns[1];
                            var value = columns[2];

                            var comp = db.Components.FirstOrDefault(c => c.ID == compId);

                            if (comp != null) {
                                var jObject = JObject.Parse(comp.Data);
                                jObject[field] = value;
                                comp.Data = jObject.ToString();
                            }
                        }

                        db.SaveChanges();
                        csvReader.Close();
                    }

                    return Json(new { Succes = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Succes = false, Message = "This file format is not supported" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { Succes = false, Message = "Please Upload Your file" }, JsonRequestBehavior.AllowGet);
        }
    }
}
