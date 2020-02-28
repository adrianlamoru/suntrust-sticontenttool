using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Drawing.Printing;
using TuesPechkin;
using System.IO;

namespace PdfServ.Models
{
    public class PDFHelper
    {
        public static string DLL_PATH = HttpContext.Current.Server.MapPath("~") + "\\bin\\TuesPechkin.Wkhtmltox.Win32.dll";

        public enum DataType
        {
            HTML,
            URL
        }

        /*
        private static IConverter converter =
            new ThreadSafeConverter(
                new RemotingToolset<PdfToolset>(
                    new Win32EmbeddedDeployment(
                        new TempFolderDeployment())));
        */

        private static IDeployment deployment =
                new Win32EmbeddedDeployment(new TempFolderDeployment());

        private static IConverter converter =
            new ThreadSafeConverter(
                new PdfToolset(deployment));

        public static byte[] ConvertToPdf(PDFHelper.DataType type, string data, string documentName)
        {
            var webSettings = new WebSettings
            {
                EnableJavascript = true,
                LoadImages = true,
                EnableIntelligentShrinking = true
            };

            ObjectSettings objSettings = null;

            if (type == DataType.HTML)
                objSettings = new ObjectSettings { HtmlText = data, WebSettings = webSettings };
            else
                objSettings = new ObjectSettings { PageUrl = data, WebSettings = webSettings };

            var document = new HtmlToPdfDocument
            {
                GlobalSettings =
                {
                    ProduceOutline = true,
                    DocumentTitle = documentName,
                    PaperSize = PaperKind.Letter,
                    UseCompression = true,
                    ImageDPI = 600,
                    ImageQuality = 100,
                    

                    //DPI = 1200,
                    Margins =
                    {
                        Top = 1,
                        Right = 0.5,
                        Bottom = 1,
                        Left = 0.5,
                        Unit = Unit.Centimeters
                    }
                },
                Objects = {
                   objSettings
                }
            };

            // Keep the converter somewhere static, or as a singleton instance!

            byte[] result = converter.Convert(document);

            return result;
        }
    }
}