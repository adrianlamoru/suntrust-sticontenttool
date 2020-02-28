using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using st1001.data;
using st1001.website.Models;
using System.IO;
using System.Diagnostics;
using st1001.website.Helpers;

namespace st1001.website.ApiControllers
{
    [Authorize]
    public class DashboardController : ApiController
    {
        private st1001Entities db = new st1001Entities();

        // GET api/dashboard/users
        [Authorize(Roles = "Administrator,Super Admin,Super Admin")]
        [Route("Api/Dashboard/Users")]
        public IList<DashboardUserViewModel> GetDashboardUsers()
        {
            var list = new List<DashboardUserViewModel>();

            var users = db.Users.Where(u => !u.Deleted && u.Role != (int)UserRolEnum.SUPER_ADMIN);

            foreach (var user in users) {
                list.Add(new DashboardUserViewModel().CopyFromModel(user));
            }

            return list;
        }

        // GET api/dashboard/users
        [HttpGet]
        [Authorize(Roles = "Administrator,Super Admin,Super Admin")]
        [Route("Api/Dashboard/UsersByType/{userTypeId}")]
        public IList<DashboardUserViewModel> GetDashboardUsers(int userTypeId)
        {
            var list = new List<DashboardUserViewModel>();

            var users = db.Users.Where(u => !u.Deleted && u.Role != (int)UserRolEnum.SUPER_ADMIN && u.UserType == userTypeId);

            foreach (var user in users)
            {
                list.Add(new DashboardUserViewModel().CopyFromModel(user));
            }

            return list;
        }

        // GET api/dashboard/ActiveProjects
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        [Route("Api/Dashboard/ActiveProjects")]
        public IList<DashboardProjectViewModel> GetActiveProjects()
        {
            var list = new List<DashboardProjectViewModel>();

            var projects = db.Projects.Where(p => p.Archived == false && p.Offer.Deleted == false);

            foreach (var project in projects) {
                list.Add(new DashboardProjectViewModel().CopyFromModel(project));
            }

            return list;
        }

        // GET api/dashboard/ActiveProjectsByOfferType/offerTypeId        
        [HttpGet]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("Api/Dashboard/ActiveProjectsByOfferType/{offerTypeId}")]
        public IList<DashboardProjectViewModel> ActiveProjectsByOfferType(int offerTypeId)
        {
            var list = new List<DashboardProjectViewModel>();

            var projects = db.Projects.Where(p => p.Archived == false && p.Offer.Deleted == false && p.Offer.OfferType == offerTypeId);

            foreach (var project in projects)
            {
                list.Add(new DashboardProjectViewModel().CopyFromModel(project));
            }

            return list;
        }

        // GET api/dashboard/users
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        [Route("Api/Dashboard/ArchivedProjects")]
        public IList<DashboardProjectViewModel> GetArchivedProjects()
        {
            var list = new List<DashboardProjectViewModel>();

            var projects = db.Projects.Where(p => p.Archived == true && p.Offer.Deleted == false);

            foreach (var project in projects) {
                list.Add(new DashboardProjectViewModel().CopyFromModel(project));
            }

            return list;
        }

        // GET api/dashboard/GetArchivedProjectsByOfferType/offerTypeId    
        [HttpGet]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("Api/Dashboard/ArchivedProjectsByOfferType/{offerTypeId}")]
        public IList<DashboardProjectViewModel> GetArchivedProjectsByOfferType(int offerTypeId)
        {
            var list = new List<DashboardProjectViewModel>();

            var projects = db.Projects.Where(p => p.Archived == true && p.Offer.Deleted == false && p.Offer.OfferType == offerTypeId);

            foreach (var project in projects)
            {
                list.Add(new DashboardProjectViewModel().CopyFromModel(project));
            }

            return list;
        }

        // Get api/dashboard
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        public DashboardViewModel Get()
        {
            DashboardViewModel dashboardVM = new DashboardViewModel();
            dashboardVM.CopyFromModel(db.Projects.Where(p => p.Offer.Deleted == false), db.Users.Where(u => !u.Deleted && u.Role != (int)UserRolEnum.SUPER_ADMIN));
            return dashboardVM;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("Api/Dashboard/Search/{text}")]
        public SearchResultsViewModel Search(string text)
        {
            var result = new SearchResultsViewModel();

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "Missing text to search!"
                    });
            }

            result.Projects = SearchInProjects(text);
            result.Assets = SearchInMediaLibrary(text);

            return result;
        }

        private IEnumerable<DashboardProjectViewModel> SearchInProjects(string text)
        {
            var projectList = new List<DashboardProjectViewModel>();

            var projects = db.Projects.Where(p =>
                p.Offer.Deleted == false && (
                p.Offer.ID.ToString().Contains(text) ||
                p.Offer.Name.Contains(text)));

            foreach (var project in projects)
            {
                projectList.Add(new DashboardProjectViewModel().CopyFromModel(project));
            }

            return projectList;
        }

        private IEnumerable<MediaAssetViewModel> SearchInMediaLibrary(string text)
        {
            string[] files = null;
            var assetList = new List<MediaAssetViewModel>();
            string mediaPath = Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PATH;

            try
            {
                files = Directory.GetFiles(mediaPath, "*" + text + "*", SearchOption.AllDirectories);

                foreach (var f in files)
                {
                    string name = Path.GetFileName(f);
                    var asset = new MediaAssetViewModel(name, f.Replace(Constants.ROOT_APP_FOLDER, string.Empty), Constants.FILES_ASSET_TYPE_FILTER);
                    asset.subtype = MediaAssetViewModel.getFileSubType(f);

                    assetList.Add(asset);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                LogHelper.Log("SearchInMediaLibrary - " + ex.Message);
            }

            return assetList;
        }
    }
}
