using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Text;
using System.Drawing.Printing;

using TuesPechkin;
using PdfServ.Models;

namespace PdfServ.Controllers
{
    public class PdfController : Controller
    {

        public static string DEFAULT_URL = "https://en.wikipedia.org/";
        public static string DEFAULT_DOCNAME = "My Document";
        public static string DEFAULT_FILENAME = "my_pdf.pdf";
        enum DataType {
            HTML,
            URL
        }

        // GET: Pdf
        public ActionResult Index()
        {
            ViewBag.defaultUrl = DEFAULT_URL;
            ViewBag.defaultDocName = DEFAULT_DOCNAME;
            ViewBag.defaultFileName = DEFAULT_FILENAME;
            return View();
        }

        // GET: PDF
        [HttpGet, ValidateInput(false)]
        public ActionResult Get()
        {
            return ProcessRequest();
        }

        // POST: PDF
        [HttpPost, ValidateInput(false)]
        public ActionResult Download()
        {
            return ProcessRequest();
        }

        public ActionResult ProcessRequest()
        {
            var url = Request["url"];
            var html = Request["html"];

            var documentName = Request["docName"];
            if (string.IsNullOrEmpty(documentName))
            {
                documentName = DEFAULT_DOCNAME;
            }

            var fileName = Request["fileName"];
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = DEFAULT_FILENAME;
            }

            byte[] pdfBuf = null;

            if (!string.IsNullOrEmpty(html))
            {
                pdfBuf = PDFHelper.ConvertToPdf(PDFHelper.DataType.HTML, html, documentName);
            }
            else
            {
                if (string.IsNullOrEmpty(url))
                    url = DEFAULT_URL;

                pdfBuf = PDFHelper.ConvertToPdf(PDFHelper.DataType.URL, url, documentName);
            }

            if (pdfBuf == null)
            {
                // return error here.
            }

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fileName,

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(pdfBuf, "application/pdf");
        }
    }
}