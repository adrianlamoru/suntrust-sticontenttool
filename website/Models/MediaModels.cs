using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace st1001.website.Models
{
    public class MediaAssetViewModel
    {
        public string name { get; set; }
        public string src { get; set; }
        public string type { get; set; }
        public string subtype { get; set; }
        public string title { get; set; }
        public bool isFile { get; set; }
        public bool isProjectFolder { get; set; }
        public bool isCustomFolder { get; set; }
        public bool isSelected { get; set; }
        public string copyToMediaLibFolder { get; set; }


        public MediaAssetViewModel() { }

        public MediaAssetViewModel(string name, string src, string type)
        {
            this.name = name;
            this.title = name;
            this.src = src;
            this.type = type;

            this.isFile = this.type == Constants.FILES_ASSET_TYPE_FILTER;
            this.isProjectFolder = this.type == Constants.PROJECT_FOLDERS_ASSET_TYPE_FILTER;
            this.isCustomFolder = this.type == Constants.CUSTOM_FOLDERS_ASSET_TYPE_FILTER;
            this.copyToMediaLibFolder = this.updateCopyDate(src);
        }

        public MediaAssetViewModel(string name, string src, string type, string subtype, string title) :
            this(name, src, type)
        {
            this.subtype = subtype;
            this.title = title;
        }

        public string updateCopyDate(string fileName)
        {
            String res = File.GetCreationTime(Constants.ROOT_APP_FOLDER + fileName).ToString("yyyy/MM/dd HH:mm:ss");
            return res;
        }

        public static string getFileSubType(string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                string ext = System.IO.Path.GetExtension(fileName).ToLower();               
               
                switch (ext) {
                    case ".jpg":
                    case ".jpeg":
                    case ".gif":
                    case ".png":
                    case ".tiff":
                        return Constants.IMAGES_FILE_TYPE_FILTER;
                    case ".doc":
                    case ".docx":
                    case ".txt":
                        return Constants.TEXTS_FILE_TYPE_FILTER;
                    case ".pdf":
                        return Constants.PDF_FILE_TYPE_FILTER;
                    case ".xls":
                    case ".xlsx":
                        return Constants.SPREADSHEETS_FILE_TYPE_FILTER;
                    case ".ppt":
                    case ".pptx":
                        return Constants.PRESENTATIONS_FILE_TYPE_FILTER;
                }
            }

            return Constants.OTHERS_FILE_TYPE_FILTER;
        }
    }
}