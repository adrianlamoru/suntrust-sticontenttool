using st1001.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace st1001.website.WS
{
    /// <summary>
    /// Summary description for OCTWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OCTWebService : System.Web.Services.WebService
    {
        private st1001Entities db = new st1001Entities();

        [WebMethod]
        public ActionResult PushOffer(Offer Offer)
        {
            ActionResult result;
            result.Status = true;
            result.Message = "Success";

            if (Offer != null)
            {
                data.Offer myOffer = null;
                try
                {
                    myOffer = Offer.GetAsOffer();
                    db.Offers.Add(myOffer);
                    db.SaveChanges();
                    result.Status = true;
                    result.Message = "Success";
                }
                catch (Exception ex)
                {
                    if (ex.Message == Offer.INVALID_APRIMO_OFFER_ID_MSG)
                    {
                        result.Status = false;
                        result.Message = Offer.INVALID_APRIMO_OFFER_ID_MSG;
                    }
                    else if (ex.Message == Offer.DUPLICATED_CONTENT_ID_MSG)
                    {
                        result.Status = false;
                        result.Message = Offer.DUPLICATED_CONTENT_ID_MSG;
                    }
                    else if (ex.Message == Offer.INVALID_CONTENT_IDS_MSG)
                    {
                        result.Status = false;
                        result.Message = Offer.INVALID_CONTENT_IDS_MSG;
                    }
                    else if (ex.Message == Offer.INVALID_OFFER_NAME_MSG)
                    {
                        result.Status = false;
                        result.Message = Offer.INVALID_OFFER_NAME_MSG;
                    }
                    else if (ex.Message == Offer.INVALID_OFFER_DESCRIPTION_MSG)
                    {
                        result.Status = false;
                        result.Message = Offer.INVALID_OFFER_DESCRIPTION_MSG;
                    }
                    else if (db.Offers.Count(e => e.ID == myOffer.ID) > 0)
                    {
                        result.Status = false;
                        result.Message = "The specified offer already exists";
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = string.Format("{0}", ex.Message);
                    }
                }
            }
            return result;
        }
    }
}
