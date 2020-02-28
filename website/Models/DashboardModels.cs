using st1001.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace st1001.website.Models
{
    public class DashboardViewModel
    {
        public IList<DashboardProjectViewModel> ActiveProjects { get; set; }
        public IList<DashboardProjectViewModel> ArchivedProjects { get; set; }
        public IList<DashboardProjectViewModel> AllProjects { get; set; }
        public IList<DashboardUserViewModel> Users { get; set; }

        public DashboardViewModel CopyFromModel(IEnumerable<Project> projects, IEnumerable<User> users)
        {
            ActiveProjects = new List<DashboardProjectViewModel>();
            ArchivedProjects = new List<DashboardProjectViewModel>();
            AllProjects = new List<DashboardProjectViewModel>();
            Users = new List<DashboardUserViewModel>();

            foreach (var user in users)
            {
                Users.Add(new DashboardUserViewModel().CopyFromModel(user));
            }

            foreach (var project in projects)
            {
                var projectViewModel = new DashboardProjectViewModel().CopyFromModel(project);

                if (project.Approved)
                {
                    ArchivedProjects.Add(projectViewModel);
                }
                else
                {
                    ActiveProjects.Add(projectViewModel);
                }

                AllProjects.Add(new DashboardProjectViewModel().CopyFromModel(project));
            }

            return this;
        }
    }

    public class DashboardProjectViewModel : IListItemViewModel<Project, DashboardProjectViewModel>
    {

        public string OfferName { get; set; }
        public int OfferID { get; set; }
        public string Author { get; set; }
        public string DateCreated { get; set; }
        public string DateArchived { get; set; }
        public string ProjectStatus { get; set; }
        public string Approval { get; set; }
        public string ContentIDs { get; set; }
        public string OfferType { get; set; }

        public DashboardProjectViewModel CopyFromModel(Project project)
        {
            this.OfferName = project.Offer.Name;
            this.OfferID = project.Offer.ID;
            this.ContentIDs = project.Offer.ContentIDs;
            this.Author = string.Format("{0} {1}", project.User.FirstName, project.User.LastName);
            this.DateCreated = project.UpdatedDate.ToString("o") + "Z";
            this.ProjectStatus = project.Archived? "Archived" : "Not Archived";
            if (project.ArchivedDate.HasValue) {
                this.DateArchived = project.ArchivedDate.Value.ToString("o") + 'Z';
            }
            this.Approval = project.Approved ? "Yes" : "No";
            this.OfferType = ModelHelper.GetOfferTypeName(project.Offer.OfferType);

            return this;
        }
    }

    public class DashboardUserViewModel : IListItemViewModel<User, DashboardUserViewModel>
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string UserType { get; set; }
        public DateTime DateJoined { get; set; }

        public DashboardUserViewModel CopyFromModel(User user)
        {
            this.ID = user.ID;
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Role = ModelHelper.GetRoleName(user.Role);
            this.DateJoined = user.Created;
            this.UserType = ModelHelper.GetUserTypeName(user.UserType);
            return this;
        }
    }
}