using st1001.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

using st1001.website.Models;
using System.Data.Entity.Infrastructure;

namespace st1001.website.ApiControllers
{
    public class UserController : ApiController
    {
        private st1001Entities db = new st1001Entities();

        // GET api/User/1
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        [ResponseType(typeof(CreateEditModel))]
        public IHttpActionResult GetUser(int id)
        {
            var currentUser = (st1001.website.DAL.Security.CustomPrincipal)ModelHelper.GetCurrentUser();
            if (!(currentUser.IsInRole(Helpers.Constants.ROLE_ADMIN) || currentUser.IsInRole(Helpers.Constants.ROLE_SA)) && currentUser.UserId != id)
            {
                return Unauthorized();
            }

            User user = db.Users.Where(u => u.ID == id && !u.Deleted).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            CreateEditModel model = new CreateEditModel();
            model.ID = user.ID;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Role = user.Role;
            model.Password = user.Password;
            model.ConfirmPassword = user.Password;
            model.Email = user.Email;
            model.UserType = user.UserType;

            return Ok(model);
        }


        [HttpPost]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        [Route("api/User/Post")]
        [ResponseType(typeof(CreateEditModel))]
        public IHttpActionResult PostUser(CreateEditModel model)
        {
            var currentUser = (st1001.website.DAL.Security.CustomPrincipal)ModelHelper.GetCurrentUser();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Role == -1)
            {
                ModelState.AddModelError("Role", "*Please select role");
            }

            else if (model.ID > 0) // Edit mode
            {
                if (!(currentUser.IsInRole(Helpers.Constants.ROLE_ADMIN) || currentUser.IsInRole(Helpers.Constants.ROLE_SA)) && currentUser.UserId != model.ID)
                {
                    return Unauthorized();
                }

                var user = db.Users.Where(u => u.ID == model.ID && !u.Deleted).FirstOrDefault();

                if (user != null)
                {
                    user.Role = model.Role;
                    user.UserType = model.UserType;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    if (user.Password != model.Password)
                    {
                        user.Password = ModelHelper.EncodePasswordToBase64(model.Password);
                    }

                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            else if (db.Users.Where(u => !u.Deleted && u.Email.ToLower() == model.Email.ToLower()).FirstOrDefault() != null)
            {
                return Conflict(); //*This email is being used. 
                //ModelState.AddModelError("Email", "*This email is being used.");
            }
            else if (currentUser.IsInRole(Helpers.Constants.ROLE_ADMIN))
            {
                User user = new User();

                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Password = ModelHelper.EncodePasswordToBase64(model.Password);
                user.Role = model.Role;
                user.UserType = model.UserType;
                user.Deleted = false;
                user.Created = DateTime.UtcNow;

                db.Users.Add(user);
                db.SaveChanges();
            }
            else
            {
                return Unauthorized();
            }

            return Ok(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Super Admin")] 
        [Route("api/User/Delete/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Deleted = true;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
