using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ionic.Zip;
using System.IO;
using st1001.data;
using Newtonsoft.Json;
using st1001.website.Helpers;
using st1001.website.Models;

namespace st1001.website.ActionResults
{
    public class ProjectExportZipResult : ActionResult
    {
        string mediaLibraryPath = Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PATH;
        string emailTemplatesPath = Constants.ROOT_APP_FOLDER + Constants.EMAIL_TEMPLATE_PATH;

        IList<Project> projects = null;
        string[] contentIDs = null;

        public string FileName
        {
            get {
                return projects != null && projects.Count == 1 ? "Project-" + projects[0].ID + ".zip" : "Projects.zip";
            }
        }

        public ProjectExportZipResult(Project projectModel, string[] contentIDs)
        {
            this.projects = new List<Project>();
            this.projects.Add(projectModel);
            this.contentIDs = contentIDs;
        }

        public ProjectExportZipResult(IList<Project> projects)
        {
            this.projects = projects;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            using (ZipFile zf = new ZipFile())
            {
                foreach (var projectModel in projects)
                {
                    foreach (var c in projectModel.Components)
                    {
                        try
                        {
                            if (c.ComponentID == Constants.MAIN_IMAGE_COMPONENT_ID && c.Data != "{}" && IsAllowedToExport(c.ContentID))
                            {
                                var data = JsonConvert.DeserializeObject<dynamic>(c.Data);
                                string relativeFilePath = Convert.ToString(data["src"]);

                                if (!Convert.ToBoolean(data["isExternal"]) && !string.IsNullOrWhiteSpace(relativeFilePath))
                                {
                                    string absPath = Constants.ROOT_APP_FOLDER + relativeFilePath;

                                    if (File.Exists(absPath))
                                    {
                                        string fileName = Path.GetFileName(relativeFilePath);
                                        string pathInsideZip = string.Format("{0}/{1}/{2}", projectModel.ID, Constants.XML_ASSET_FOLDER_NAME, c.ContentID);

                                        zf.AddItem(absPath, pathInsideZip);
                                    }
                                    else
                                    {
                                        LogHelper.Log("ProjectExportZipResult - Reference to Image not found: " + absPath);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Log("ProjectExportZipResult - Adding item to zip" + ex.Message);
                        }
                    }

                    var pVM = new ProjectLayoutCreateEditViewModel();
                    pVM.CopyFromModel(projectModel);
                    var offerType = projectModel.Offer.OfferType;
                    ProjectXMLHelper xmlHelper = new ProjectXMLHelper();
                    ProjectHTMLHelper htmlHelper = new ProjectHTMLHelper();
                    
                    foreach (var contentID in pVM.Layouts)
                    {
                        if (IsAllowedToExport(contentID.ID))
                        {
                            if (offerType == 0)
                            {
                                string xml = xmlHelper.GetAsXML(pVM.ID.ToString(), pVM.Name, contentID);
                                string xmlName = string.Format("{0}/{1}/{2}.xml", projectModel.ID, Constants.XML_DESCRIPTORS_FOLDER_NAME, contentID.ID);
                                zf.AddEntry(xmlName, xml, System.Text.Encoding.UTF8);
                            } else {
                                string emailFolderPath = "";
                                try
                                {   
                                    emailFolderPath = emailTemplatesPath + "/" + pVM.ID.ToString();
                                    if (!Directory.Exists(emailFolderPath))
                                    {
                                        Directory.CreateDirectory(emailFolderPath);
                                    }
                                    emailFolderPath = emailFolderPath + "/" + contentID.ID.ToString();
                                    if (!Directory.Exists(emailFolderPath))
                                    {
                                        Directory.CreateDirectory(emailFolderPath);
                                    }
                                }
                                catch
                                {

                                }
                                string guid = System.Guid.NewGuid().ToString();
                                string emailName = "/" + guid + ".html";
                                string emailFilePath = "/GeneratedEmailTemplates/" + pVM.ID.ToString() + "/" + contentID.ID.ToString() + emailName;
                                string html = htmlHelper.GetAsHTML(pVM.ID.ToString(), pVM.Name, contentID, emailFilePath);
                                string htmlName = string.Format("{0}/{1}/{2}.html", projectModel.ID, Constants.HTML_DESCRIPTORS_FOLDER_NAME, contentID.ID);
                                zf.AddEntry(htmlName, html, System.Text.Encoding.UTF8);
                               
                                using (StreamWriter writetext = new StreamWriter(emailFolderPath + emailName))
                                {
                                    writetext.WriteLine(html);
                                }
                            }
                            
                        }
                    }
                }

                context.HttpContext.Response.ContentType = "application/zip";
                context.HttpContext.Response.AppendHeader("content-disposition", "attachment; filename=" + FileName);

                zf.Save(context.HttpContext.Response.OutputStream);
            }
        }

        private bool IsAllowedToExport(string cid)
        {
            if (this.contentIDs == null)
            {
                return true;
            }

            foreach (string item in this.contentIDs)
            {
                if (cid == item)
                {
                    return true;
                }
            }

            return false;
        }
    }
}