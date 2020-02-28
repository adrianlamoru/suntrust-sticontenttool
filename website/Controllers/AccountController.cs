using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using st1001.website.Models;
using st1001.website;
using st1001.data;
using Newtonsoft.Json;
using System.Web.Configuration;
using PagedList;
using System.Data;
using System.Data.Entity;

namespace st1001.website.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private st1001Entities db = new st1001Entities(ModelHelper.cnnString);
        
        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // GET: /UserProfile/
        public ActionResult UserProfile()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
               
                string email = model.Email;
                string password = ModelHelper.EncodePasswordToBase64(model.Password ?? "");

                var user = db.Users.Where(u => u.Email == email && u.Password == password && !u.Deleted).FirstOrDefault();

                // User found in the database
                if (user == null)
                {
                    ModelState.AddModelError("", "The email or password you have entered is incorrect. Please try again.");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);

                    return RedirectToLocal(returnUrl);
                }
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();

            FormsAuthentication.SignOut();

            return RedirectToAction("Dashboard", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Super Admin")] 
        public ActionResult GetUsers(string searchString, int? page)
        {
            try
            {
                List<User> users = db.Users.Where(u => !u.Deleted).ToList();

                ViewBag.CurrentSearch = searchString;

                if (!string.IsNullOrEmpty(searchString))
                {
                    string search = searchString.ToUpper();
                    users = users.Where(s => s.Email.ToUpper().Contains(search)
                                                    || ModelHelper.GetRoleName(s.Role).ToUpper().Contains(search)
                                                    || s.Email.ToUpper().Contains(search)
                                                    || (!string.IsNullOrEmpty(s.FirstName) && s.FirstName.ToUpper().Contains(search))
                                                    || (!string.IsNullOrEmpty(s.LastName) && s.LastName.ToUpper().Contains(search))
                                                ).ToList();
                }

                int pageSize = Convert.ToInt16(WebConfigurationManager.AppSettings["AmountPerPage"]);
                int pageNumber = (page ?? 1);

                return View("UserList", users.ToPagedList(pageNumber, pageSize));
            }
            catch
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Administrator,Super Admin")] //Roles.Admin
        public ActionResult CreateUser()
        {
            try
            {
                return View("CreateUser");
            }
            catch
            {
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Super Admin")] //Roles.Admin
        public ActionResult CreateUser(CreateEditModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Role == -1)
                {
                    ModelState.AddModelError("Role", "*Please select role");
                }
                else if (db.Users.Where(u => !u.Deleted && u.Email == model.Email).FirstOrDefault() != null)
                {
                    ModelState.AddModelError("Email", "*This email is being used.");
                }
                else
                {
                    User user = new User();

                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Password = ModelHelper.EncodePasswordToBase64(model.Password);
                    user.Role = model.Role;
                    user.UserType = model.UserType;

                    db.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("GetUsers");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Administrator,Super Admin")] 
        public ActionResult EditUser(string id)
        {
            User user = db.Users.Where(u => !u.Deleted && u.Email == id).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }

            CreateEditModel model = new CreateEditModel();
            model.ID = user.ID;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.Role = user.Role;
            model.UserType = user.UserType;
            model.Email = user.Email;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Super Admin")] 
        public ActionResult EditUser(CreateEditModel model)
        {
            if (!string.IsNullOrEmpty(model.FirstName) && !string.IsNullOrEmpty(model.LastName) && !string.IsNullOrEmpty(model.Email) && model.Role != -1)
            {
                User user = db.Users.Where(u => u.ID == model.ID).FirstOrDefault();

                user.Role = model.Role;
                user.UserType = model.UserType;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetUsers");
            }
            
            if (model.Role == -1)
            {
                ModelState.AddModelError("Role", "*Please select role");
            }
            
            return View(model);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Dashboard", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
