using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Diagnostics;
using System.Threading.Tasks;
using System.IO;
using st1001.website.Models;
using st1001.data;
using System.Web.Http.Description;
using System.Drawing;
using st1001.website.Helpers;

namespace st1001.website.ApiControllers
{
    [Authorize]
    public class MediaController : ApiController
    {

        private st1001Entities db = new st1001Entities();

        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("api/Media/ImagesForProject/{projectId}")]
        public IEnumerable<MediaAssetViewModel> GetImagesForProject(int projectId)
        {
            string[] files = null;
            string projectPath = string.Empty;
            var list = new List<MediaAssetViewModel>();

            //Get images from the Project's folder and every Public project's folder
            var projects = db.Projects.Where(p => p.Approved || p.ID == projectId);

            foreach (var p in projects)
            {
                projectPath = Constants.MEDIA_LIBRARY_PROJECT_PATH + '/' + p.ID;

                try
                {
                    if (Directory.Exists(Constants.ROOT_APP_FOLDER + projectPath))
                    {
                        files = Directory.GetFiles(Constants.ROOT_APP_FOLDER + projectPath, "*", SearchOption.AllDirectories);

                        foreach (var f in files)
                        {
                            string name = Path.GetFileName(f);

                            if (MediaAssetViewModel.getFileSubType(name) == Constants.IMAGES_FILE_TYPE_FILTER)
                            {
                                list.Add(new MediaAssetViewModel(name, f.Replace(Constants.ROOT_APP_FOLDER, string.Empty), Constants.FILES_ASSET_TYPE_FILTER));
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Log("GetImagesForProject - " + ex.Message);
                }
            }

            //Get all images form public folders
            files = Directory.GetFiles(Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PUBLIC_PATH, "*", SearchOption.AllDirectories);

            foreach (var f in files)
            {
                string name = Path.GetFileName(f);
                list.Add(new MediaAssetViewModel(name, f.Replace(Constants.ROOT_APP_FOLDER, string.Empty), Constants.FILES_ASSET_TYPE_FILTER));
            }

            return list;
        }


        [HttpPost]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("api/Media/Upload/{projectId}")]
        public async Task<HttpResponseMessage> PostFormData(int projectId)
        {  
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                //throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Error in Request.Content.IsMimeMultipartContent(). Value False");
            }

            string projectFolderPath = Constants.MEDIA_LIBRARY_PROJECT_PATH + "/" + projectId;
            string root = Constants.ROOT_APP_FOLDER + projectFolderPath;

            try
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error creating Directory [{0}]. {1}", root, e.Message));
            }


            var provider = new MultipartFormDataStreamProvider(root);

            List<string> files = new List<string>();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error in Request.Content.ReadAsMultipartAsync(provider). {0}", e.Message));
            }

            foreach (MultipartFileData file in provider.FileData)
            {
                string originalFilename = file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                string newFileName = Path.Combine(root, originalFilename);
                
                if (MediaAssetViewModel.getFileSubType(originalFilename) == Constants.IMAGES_FILE_TYPE_FILTER)
                {
                    Image newImage = Image.FromFile(file.LocalFileName);

                    if (!MatchSize(newImage))
                    {
                        int newImageWidth = newImage.Width;
                        int newImageHeight = newImage.Height;

                        try
                        {
                            newImage.Dispose();
                            File.Delete(file.LocalFileName);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Log("api/Media/Upload/{projectId} - " + ex.Message);
                            //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error uploading/deleting file [{0}]. {1}", newFileName, e.Message));
                        }

                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("The file does not have an allowed dimension. Current dimension: [{0}px] [{1}px]", newImageWidth, newImageHeight));
                    }
                    else
                    {
                        newImage.Dispose();
                    }
                }

                try
                {
                    if (File.Exists(newFileName))
                    {
                        File.Delete(newFileName);
                    }
                }
                catch (System.Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error Deleting file [{0}]. {1}", newFileName, e.Message));
                }

