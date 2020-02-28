using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class OCTWebServiceTest
    {
        [TestMethod]
        public void CreateOfferEntry()
        {
            bool success = true;
            try
            {
                OCTWebService.OCTWebServiceSoap12Client ws = new OCTWebService.OCTWebServiceSoap12Client();
                Console.WriteLine("Calling Web methods");
                OCTWebService.Offer offer = new OCTWebService.Offer();
                offer.AprimoOfferID = "1122";
                offer.Name = "Consumer Lending: Home Equity ITA";
                offer.Description = "Description here";
                offer.Status = "Status here";

                
                /*
                offer.ContentItems = new OCTWebService.OfferContent[] {
                    new OCTWebService.OfferContent { ID = "EQACQ1014005"},
                    new OCTWebService.OfferContent { ID = "EQACQ1014006"}
                };
                */

                OCTWebService.ActionResult result = ws.PushOffer(offer);
                /*
                offer.AprimoOfferID = null;
                result = ws.PushOffer(offer);

                offer.AprimoOfferID = "-1";
                result = ws.PushOffer(offer);

                offer.AprimoOfferID = "0";
                result = ws.PushOffer(offer);

                offer.AprimoOfferID = "1122";
                result = ws.PushOffer(offer);

                offer.ContentItems = new OCTWebService.OfferContent[] {
                    new OCTWebService.OfferContent { ID = "EQACQ1014005"},
                    new OCTWebService.OfferContent { ID = "EQACQ1014005"}
                };
                result = ws.PushOffer(offer);

                offer.ContentItems = new OCTWebService.OfferContent[] {
                    new OCTWebService.OfferContent { ID = "EQACQ1014005"},
                    new OCTWebService.OfferContent { ID = "EQACQ1014006"}
                };
                result = ws.PushOffer(offer);

                offer.ContentItems = new OCTWebService.OfferContent[] {};
                result = ws.PushOffer(offer);
                */
                Console.WriteLine("Finishing...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
            }

            Assert.AreEqual(true, success);
        }

  
    }
}
