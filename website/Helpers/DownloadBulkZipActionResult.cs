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
    public class DownloadBulkZipResult : ActionResult
    {
        IEnumerable<MediaAssetViewModel> assetList = null;

        public DownloadBulkZipResult(IEnumerable<MediaAssetViewModel> assetList)
        {
            this.assetList = assetList;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            using (ZipFile zf = new ZipFile())
            {
                foreach (var item in assetList)
                {
                    try
                    {
                        zf.AddItem(Constants.ROOT_APP_FOLDER + item.src, "/MediaAssets");
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Log("DownloadBulkZipResult - Adding item to zip" + ex.Message);
                    }
                }

                context.HttpContext.Response.ContentType = "application/zip";
                context.HttpContext.Response.AppendHeader("content-disposition", "attachment; filename=MediaAssets.zip");

                zf.Save(context.HttpContext.Response.OutputStream);
            }
        }

    }
}