using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Drawing.Printing;
using System.Net;
using System.Net.Mime;
using System.Web.Hosting;

using Ionic.Zip;

using st1001.data;
using st1001.website.Helpers;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Net.Mail;
using System.Security.Permissions;
using System.Configuration;

namespace st1001.website.Controllers
{
    public class PDFController : Controller
    {
        public enum FileStatus
        {
            Created = 0,
            Converted = 1,
            Sent = 2
        }

        public static string ROOT_VIRTUAL_PATH = HttpRuntime.AppDomainAppVirtualPath;
        public static string ROOT_APP_FOLDER = HttpRuntime.AppDomainAppPath;


        [HttpGet]
        public ActionResult DonwloadFile(string guid)
        {
            if (string.IsNullOrWhiteSpace(guid))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "missing id param");
            }

            string pdfFolder = ROOT_APP_FOLDER + Constants.PDF_BY_EMAIL_TEMP_PATH;
            string fileName = guid + ".zip";
            string filePath = pdfFolder + Path.DirectorySeparatorChar + fileName;

            if (!System.IO.File.Exists(filePath)) {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "The requested file was not found in our system!");
            }

            var cd = new ContentDisposition {
                FileName = fileName,
                Inline = true
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filePath, "application/zip");
            
        }

        [HttpPost, ValidateInput(false), Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        public ActionResult Download()
        {
            var pID = Request["pid"];
            var cIDs = Request["cids"];

            if (string.IsNullOrWhiteSpace(pID) || string.IsNullOrWhiteSpace(cIDs))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "pid and cids params are required");
            }

            var url = Url.Action("Layouts", "Preview", new { projectID = pID, cids = cIDs }, this.Request.Url.Scheme);
            byte[] pdfBuf = PDFHelper.ConvertToPdf(PDFHelper.DataType.URL, url, "SunTrust");

            var cd = new ContentDisposition
                        {
                            FileName = "Offer" + pID + "Review.pdf",
                            Inline = false // always prompt the user for downloading, set to true if you want the browser to try to show the file inline
                        };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(pdfBuf, "application/pdf");
        }

        [HttpPost, ValidateInput(false), Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        public ActionResult DownloadAsZip()
        {
            var pIDs = Request["pids"];
            string email = User.Identity.Name;

            if (string.IsNullOrWhiteSpace(pIDs))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "pids is required");
            }

            var ids = pIDs.Split(',');
            if (ids.Length == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "pids parameter is empty");
            }

            int maxPDFNumberOnTheFly = 0;
            string maxPDFNumberOnTheFlyStr = ConfigurationManager.AppSettings["MaxPDFNumberOnTheFly"];
            Int32.TryParse(maxPDFNumberOnTheFlyStr, out maxPDFNumberOnTheFly);

            var db = new st1001Entities();
            var offers = db.Offers.Where(o => ids.Contains(o.ID.ToString())).ToList();

            var contentsIdsCount = 0;
            foreach (var offer in offers) {
                var cIds = offer.ContentIDs.Split(',');
                contentsIdsCount = contentsIdsCount + cIds.Length;
            }

            var urlColl = GetUrlCollection(ids);

            if (contentsIdsCount <= maxPDFNumberOnTheFly) // return the zip file on the fly
            {
                ZipFile zip = GetRequestAsZip(urlColl);
                var cd = new ContentDisposition
                {
                    FileName = "OfferReview.zip",
                    Inline = false // always prompt the user for downloading, set to true if you want the browser to try to show the file inline
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());

                var oStream = new MemoryStream();
                zip.Save(oStream);
                oStream.Position = 0;
                return File(oStream, "application/zip");
            }
            else // Send the result file as Email async
            {
                HostingEnvironment.QueueBackgroundWorkItem(ct => SendZipByEmailAsync(urlColl, pIDs, email));
                return new HttpStatusCodeResult(HttpStatusCode.OK, "The zip file will be send by email");
            }
        }

        [HostProtectionAttribute(SecurityAction.LinkDemand, ExternalThreading = true)]
        private void SendZipByEmailAsync(List<KeyValuePair<string, string>> urlColl, string pIDs, string emailTo)
        {
            st1001Entities db = new st1001Entities();
            var processTask = new ProcessFileTask
            {
                PIDs = pIDs,
                CIDs = string.Empty,
                Status = (int)FileStatus.Created,
                EmailTo = emailTo,
                RequestedDate = DateTime.UtcNow
            };

            db.ProcessFileTasks.Add(processTask);

            ZipFile zip = GetRequestAsZip(urlColl);
            if (zip == null)
            {
                LogHelper.Log("SendZipByEmailAsync - Error generating the zip file");
                processTask.Description = "Error generating the zip file";
                db.SaveChanges();
                return;
            }

            var result = SaveZipOnDisk(zip);
            if (result == null)
            {
                LogHelper.Log("SendZipByEmailAsync - Error saving the file on disk");
                processTask.Description = "Error saving the file on disk";
                db.SaveChanges();
                return;
            }

            processTask.Status = (int)FileStatus.Converted;
            processTask.ConvertedDate = DateTime.UtcNow;
            processTask.ConvertedFilePath = result.Item1;
            db.SaveChanges();

            try
            {
                SendEmail(processTask.EmailTo, result.Item2);
            }
            catch (Exception ex)
            {
                LogHelper.Log("SendZipByEmailAsync - Error sendig the email - " + ex.Message);
                processTask.Description = "Error sendig the email - " + ex.Message;
                db.SaveChanges();
                return;
            }

            processTask.Status = (int)FileStatus.Sent;
            processTask.Description = string.Format("Email sent to {0}", processTask.EmailTo);
            processTask.SentDate = DateTime.UtcNow;
            db.SaveChanges();
        }

        private Tuple<string, string> SaveZipOnDisk(ZipFile zip)
        {
            try
            {
                string pdfFolder = ROOT_APP_FOLDER + Constants.PDF_BY_EMAIL_TEMP_PATH;
                DirectoryInfo dir = Directory.CreateDirectory(pdfFolder);
                string guid = System.Guid.NewGuid().ToString();
                string fileName = guid + ".zip";

                string filePath = pdfFolder + Path.DirectorySeparatorChar + fileName;

                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ROOT_VIRTUAL_PATH;

                string fileUrl = baseUrl + "/PDF/DonwloadFile?guid=" + guid;

                using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    file.Position = 0;
                    zip.Save(file);
                }

                return new Tuple<string, string>(filePath, fileUrl);
            }
            catch (Exception ex)
            {
                LogHelper.Log("SaveZipOnDisk - Error saving the zip file on disk - " + ex.Message);
            }

            return null;
        }

        private void SendEmail(string messageTo, string fileUrl)
        {
            string domain = ConfigurationManager.AppSettings["EmailServer:Domain"];
            string portStr = ConfigurationManager.AppSettings["EmailServer:Port"];
            int port = 0;
            Int32.TryParse(portStr, out port);

            string enableSslStr = ConfigurationManager.AppSettings["EmailServer:EnableSsl"];
            bool enableSsl = false;
            Boolean.TryParse(enableSslStr, out enableSsl);

            string username = ConfigurationManager.AppSettings["EmailServer:Username"];
            string password = ConfigurationManager.AppSettings["EmailServer:Password"];
            string messageFrom = ConfigurationManager.AppSettings["EmailMessage:From"];
            string messageSubject = ConfigurationManager.AppSettings["EmailMessage:Subject"];
            string messageBody = ConfigurationManager.AppSettings["EmailMessage:Body"] + fileUrl;

            string isBodyHtmlStr = ConfigurationManager.AppSettings["EmailMessage:IsBodyHtml"];
            var isBodyHtml = false;
            Boolean.TryParse(isBodyHtmlStr, out isBodyHtml);

            var message = new MailMessage (messageFrom, messageTo)
            {
                IsBodyHtml = isBodyHtml,
                Subject = messageSubject,
                Body = messageBody,
            };
            
            using (var smtpClient = new SmtpClient(domain))
            {
                smtpClient.Port = port > 0 ? port : 25;
                smtpClient.EnableSsl = enableSsl;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(username, password);
                smtpClient.Send(message);
            }
        }

        private ZipFile GetRequestAsZip(List<KeyValuePair<string, string>> urlColl)
        {
            using (ZipFile zip = new ZipFile())
            {
                foreach (var item in urlColl)
                {
                    try
                    {
                        byte[] pdfBuf = PDFHelper.ConvertToPdf(PDFHelper.DataType.URL, item.Value, "SunTrust");
                        zip.AddEntry("Offer" + item.Key + "Review.pdf", pdfBuf);
                    }
                    catch (Exception e)
                    {
                        LogHelper.Log("SendZipByEmailAsync - Error generating the zip file - " + e.Message);
                    }
                }

                return zip;
            }
        }

        private List<KeyValuePair<string, string>> GetUrlCollection(string[] ids)
        {
            var urlColl = new List<KeyValuePair<string, string>>();
            var url = string.Empty;

            foreach (var id in ids)
            {
                url = Url.Action("Layouts", "Preview", new { projectID = id, cids = string.Empty }, this.Request.Url.Scheme);
                urlColl.Add(new KeyValuePair<string, string>(id, url));
            }

            return urlColl;
        }
    }
}