using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace st1001.website.WS
{
    public class Offer
    {
        public const string INVALID_APRIMO_OFFER_ID_MSG = "The specified Aprimo Offer ID value is not valid";
        public const string INVALID_CONTENT_IDS_MSG = "There are not valid content ID or the content ID list is empty";
        public const string DUPLICATED_CONTENT_ID_MSG = "The content ID list has duplicates";
        public const string INVALID_OFFER_NAME_MSG = "The offer name cannot be null or empty";
        public const string INVALID_OFFER_DESCRIPTION_MSG = "The offer description connot be null or empty";

        public string AprimoOfferID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public OfferContent[] ContentItems { get; set; }
        public Data[] OtherData { get; set; }

        public data.Offer GetAsOffer()
        {
            if (string.IsNullOrWhiteSpace(this.AprimoOfferID))
            {
                throw new Exception(INVALID_APRIMO_OFFER_ID_MSG);
            }

            int id = 0;
            Int32.TryParse(this.AprimoOfferID, out id);

            if (id <= 0)
            {
                throw new Exception(INVALID_APRIMO_OFFER_ID_MSG);
            }

            string contentIDs = ContentItemsAsString();

            if (string.IsNullOrWhiteSpace(contentIDs))
            {
                throw new Exception(INVALID_CONTENT_IDS_MSG);
            }

            data.Offer offer = new data.Offer();

            offer.ID = id;

            if (string.IsNullOrEmpty(this.Name))
            {
                throw new Exception(INVALID_OFFER_NAME_MSG);
            }

            if (string.IsNullOrEmpty(this.Description))
            {
                throw new Exception(INVALID_OFFER_DESCRIPTION_MSG);
            }

            offer.Name = this.Name != null ? this.Name.Trim() : string.Empty;
            offer.Description = this.Description != null ? this.Description.Trim() : string.Empty;
            offer.CreateByExternal = "Aprimo";
            offer.UpdatedDate = DateTime.UtcNow;
            offer.ContentIDs = contentIDs != null ? contentIDs.Trim() : string.Empty;
            offer.HasProject = false;

            return offer;
        }

        public string ContentItemsAsString() {
            if (ContentItems == null)
                return string.Empty;

            StringBuilder st = new StringBuilder();

            foreach (var content in ContentItems)
            {
                if (!string.IsNullOrWhiteSpace(content.ID))
                {
                    if (ContentItems.Where(c => c.ID == content.ID).Count() > 1)
                    {
                        throw new Exception(DUPLICATED_CONTENT_ID_MSG);
                    }

                    st.Append( content.ID != null ? content.ID.Trim() : string.Empty);
                    st.Append(",");
                }
            }
            return st.ToString();
        }
    }
}