                try
                {   
                    File.Move(file.LocalFileName, newFileName);
                    File.SetLastWriteTime(newFileName, DateTime.Now);

                    files.Add(projectFolderPath + '/' + originalFilename);
                }
                catch (System.Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error Moving file [{0}] to [{1}] {2}", file.LocalFileName, newFileName, e.Message));
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, files);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("api/Media/UploadToFolder")]
        public async Task<HttpResponseMessage> PostAssetData()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                //throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                return Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Error in Request.Content.IsMimeMultipartContent(). Value False");
            }

            string folder = Request.RequestUri.ParseQueryString().Get("folder");
            string root = Constants.ROOT_APP_FOLDER + (string.IsNullOrWhiteSpace(folder) ? Constants.MEDIA_LIBRARY_PUBLIC_PATH : folder);

            try
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error creating Directory [{0}]. {1}", root, e.Message));
            }

            var provider = new MultipartFormDataStreamProvider(root);
            var files = new List<MediaAssetViewModel>();

            try
            {
                await Request.Content.ReadAsMultipartAsync(provider);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error in Request.Content.ReadAsMultipartAsync(provider). {0}", e.Message));
            }

            foreach (MultipartFileData file in provider.FileData)
            {
                string originalFilename = file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
                string newFileName = Path.Combine(root, originalFilename);

                if (MediaAssetViewModel.getFileSubType(originalFilename) == Constants.IMAGES_FILE_TYPE_FILTER)
                {
                    Image newImage = Image.FromFile(file.LocalFileName);

                    if (!MatchSize(newImage))
                    {
                        int newImageWidth = newImage.Width;
                        int newImageHeight = newImage.Height;

                        try
                        {
                            newImage.Dispose();
                            File.Delete(file.LocalFileName);
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Log("api/Media/UploadToFolder - " + ex.Message);
                            //return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error uploading/deleting file [{0}]. {1}", newFileName, e.Message));
                        }

                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, string.Format("The file does not have an allowed dimension. Current dimension: [{0}px] [{1}px]", newImageWidth, newImageHeight));
                    }
                    else
                    {
                        newImage.Dispose();
                    }
                }

                try
                {
                    if (File.Exists(newFileName))
                    {
                        File.Delete(newFileName);
                    }
                }
                catch (System.Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error Deleting file [{0}]. {1}", newFileName, e.Message));
                }

                try
                {
                    File.Move(file.LocalFileName, newFileName);
                    File.SetLastWriteTime(newFileName, DateTime.Now);

                    var asset = createMediaAssetViewModel(newFileName, Constants.FILES_ASSET_TYPE_FILTER);

                    if (asset.src[0] != '/')
                    {
                        asset.src = '/' + asset.src;
                    }

                    asset.subtype = MediaAssetViewModel.getFileSubType(newFileName);

                    files.Add(asset);
                }
                catch (System.Exception e)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, string.Format("Error Moving file [{0}] to [{1}] {2}", file.LocalFileName, newFileName, e.Message));
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, files);
        }

        SizeF[] ValidImagesSizes = new SizeF[] {
            new SizeF { Width = 828, Height = 332},
            new SizeF { Width = 828, Height = 388},
            new SizeF { Width = 736, Height = 344},
            new SizeF { Width = 610, Height = 427},
            new SizeF { Width = 736, Height = 344},
            new SizeF { Width = 166, Height = 133},
            new SizeF { Width = 240, Height = 96},
            new SizeF { Width = 324, Height = 192},
            new SizeF { Width = 562, Height = 264},
            new SizeF { Width = 100, Height = 150},
            new SizeF { Width = 150, Height = 100},
            new SizeF { Width = 200, Height = 100},
            new SizeF { Width = 320, Height = 128},
            new SizeF { Width = 320, Height = 150},
            new SizeF { Width = 600, Height = 268},
            new SizeF { Width = 600, Height = 118},
            new SizeF { Width = 374, Height = 180},
            new SizeF { Width = 0, Height = 240}
        };

        private bool MatchSize(Image image)
        {
            if (image == null)
            {
                return false;
            }
            foreach (var size in ValidImagesSizes)
            {
                
                if ((size.Width == 0 ||  image.Width == size.Width) && image.Height == size.Height)
                {
                    return true;
                }
            }

            return false;
        }

        private MediaAssetViewModel createMediaAssetViewModel(string assetFile, string assetType)
        {
            MediaAssetViewModel asset = new MediaAssetViewModel(
                    Path.GetFileName(assetFile),
                    assetFile.Replace(Constants.ROOT_APP_FOLDER, string.Empty),
                    assetType);

            asset.title = asset.name;

            //TODO: Get extra info like owner and created date. It should be gotten from a new table (filled in each addFolder or uploadMedia)

            return asset;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("api/Media/AllAssets")]
        //data.filter is optional Constants.[ALL_ASSET_TYPE_FILTER | PROJECT_FOLDERS_ASSET_TYPE_FILTER | CUSTOM_FOLDERS_ASSET_TYPE_FILTER | FILES_ASSET_TYPE_FILTER]
        [ResponseType(typeof(IEnumerable<MediaAssetViewModel>))]
        public IHttpActionResult GetAllAssets([FromBody] MediaRequestParams data)
        {
            string[] files = null;
            string[] folders = null;
            var list = new List<MediaAssetViewModel>();

            if (string.IsNullOrWhiteSpace(data.filter))
            {
                data.filter = Constants.ALL_ASSET_TYPE_FILTER;
            }

            try
            {
                if (data.filter == Constants.ALL_ASSET_TYPE_FILTER || data.filter == Constants.PROJECT_FOLDERS_ASSET_TYPE_FILTER)
                {
                    folders = Directory.GetDirectories(Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PROJECT_PATH);

                    foreach (var folder in folders)
                    {
                        MediaAssetViewModel projectFolder = createMediaAssetViewModel(folder, Constants.PROJECT_FOLDERS_ASSET_TYPE_FILTER);

                        //TODO: Get the list of offers to get the title from offer.Name
                        projectFolder.title = projectFolder.name + "-Project";

                        list.Add(projectFolder);
                    }
                }

                if (data.filter == Constants.ALL_ASSET_TYPE_FILTER || data.filter == Constants.CUSTOM_FOLDERS_ASSET_TYPE_FILTER)
                {
                    folders = Directory.GetDirectories(Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PUBLIC_PATH);

                    foreach (var folder in folders)
                    {
                        list.Add(createMediaAssetViewModel(folder, Constants.CUSTOM_FOLDERS_ASSET_TYPE_FILTER));
                    }
                }

                if (data.filter == Constants.ALL_ASSET_TYPE_FILTER || data.filter == Constants.FILES_ASSET_TYPE_FILTER)
                {
                    files = Directory.GetFiles(Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PUBLIC_PATH);

                    foreach (var f in files)
                    {
                        var asset = createMediaAssetViewModel(f, Constants.FILES_ASSET_TYPE_FILTER);
                        asset.subtype = MediaAssetViewModel.getFileSubType(f);

                        list.Add(asset);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            ReasonPhrase = ex.Message
                        });
            }

            return Ok(list);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("api/Media/AssetsInFolder")]
        //data.path is required. Full relative Folder path, Ex: /MediaLibrary/Public/Test
        //data.filter is optional Constants.[ALL_FILE_TYPE_FILTER | IMAGES_FILE_TYPE_FILTER | TEXTS_FILE_TYPE_FILTER | 
        //          SPREADSHEETS_FILE_TYPE_FILTER | PRESENTATIONS_FILE_TYPE_FILTER | OTHERS_FILE_TYPE_FILTER]
        [ResponseType(typeof(IEnumerable<MediaAssetViewModel>))]
        public IHttpActionResult GetAssetsInFolder([FromBody] MediaRequestParams data)
        {
            string[] files = null;
            var list = new List<MediaAssetViewModel>();

            if (data == null || data.path == null || !data.path.Contains(Constants.MEDIA_LIBRARY_PATH))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                        {
                            ReasonPhrase = "Invalid path!"
                        });
            }

            if (string.IsNullOrWhiteSpace(data.filter))
            {
                data.filter = Constants.ALL_FILE_TYPE_FILTER;
            }

            try
            {
                if (Directory.Exists(Constants.ROOT_APP_FOLDER + data.path))
                {
                    files = Directory.GetFiles(Constants.ROOT_APP_FOLDER + data.path);

                    foreach (var f in files)
                    {
                        var asset = createMediaAssetViewModel(f, Constants.FILES_ASSET_TYPE_FILTER);
                        asset.subtype = MediaAssetViewModel.getFileSubType(f);

                        if (data.filter == Constants.ALL_FILE_TYPE_FILTER || data.filter == asset.subtype)
                        {
                            list.Add(asset);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log("GetAssetsInFolder - " + ex.Message);
            }


            return Ok(list);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Super Admin")]
        [Route("api/Media/CreateFolder")]
        //data.path is required. Folder name to create
        [ResponseType(typeof(MediaAssetViewModel))]
        public IHttpActionResult CreateFolder([FromBody] MediaRequestParams data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.path))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "Null or Empty folder name!"
                    });
            }

            string folder = Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PUBLIC_PATH + "/" + data.path;

            if (Directory.Exists(folder))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "Folder already exists!"
                    });
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(folder);

                    return Ok(new MediaAssetViewModel(data.path, Constants.MEDIA_LIBRARY_PUBLIC_PATH + "/" + data.path, Constants.CUSTOM_FOLDERS_ASSET_TYPE_FILTER));
                }
                catch (Exception ex)
                {
                    throw new HttpResponseException(
                        new HttpResponseMessage(HttpStatusCode.InternalServerError)
                        {
                            ReasonPhrase = ex.Message
                        });
                }
            }

        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Super Admin")]
        [Route("api/Media/DeleteFolder")]
        //data.path is required. Folder src to delete
        public IHttpActionResult DeleteFolder([FromBody] MediaRequestParams data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.path))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "Null or Empty folder name!"
                    });
            }

            string folder = Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PUBLIC_PATH + "/" + data.path;

            try
            {
                Directory.Delete(folder);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = ex.Message
                    });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Super Admin")]
        [Route("api/Media/DeleteFile")]
        //data.path is required. File src to delete
        public IHttpActionResult DeleteFile([FromBody] MediaRequestParams data)
        {
            if (data == null || string.IsNullOrWhiteSpace(data.path))
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        ReasonPhrase = "Null or Empty folder name!"
                    });
            }

            string file = Constants.ROOT_APP_FOLDER + "/" + data.path;

            try
            {
                File.Delete(file);

                return Ok();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(
                    new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = ex.Message
                    });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Super Admin")]
        [Route("api/Media/DeleteBulk")]
        //data.path is required. Folder src to delete
        [ResponseType(typeof(IList<MediaAssetViewModel>))]
        public IHttpActionResult DeleteBulk([FromBody] IEnumerable<MediaAssetViewModel> assetList)
        {
            var deleteds = new List<MediaAssetViewModel>();

            if (assetList != null)
            {
                foreach (var item in assetList)
                {
                    string file = Constants.ROOT_APP_FOLDER + item.src;
                    //.Replace(Constants.MEDIA_LIBRARY_PATH, Constants.MEDIA_LIBRARY_FOLDER_NAME);
                    //.Replace('/', Path.DirectorySeparatorChar);

                    try
                    {
                        File.Delete(file);
                        deleteds.Add(item);
                    }
                    catch (Exception ex)
                    {
                        //Nothing to do
                        LogHelper.Log("DeleteBulk - " + ex.Message);
                    }
                }
            }

            return Ok(deleteds);
        }
    }
}
