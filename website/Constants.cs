using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace st1001.website
{
    public class Constants
    {
        public static string ROOT_APP_FOLDER = HttpContext.Current.Server.MapPath("~");
        public const string MEDIA_LIBRARY_FOLDER_NAME = "MediaLibrary";
        public const string EMAIL_TEMPLATE_FOLDER_NAME = "GeneratedEmailTemplates";
        public const string MEDIA_LIBRARY_PATH = "/" + MEDIA_LIBRARY_FOLDER_NAME;
        public const string EMAIL_TEMPLATE_PATH = "/" + EMAIL_TEMPLATE_FOLDER_NAME;
        public const string MEDIA_LIBRARY_PROJECT_PATH = MEDIA_LIBRARY_PATH + "/Project";
        public const string MEDIA_LIBRARY_PUBLIC_PATH = MEDIA_LIBRARY_PATH + "/Public";
        public const string PDF_BY_EMAIL_TEMP_PATH = "App_Data\\PDFTemp";

        public const string XML_ASSET_FOLDER_NAME = "Assets";
        public const string XML_DESCRIPTORS_FOLDER_NAME = "XMLDescriptors";
        public const string HTML_DESCRIPTORS_FOLDER_NAME = "HTMLDescriptors";

        public const int MAIN_IMAGE_COMPONENT_ID = 1;

        public const string ALL_ASSET_TYPE_FILTER = "ALL";
        public const string PROJECT_FOLDERS_ASSET_TYPE_FILTER = "PROJECT_FOLDER";
        public const string CUSTOM_FOLDERS_ASSET_TYPE_FILTER = "CUSTOM_FOLDER";
        public const string FILES_ASSET_TYPE_FILTER = "FILE";

        public const string ALL_FILE_TYPE_FILTER = "ALL";
        public const string IMAGES_FILE_TYPE_FILTER = "IMAGE";
        public const string PDF_FILE_TYPE_FILTER = "PDF";
        public const string TEXTS_FILE_TYPE_FILTER = "TEXT";
        public const string SPREADSHEETS_FILE_TYPE_FILTER = "SPREADSHEET";
        public const string PRESENTATIONS_FILE_TYPE_FILTER = "PRESENTATION";
        public const string OTHERS_FILE_TYPE_FILTER = "OTHER";
    }
}