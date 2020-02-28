using System.Drawing.Printing;
using TuesPechkin;

namespace st1001.website.Helpers
{
    public class PDFHelper
    {
        private static IDeployment deployment = null;
        private static IConverter converter = null;

        public enum DataType
        {
            HTML,
            URL
        }

        static PDFHelper()
        {
            deployment = new Win32EmbeddedDeployment(new TempFolderDeployment());
            converter = new ThreadSafeConverter(
                new RemotingToolset<PdfToolset>(deployment));
        }

        public static byte[] ConvertToPdf(PDFHelper.DataType type, string data, string documentName)
        {
            var webSettings = new WebSettings
            {
                EnableJavascript = true,
                LoadImages = true,
                EnableIntelligentShrinking = true
            };

            ObjectSettings objSettings = null;

            objSettings = type == DataType.HTML
                ? new ObjectSettings { HtmlText = data, WebSettings = webSettings }
                : new ObjectSettings { PageUrl = data, WebSettings = webSettings };

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

            return converter.Convert(document);
        }
    }
}