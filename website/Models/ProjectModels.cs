using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json.Linq;

using st1001.data;
using st1001.website.DAL.Security;

namespace st1001.website.Models
{
    public class ProjectCreateEditViewModel : ICreateEditViewModel<Project>
    {
        public int ID { get; set; }
        public int CreatedBy { get; set; }
        public bool Approved { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid Guid { get; set; }

        public void CopyToModelForCreate(Project model)
        {
            model.ID = this.ID;
            model.CreatedBy = ((CustomPrincipal)ModelHelper.GetCurrentUser()).UserId;
            model.Approved = false;
            model.UpdatedDate = DateTime.UtcNow;
            model.GUID = Guid.NewGuid();
        }

        public void CopyToModelForEdit(st1001Entities db, Project model)
        {
            //model.ID = this.ID;
            model.Approved = this.Approved;
            model.UpdatedDate = DateTime.UtcNow;
            //model.GUID = Guid.NewGuid();
        }
    }

    public class ProjectLayoutCreateEditViewModel : ICreateEditViewModel<Project>, IListItemViewModel<Project, ProjectLayoutCreateEditViewModel>
    {
        class ComponentComparer : IEqualityComparer<Component>
        {
            public bool Equals(Component c1, Component c2)
            {
                return c1.ContentID == c2.ContentID && c1.ComponentID == c2.ComponentID && c1.SectionID == c2.SectionID;
            }

            public int GetHashCode(Component obj)
            {
                return obj.GetHashCode();
            }
        }

        public int ID { get; set; }
        public String Name { get; set; }
        public String CreatedBy { get; set; }
        public String Description { get; set; }
        public bool Approved { get; set; }
        public string UpdatedDate { get; set; }
        public string ArchivedDate { get; set; }
        public System.Guid Guid { get; set; }

        public IList<LayoutViewModel> Layouts { get; set; }

        public ProjectLayoutCreateEditViewModel() {
            Layouts = new List<LayoutViewModel>();
        }

        public void CopyToModelForCreate(Project model)
        {
            throw new NotImplementedException();
        }

