using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using st1001.data;
using st1001.website.DAL.Security;

namespace st1001.website.Models
{
    public class OfferCreateEditViewModel : ICreateEditViewModel<Offer>
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ContentIDs { get; set; }
        public string CreateByExternal { get; set; }
        public string OfferType { get; set; }

        public void CopyToModelForCreate(Offer model)
        {
            model.ID = this.ID;
            model.Name = this.Name;
            model.Description = this.Description;
            model.CreatedBy = ((CustomPrincipal)ModelHelper.GetCurrentUser()).UserId;
            model.UpdatedDate = DateTime.Now;
            model.ContentIDs = this.ContentIDs;
            model.HasProject = false;
            model.CreateByExternal = this.CreateByExternal;
            model.OfferType = Int32.Parse(this.OfferType);
        }

        public void CopyToModelForEdit(st1001Entities db, Offer model)
        {
            //model.ID = this.ID;
            model.Name = this.Name;
            model.Description = this.Description;
            //model.CreatedBy = this.CreatedBy;
            model.UpdatedDate = DateTime.UtcNow;
            model.ContentIDs = this.ContentIDs;
            //model.HasProject = false;
            model.CreateByExternal = this.CreateByExternal;
        }

        public OfferCreateEditViewModel CopyFromModel(Offer model)
        {
            this.ID = model.ID;
            this.Name = model.Name;
            this.ContentIDs = model.ContentIDs;
            this.Description = model.Description;
            this.OfferType = ModelHelper.GetOfferTypeName(model.OfferType);

            return this;
        }
    }

    public class OfferListItemViewModel : IListItemViewModel<Offer, OfferListItemViewModel>
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CreateByName { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool HasProject { get; set; }
        public string CreateByExternal { get; set; }
        public string OfferType { get; set; }

        public OfferListItemViewModel CopyFromModel(Offer offerModel)
        {
            this.ID = offerModel.ID;
            this.Name = offerModel.Name;
            this.Description = offerModel.Description;
            //this.CreatedByName = getName();
            this.UpdatedDate = offerModel.UpdatedDate;
            this.HasProject = offerModel.HasProject;
            this.CreateByExternal = offerModel.CreateByExternal;
            this.OfferType = ModelHelper.GetUserTypeName(offerModel.OfferType);

            return this;
        }
    }

}
