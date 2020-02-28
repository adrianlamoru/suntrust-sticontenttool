using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.IO;
using st1001.data;
using st1001.website.Models;
using st1001.website.Helpers;

namespace st1001.website.ApiControllers
{
    [Authorize]
    public class ProjectController : ApiController
    {
        private st1001Entities db = new st1001Entities();

        // GET: api/Project
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        public IQueryable<Project> GetProjects()
        {
            return db.Projects.Where(p => p.Offer.Deleted == false);
        }

        // GET: api/Project/5
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [ResponseType(typeof(ProjectLayoutCreateEditViewModel))]
        public IHttpActionResult GetProject(int id)
        {
            Project project = db.Projects.FirstOrDefault(p => p.Offer.Deleted == false && p.ID == id);

            if (project == null)
            {
                return NotFound();
            }

            var pVM = new ProjectLayoutCreateEditViewModel();
            pVM.CopyFromModel(project);

            return Ok(pVM);
        }

        // POST: api/Project/5
        [HttpPost]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("api/Project/Update/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult UpdateProject(int id, [FromBody] ProjectLayoutCreateEditViewModel projectLayoutViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != projectLayoutViewModel.ID)
            {
                return BadRequest();
            }

            Project projectModel = db.Projects.Find(id);

            if (projectModel == null)
            {
                return NotFound();
            }

            TryToCreateTheFolderProject(projectLayoutViewModel.ID);
            CopyImagesToLocalFolder(projectLayoutViewModel);

            //Remove the existing Components to be replaced by the new ones
            db.Components.RemoveRange(projectModel.Components);

            projectLayoutViewModel.CopyToModelForEdit(db, projectModel);

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        //Copy every used image to the own project's folder and update the path
        private void CopyImagesToLocalFolder(ProjectLayoutCreateEditViewModel projectLayoutViewModel)
        {
            string projectFolder = Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PROJECT_PATH + "/" + projectLayoutViewModel.ID;
            var targetFolder = new DirectoryInfo(projectFolder);

            foreach (var l in projectLayoutViewModel.Layouts)
            {
                foreach (var s in l.Sections)
                {
                    foreach (var c in s.Components)
                    {
                        if (!c.Inactive && c.TypeID == Constants.MAIN_IMAGE_COMPONENT_ID && c.Data != null && !Convert.ToBoolean(c.Data["isExternal"]))
                        {
                            string imagePath = Constants.ROOT_APP_FOLDER + Convert.ToString(c.Data["src"]);
                            var cFile = new FileInfo(imagePath);

                            if (cFile.Directory.FullName != targetFolder.FullName)
                            {
                                try
                                {
                                    var fileName = targetFolder.FullName + Path.DirectorySeparatorChar + cFile.Name;

                                    if (File.Exists(fileName)) {
                                        cFile.CopyTo(fileName);
                                        c.Data["src"] = Constants.MEDIA_LIBRARY_PROJECT_PATH + "/" + projectLayoutViewModel.ID + "/" + cFile.Name;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Log("CopyImagesToLocalFolder - [" + cFile + "] - " + ex.Message);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void TryToCreateTheFolderProject(int projectID)
        {
            string folder = Constants.ROOT_APP_FOLDER + Constants.MEDIA_LIBRARY_PROJECT_PATH + Path.DirectorySeparatorChar + projectID;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
        }

        // POST: api/Project
        [HttpPost]
        [Authorize(Roles = "Administrator,Project Manager,Super Admin")]
        [Route("api/Project/Create")]
        [ResponseType(typeof(ProjectLayoutCreateEditViewModel))]
        public IHttpActionResult CreateProject(ProjectCreateEditViewModel projectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ProjectExists(projectViewModel.ID))
            {
                return Conflict();
            }

            var newProject = new Project();
            projectViewModel.CopyToModelForCreate(newProject);

            db.Projects.Add(newProject);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ProjectExists(newProject.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            try
            {
                TryToCreateTheFolderProject(newProject.ID);
            }
            catch (Exception e)
            {
                //This error is not a show stopper at this point 
                LogHelper.Log("CreateProject - Creating folder's project - " + e.Message);
            }

            return Ok(new ProjectLayoutCreateEditViewModel().CopyFromModel(newProject));
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrator, Project Manager, Editor,Super Admin")]
        [Route("api/Project/Delete/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RemoveProject(int id)
        {
            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Project Manager,Super Admin")]
        [Route("api/Project/MoveProjectsTo/{status}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult MoveProjectsTo(string status, [FromBody] List<string> ids)
        {
            var archived = false;

            if (status.ToLower().Equals("archive"))
            {
                archived = true;
            }
            else if (status.ToLower().Equals("active"))
            {
                archived = false;
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            var projects = db.Projects.Where(p => ids.Contains(p.ID.ToString())).ToList();
            projects.ForEach(p =>
                    {
                        if (archived)
                        {
                            p.ArchivedDate = DateTime.UtcNow;
                        }

                        p.Archived = archived;

                    }
                );

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Project Manager,Super Admin")]
        [Route("api/Project/Aproved/{id}/{status}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult ChangeProjectStatus(int id, bool status)
        {
            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return NotFound();
            }

            project.Approved = status;
            if (status == true)
            {
                project.ApprovedDate = DateTime.UtcNow;
            }
            else
            {
                project.ApprovedDate = null;
            }

            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ID == id) > 0;
        }
    }
}