        public void CopyToModelForEdit(st1001Entities db, Project model)
        {
            //model.ID = this.ID;
            model.Approved = this.Approved;
            model.UpdatedDate = DateTime.UtcNow;
            //model.GUID = Guid.NewGuid();

            if (!string.IsNullOrWhiteSpace(this.ArchivedDate))
            {
                model.ArchivedDate = DateTime.Parse(this.ArchivedDate);
            }

            ComponentComparer compComparer = new ComponentComparer();

            foreach (var l in this.Layouts) 
            {
                if (l.LayoutDetail != null && l.LayoutDetail.ID > 0)
                {
                    var layoutDetail = model.Offer.LayoutDetails.SingleOrDefault(d => d.ContentID == l.ID);
                    if (layoutDetail != null)
                    {
                        if (string.IsNullOrWhiteSpace(l.LayoutDetail.Note))
                        {
                            db.LayoutDetails.Remove(layoutDetail);
                            //model.Offer.LayoutDetails.Remove(layoutDetail);
                        }
                        else
                        {
                            l.LayoutDetail.CopyToModelForEdit(layoutDetail);
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(l.LayoutDetail.Note))
                    {
                        var layoutDetail = new LayoutDetail();
                        l.LayoutDetail.CopyToModelForEdit(layoutDetail);
                        model.Offer.LayoutDetails.Add(layoutDetail);
                    }
                }

                foreach (var s in l.Sections)
                {
                    foreach (var c in s.Components)
                    {
                        var comp = new Component();
                        comp.ContentID = l.ID != null ? l.ID.Trim() : string.Empty;
                        comp.SectionID = s.ID;
                        comp.ComponentID = c.TypeID;
                        comp.Inactive = c.Inactive;
                        comp.Data = c.Data.ToString(Newtonsoft.Json.Formatting.None);

                        if (!model.Components.Contains(comp, compComparer))
                        {
                            model.Components.Add(comp);
                        }
                            
                    }
                }
            }
        }

        public ProjectLayoutCreateEditViewModel CopyFromModel(Project model)
        {
            this.ID = model.ID;
            this.Approved = model.Approved;
            this.UpdatedDate = model.UpdatedDate.ToString("o");
            this.ArchivedDate = model.ArchivedDate != null ? model.ArchivedDate.Value.ToString("o") : string.Empty;
            this.Guid = model.GUID ?? new Guid();

            if (model.Offer != null) {
                this.Name = model.Offer.Name;
                this.Description = model.Offer.Description;
            }

            if (model.User != null)
            {
                this.CreatedBy = string.Format("{0} {1}", model.User.FirstName, model.User.LastName);
            }

            if (model.Components != null) 
            {
                var layoutGroups = model.Components.GroupBy(l => l.ContentID);

                foreach (var layoutGroup in layoutGroups)
                {
                    if (model.Offer.ContentIDs.Split(',').Contains(layoutGroup.Key))
                    {
                        var layoutVM = new LayoutViewModel();
                        layoutVM.ID = layoutGroup.Key != null ? layoutGroup.Key.Trim() : string.Empty;

                        var layoutDetailVM = new LayoutDetailViewModel();

                        var layoutDetail = model.Offer.LayoutDetails.SingleOrDefault(d => d.ContentID == layoutVM.ID);
                        if (layoutDetail != null)
                        {
                            layoutDetailVM.CopyFromModel(layoutDetail);
                            layoutVM.LayoutDetail = layoutDetailVM;
                        }
                        else
                        {
                            layoutDetailVM.OfferID = model.Offer.ID;
                            layoutDetailVM.OfferTypeID = model.Offer.OfferType;
                            layoutDetailVM.ContentID = layoutVM.ID;
                            layoutVM.LayoutDetail = layoutDetailVM;
                        }

                        var sectionGroups = layoutGroup.GroupBy(s => s.SectionID);

                        foreach (var sectionGroup in sectionGroups)
                        {
                            var sectionVM = new SectionViewModel();
                            sectionVM.ID = sectionGroup.Key;

                            foreach (var component in sectionGroup)
                            {
                                var componentVM = new ComponentViewModel();
                                componentVM.ID = component.ID;
                                componentVM.TypeID = component.ComponentID;
                                componentVM.Inactive = component.Inactive;

                                //TODO: An error is raising here when component.Data contain a simple string
                                componentVM.Data = JObject.Parse(component.Data);

                                sectionVM.Components.Add(componentVM);
                            }

                            layoutVM.Sections.Add(sectionVM);
                        }

                        this.Layouts.Add(layoutVM);
                    }
                }
            }

            //Include every possible Layout
            if (model.Offer != null)
            {
                foreach (string lID in model.Offer.ContentIDs.Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(lID))
                    {
                        if (!this.Layouts.Any(l => l.ID.Trim() == lID.Trim()))
                        {
                            var contentID = lID != null ? lID.Trim() : string.Empty;

                            var layoutDetailVM = new LayoutDetailViewModel();

                            var layoutDetail = model.Offer.LayoutDetails.SingleOrDefault(d => d.ContentID == contentID);
                            if (layoutDetail != null)
                            {
                                layoutDetailVM.CopyFromModel(layoutDetail);
                            }
                            else
                            {
                                layoutDetailVM.OfferID = model.Offer.ID;
                                layoutDetailVM.OfferTypeID = model.Offer.OfferType;
                                layoutDetailVM.ContentID = contentID;
                            }
                            this.Layouts.Add(new LayoutViewModel() { ID = contentID, LayoutDetail = layoutDetailVM });
                        }
                    }
                }
            }

            //Order the Layouts 
            this.Layouts = this.Layouts.OrderBy(l => l.ID).ToList();

            return this;
        }
    }

    public class LayoutDetailViewModel
    {
        public int ID { get; set; }
        public int OfferID { get; set; }
        public int OfferTypeID { get; set; }
        public string ContentID { get; set; }
        public string Note { get; set; }

        public LayoutDetailViewModel() { }

        public LayoutDetailViewModel(LayoutDetail detail)
        {
            CopyFromModel(detail);
        }

        public void CopyFromModel(LayoutDetail detail)
        {
            this.ID = detail.ID;
            this.OfferID = detail.OfferID;
            this.OfferTypeID = detail.Offer.OfferType;
            this.ContentID = detail.ContentID;
            this.Note = detail.Note;
        }

        public void CopyToModelForEdit(LayoutDetail detail)
        {
            detail.ID = this.ID;
            detail.OfferID = this.OfferID;
            detail.ContentID = this.ContentID;
            detail.Note = this.Note;
        }
    }

    public class LayoutViewModel
    {
        public string ID { get; set; }
        public LayoutDetailViewModel LayoutDetail { get; set; }
        public IList<SectionViewModel> Sections { get; set; }

        public LayoutViewModel() {
            Sections = new List<SectionViewModel>();
            LayoutDetail = new LayoutDetailViewModel();
        }
    }

    public class SectionViewModel
    {
        public int ID { get; set; }
        public IList<ComponentViewModel> Components { get; set; }

        public SectionViewModel() {
            Components = new List<ComponentViewModel>();
        }
    }

    public class ComponentViewModel
    {
        public int ID { get; set; }
        public int TypeID { get; set; }
        public bool Inactive { get; set; }
        public JObject Data { get; set; }
    }
}