using Newtonsoft.Json;
using st1001.data;
using st1001.website.ActionResults;
using st1001.website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace st1001.website.Controllers
{
    [Authorize]
    public class MediaLibraryController : Controller
    {
        //
        // GET: /MediaLibrary/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Super Admin")]
        [Route("Media/DownloadBulk")]
        public ActionResult DownloadBulk()
        {
            string assetListJson = Request["assetList"];

            if (!string.IsNullOrWhiteSpace(assetListJson)) {
                var assetList = JsonConvert.DeserializeObject<IList<MediaAssetViewModel>>(assetListJson);

                return new DownloadBulkZipResult(assetList);
            }

            return null;
        }
	}
}