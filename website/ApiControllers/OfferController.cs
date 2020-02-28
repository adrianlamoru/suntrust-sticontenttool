using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using st1001.data;
using st1001.website.Models;
using System;

namespace st1001.website.ApiControllers
{
    [Authorize]
    public class OfferController : ApiController
    {
        private st1001Entities db = new st1001Entities();

        // GET api/Offer
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        public IList<OfferListItemViewModel> GetOffers()
        {
            var list = new List<OfferListItemViewModel>();

            foreach (var offer in db.Offers.Where(o => o.Deleted == false).ToList())
            {
                list.Add(new OfferListItemViewModel().CopyFromModel(offer));
                
            }

            return list;
        }
        
        // GET api/Offer/offerTypeId
        [HttpGet]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("api/Offer/OffersByType/{offerTypeId}")]
        public IList<OfferListItemViewModel> GetOffersByType(int offerTypeId)
        {
            var list = new List<OfferListItemViewModel>();

            foreach (var offer in db.Offers.Where(o => o.Deleted == false && o.OfferType == offerTypeId).ToList())
            {
                list.Add(new OfferListItemViewModel().CopyFromModel(offer));

            }

            return list;
        }

        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")] 
        [Route("api/Offer/Availables")]
        public IList<OfferListItemViewModel> GetOffersAvailables()
        {
            var list = new List<OfferListItemViewModel>();

            var query = from o in db.Offers
                        join p in db.Projects on o.ID equals p.ID into jg
                        where o.Deleted == false
                        from subp in jg.DefaultIfEmpty()
                        where subp.ID == null
                        select o;

            foreach (var offer in query.ToList())
            {
                list.Add(new OfferListItemViewModel().CopyFromModel(offer));
            }

            return list;
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]
        [Route("api/Offer/AvailablesByOfferType/{offerTypeId}")]
        public IList<OfferListItemViewModel> GetOffersAvailablesByOfferType(int offerTypeId)
        {
            var list = new List<OfferListItemViewModel>();

            var query = from o in db.Offers
                        join p in db.Projects on o.ID equals p.ID into jg
                        where o.Deleted == false && o.OfferType == offerTypeId
                        from subp in jg.DefaultIfEmpty()
                        where subp.ID == null
                        select o;

            foreach (var offer in query.ToList())
            {
                list.Add(new OfferListItemViewModel().CopyFromModel(offer));
            }

            return list;
        }

        // GET api/Offer/1
        [Authorize(Roles = "Administrator,Project Manager,Editor,Super Admin")]  
        [ResponseType(typeof(OfferListItemViewModel))]
        public IHttpActionResult GetOffer(int id)
        {
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return NotFound();
            }

            var offerListItem = new OfferListItemViewModel().CopyFromModel(offer);

            if (offer.CreatedBy == null)
            {
                offerListItem.CreateByName = offer.CreateByExternal;
            }
            else {
                var user = db.Users.FirstOrDefault(u => u.ID == offer.ID);

                if (user != null) {
                    offerListItem.CreateByName = user.FirstName + " " + user.LastName;
                }
            }

            return Ok(offerListItem);
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrator,Super Admin")] 
        [Route("api/Offer/Create")]
        [ResponseType(typeof(Offer))]
        public IHttpActionResult PostOffer(OfferCreateEditViewModel offerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var offerModel = new Offer();
            offerViewModel.CopyToModelForCreate(offerModel);

            db.Offers.Add(offerModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OfferExists(offerViewModel.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok(offerModel);
        }

        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        [Route("api/Offer/Delete/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult DeleteOffer(int id)
        {
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return NotFound();
            }

            offer.Deleted = true;
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Administrator,Super Admin")]
        [Route("api/Offer/Edit/{id}")]
        [ResponseType(typeof(OfferCreateEditViewModel))]
        public IHttpActionResult GetOfferForEdit(int id)
        {
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return NotFound();
            }

            var offerModel = new OfferCreateEditViewModel();
            offerModel.CopyFromModel(offer);

            return Ok(offerModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Super Admin")]
        [Route("api/Offer/Save")]
        [ResponseType(typeof(Offer))]
        public IHttpActionResult SaveOffer(OfferCreateEditViewModel offerViewModel)
        {
            Offer offer = db.Offers.Find(offerViewModel.ID);
            if (offer == null)
            {
                return NotFound();
            }

            offer.Name = offerViewModel.Name;
            offer.ContentIDs = offerViewModel.ContentIDs;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Super Admin")]
        [Route("api/Offer/Copy")]
        [ResponseType(typeof(Offer))]
        public IHttpActionResult CopyOfferFromOther(CopyContentModel copyModel)
        {
            int sourceProjectID = -1;
            int targetProjectID = -1;
            Int32.TryParse(copyModel.targetProjectID, out targetProjectID);
            Int32.TryParse(copyModel.sourceProjectID, out sourceProjectID);

            Project projectT = db.Projects.FirstOrDefault(p => p.Offer.Deleted == false && p.ID == targetProjectID);
            Project projectS = db.Projects.FirstOrDefault(p => p.Offer.Deleted == false && p.ID == sourceProjectID);

            if (projectT == null || projectS == null)
            {
                return NotFound();
            }

            int contentIdx = 0;
            projectT = db.Projects.FirstOrDefault(p => p.Offer.Deleted == false && p.ID == targetProjectID);
            removeOldComponents(projectT);

            List<Component> newComponents = new List<Component>();

            foreach (string sourceContentID in copyModel.sourceContentIDs)
            {
                string targetContentID = copyModel.targetContentIDs[contentIdx++];

                foreach (Component sourceComponent in projectS.Components)
                {
                    if (sourceComponent.ContentID == sourceContentID)
                    {
                        Component tempComponent = new Component();
                        tempComponent.ProjectID = targetProjectID;
                        tempComponent.ContentID = targetContentID;
                        tempComponent.SectionID = sourceComponent.SectionID;
                        tempComponent.ComponentID = sourceComponent.ComponentID;
                        tempComponent.Data = sourceComponent.Data;
                        tempComponent.Inactive = sourceComponent.Inactive;

                        newComponents.Add(tempComponent);
                    }
                }
            }
            db.Components.AddRange(newComponents);

            try
            {
                db.SaveChanges();
            }
            catch (Exception e) {
                string t = e.Message;
            }
            
            return Ok(projectT);
        }

        private void removeOldComponents(Project projectT)
        {
            db.Components.RemoveRange(projectT.Components);
            db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OfferExists(int id)
        {
            return db.Offers.Count(e => e.ID == id) > 0;
        }
    }
}