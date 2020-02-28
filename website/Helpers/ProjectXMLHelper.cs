using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using st1001.website.Models;
using System.Text;
using st1001.data;
using System.Data.Entity;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Xml;

namespace st1001.website.Helpers
{
    public class ProjectXMLHelper
    {
        const string ERROR_PLACEHOLDER = "##ERROR_PLACEHOLDER##";
        const string CONTENT_IDS_PLACEHOLDER = "##CONTENT_IDS_PLACEHOLDER##";
        const string OFFER_NAME_PLACEHOLDER = "##OFFER_NAME_PLACEHOLDER##";
        const string OFFER_ID_PLACEHOLDER = "##OFFER_ID_PLACEHOLDER##";
        const string CONTENT_ID_PLACEHOLDER = "##CONTENT_ID_PLACEHOLDER##";
        const string ONLINE_ZONES_PLACEHOLDER = "##ONLINE_ZONES_PLACEHOLDER##";
        const string NEW_ACCOUNT_ZONES_PLACEHOLDER = "##NEW_ACCOUNT_ZONES_PLACEHOLDER##";
        const string PRIVATE_WEALTH_ZONES_PLACEHOLDER = "##PRIVATE_WEALTH_ZONES_PLACEHOLDER##";
        const string RTO_MOBILE_ZONES_PLACEHOLDER = "##RTO_MOBILE_ZONES_PLACEHOLDER##";
        const string SECTION_ID_PLACEHOLDER = "##SECTION_ID_PLACEHOLDER##";
        const string SECTION_NAME_PLACEHOLDER = "##SECTION_NAME_PLACEHOLDER##";
        const string COMPONENTS_PLACEHOLDER = "##COMPONENTS_PLACEHOLDER##";
        const string COMP_NAME_PLACEHOLDER = "##COMP_NAME_PLACEHOLDER##";
        const string COMP_TYPE_PLACEHOLDER = "##COMP_TYPE_PLACEHOLDER##";
        const string COMP_VALUE_PLACEHOLDER = "##COMP_VALUE_PLACEHOLDER##";
        const string ACTION_TYPE_PLACEHOLDER = "##ACTION_TYPE_PLACEHOLDER##";
        const string ACTION_DESC_PLACEHOLDER = "##ACTION_DESC_PLACEHOLDER##";
        const string ACTION_CALL_TO_PLACEHOLDER = "##ACTION_CALL_TO_PLACEHOLDER##";
        const string ACTION_TRIGGER_PLACEHOLDER = "##ACTION_TRIGGER_PLACEHOLDER##";
        const string COMP_URI_PLACEHOLDER = "##COMP_URI_PLACEHOLDER##";
        const string COMP_ALT_PLACEHOLDER = "##COMP_ALT_PLACEHOLDER##";
        const string LINKED_ACTION_PLACEHOLDER = "##LINKED_ACTION_PLACEHOLDER##";

        const string ACTIONS_PLACEHOLDER = "##ACTIONS_PLACEHOLDER##";
        const string EMPTY_ACTIONS_PLACEHOLDER = "<tns:Actions></tns:Actions>";

        const string ONLINE_BANKING_SECTION_TYPE = "VO";
        const string NEW_ACCOUNT_CENTER_SECTION_TYPE = "NEW";
        const string PRIVATE_WEALTH_MANAGEMENT_SECTION_TYPE = "PWM";
        const string RTO_MOBILE_SECTION_TYPE = "RTO";


        const string LEARN_MORE_XML_CTA_VALUE = "LearnMore";
        const string BALANCE_TRANSFER_VALUE = "gotoOLB-BalanceTransfer";
        const string OLB_BRIGHTFOLIO_VALUE = "goToOLB-Brightfolio";
        const string OLB_BRIGHTFOLIO_MOB_VALUE = "goToOLB-Brightfolio-MOB";

        /*Online Banking sections*/
        const int PRIMARY_BANNER_SECTION = 1;
        const int DETAIL_LEARN_MORE_SECTION = 2;
        const int GHOST_OFFR_RECOMMENDED_SECTION = 3;
        const int VIEW_ALL_OFFERS_SECTION = 4;
        const int SPLASH_SECTION = 5;
        const int SIGN_OFF_SECTION = 6;
        /*New Account Center sections*/
        const int CREDIT_CARD_SECTION = 7;
        const int DEPOSIT_SECTION = 8;
        const int EQUITY_SECTION = 9;
        const int SECTION_TYPE_CERTIFICATES_OF_DEPOSIT = 10;
        /* Bulletin Zone section */
        const int SECTION_TYPE_SERVICE_BULLETIN = 11;
        /*PWM sections*/
        const int PWM_HERO_OFFR = 12;
        const int PWM_EDU_OFFER = 13;
        const int PWM_SPECIAL_OFFER = 14;
        const int PWM_BULLETIN = 15;
        const int PMW_LEARN_MORE = 16;
        const int PRIMARY_OFFR = 17;
        const int VIEW_ALL = 18;
        const int PWM_BULLETIN_ZONE = 19;
        const int GHOST_OFFR = 20;
        const int LEARN_MORE = 21;
        /*RTO Mobile sections*/
        const int RTO_PRIMARY_OFFR = 22;
        const int RTO_LEARN_MORE = 23;
        const int RTO_ALL_OFFER = 25;

        const int MAIN_IMAGE_COMP_ID = 1;
        const int CALL_TO_ACTION_COMP_ID = 2;
        const int DETAILS_COMP_ID = 3;
        const int DISCLAIMER_COMP_ID = 4;
        const int REMAINDER_COMP_ID = 5;
        const int OFFER_REJECTION_COMP_ID = 6;
        const int HEADLINE_COMP_ID = 7;
        const int TERM_CONDITIONS_COMP_ID = 8;
        const int OFFER_DETAILS_COMP_ID = 9;
        const int CALL_TO_TWO_ACTION_COMP_ID = 10;
        const int SUB_HEADLINE_COMP_ID = 11;
        const int CLICK_TO_CHAT = 12;
        const int CC_MAIN_IMAGE_COMP_ID = 16;

        const string BULLETIN_ZONE_CODE = "BULLETIN_ZONE";

        string errorNode = "<?xml version=\"1.0\" ?> " +
            "<errors><error>" + ERROR_PLACEHOLDER + "</error></errors>";

        /*string rootNode ="<?xml version=\"1.0\" encoding=\"UTF-8\"?> " +
            "<tns:offerInfo contentID=\"" + CONTENT_ID_PLACEHOLDER + "\" id=\"" + OFFER_ID_PLACEHOLDER + "\" type=\"\" " +
                "xmlns:tns=\"http://www.suntrust.com/OfferContent\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                "xsi:schemaLocation=\"http://www.suntrust.com/OfferContent OfferContent.xsd\"> " +
                "<tns:title>" + OFFER_NAME_PLACEHOLDER + "</tns:title> " +
                "<tns:offerAttributes><tns:offerAttribute /></tns:offerAttributes>" +
                "<tns:channels>" +
                    "<tns:channel id=\"VO\" name=\"ONLINE BANKING\">" +
                        "<tns:zones>" +
                            ONLINE_ZONES_PLACEHOLDER +
                        "</tns:zones>" +
                    "</tns:channel>" +
                    "<tns:channel id=\"NEW\" name=\"NEW ACCOUNT CENTER\">" +
                        "<tns:zones>" +
                            NEW_ACCOUNT_ZONES_PLACEHOLDER +
                        "</tns:zones>" +
                    "</tns:channel>" +
                    "<tns:channel id=\"DC\" name=\"PRIVATE WEALTH MANAGEMENT\">" +
                        "<tns:zones>" +
                            PRIVATE_WEALTH_ZONES_PLACEHOLDER +
                        "</tns:zones>" +
                    "</tns:channel>" +
                    "<tns:channel id=\"MD\" name=\"RTO MOBILE\">" +
                        "<tns:zones>" +
                            RTO_MOBILE_ZONES_PLACEHOLDER +
                        "</tns:zones>" +
                    "</tns:channel>" +
                "</tns:channels>" +
            "</tns:offerInfo>";*/
        private string rootNode = "<?xml version=\"1.0\" encoding=\"UTF-8\"?> " +
                              "<tns:offerInfo contentID=\"" + CONTENT_ID_PLACEHOLDER + "\" id=\"" +
                              OFFER_ID_PLACEHOLDER + "\" type=\"\" " +
                              "xmlns:tns=\"http://www.suntrust.com/OfferContent\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                              "xsi:schemaLocation=\"http://www.suntrust.com/OfferContent OfferContent.xsd\"> " +
                              "<tns:title>" + OFFER_NAME_PLACEHOLDER + "</tns:title> " +
                              "<tns:offerAttributes><tns:offerAttribute /></tns:offerAttributes>" +
                              "<tns:channels>";

        string onlineBankingChannel = "<tns:channel id=\"VO\" name=\"ONLINE BANKING\">" +
                                                  "<tns:zones>" +
                                                      ONLINE_ZONES_PLACEHOLDER +
                                                  "</tns:zones>" +
                                              "</tns:channel>";

        string newAccountChannel = "<tns:channel id=\"NEW\" name=\"NEW ACCOUNT CENTER\">" +
                                                  "<tns:zones>" +
                                                      NEW_ACCOUNT_ZONES_PLACEHOLDER +
                                                  "</tns:zones>" +
                                              "</tns:channel>";

        string privateWealthChannel = "<tns:channel id=\"DC\" name=\"PRIVATE WEALTH MANAGEMENT\">" +
                                                  "<tns:zones>" +
                                                      PRIVATE_WEALTH_ZONES_PLACEHOLDER +
                                                  "</tns:zones>" +
                                              "</tns:channel>";

        string rtoMobileChannel = "<tns:channel id=\"MD\" name=\"RTO MOBILE\">" +
                                              "<tns:zones>" +
                                                   RTO_MOBILE_ZONES_PLACEHOLDER +
                                              "</tns:zones>" +
                                          "</tns:channel>";

        string closeRootNode = "</tns:channels>" +
                                       "</tns:offerInfo>";

        string sectionNode =
            "<tns:zone id=\"" + SECTION_ID_PLACEHOLDER + "\" name=\"" + SECTION_NAME_PLACEHOLDER + "\">" +
                "<tns:contents>" +
                    COMPONENTS_PLACEHOLDER +
                "</tns:contents>" +
                "<tns:Actions>" + ACTIONS_PLACEHOLDER + "</tns:Actions>" +
            "</tns:zone>";

        string componentNode =
            "<tns:content Name=\"" + COMP_NAME_PLACEHOLDER + "\" Type=\"" + COMP_TYPE_PLACEHOLDER + "\" Alt=\"" + COMP_ALT_PLACEHOLDER + "\" Uri=\"" + COMP_URI_PLACEHOLDER + "\">" +
                "<tns:Value>" + COMP_VALUE_PLACEHOLDER + "</tns:Value>" +
                LINKED_ACTION_PLACEHOLDER +
            "</tns:content>";

        string actionNode =
            "<tns:Action Type=\"" + ACTION_TYPE_PLACEHOLDER + "\" Description=\"" + ACTION_DESC_PLACEHOLDER +
                "\" CalltoAction=\"" + ACTION_CALL_TO_PLACEHOLDER + "\" Trigger=\"" + ACTION_TRIGGER_PLACEHOLDER + "\"/>";


        private st1001Entities db = new st1001Entities();
        private DbSet<SectionType> sectionsTypes;


        public ProjectXMLHelper()
        {
            this.sectionsTypes = db.SectionTypes;
        }

        public string GetAsXML(string projectID, string projectName, LayoutViewModel contentID)
        {
            try
            {
                bool obChannelAbailable = false;
                bool nacChannelAbailable = false;
                bool pwmChannelAbailable = false;
                bool rtoChannelAbailable = false;

                string xml = rootNode.Replace(OFFER_ID_PLACEHOLDER, projectID)
                        .Replace(OFFER_NAME_PLACEHOLDER, projectName.Replace("&", "&amp;"))
                        .Replace(CONTENT_ID_PLACEHOLDER, contentID.ID.Trim());

                foreach (var section in contentID.Sections)
                {
                    if (section.ID == SECTION_TYPE_SERVICE_BULLETIN)
                    {
                        if (!obChannelAbailable)
                        {
                            xml += onlineBankingChannel;
                            xml = xml.Replace(ONLINE_ZONES_PLACEHOLDER, GenerateSections(contentID.Sections, sectionsTypes.Where(s => s.Type == ONLINE_BANKING_SECTION_TYPE || s.ID == SECTION_TYPE_SERVICE_BULLETIN).ToList()));
                            obChannelAbailable = true;
                        }

                        if (!nacChannelAbailable)
                        {
                            xml += newAccountChannel;
                            xml = xml.Replace(NEW_ACCOUNT_ZONES_PLACEHOLDER,
                                    GenerateSections(contentID.Sections, sectionsTypes.Where(s => s.Type == NEW_ACCOUNT_CENTER_SECTION_TYPE).ToList()));
                            nacChannelAbailable = true;
                        }
                    }
                    else if ((section.ID >= PRIMARY_BANNER_SECTION && section.ID <= SIGN_OFF_SECTION) && section.Components.Count > 0)
                    {
                        if (!obChannelAbailable)
                        {
                            xml += onlineBankingChannel;
                            xml = xml.Replace(ONLINE_ZONES_PLACEHOLDER, GenerateSections(contentID.Sections, sectionsTypes.Where(s => s.Type == ONLINE_BANKING_SECTION_TYPE || s.ID == SECTION_TYPE_SERVICE_BULLETIN).ToList()));
                            obChannelAbailable = true;
                        }

                    }
                    else if ((section.ID >= CREDIT_CARD_SECTION && section.ID <= SECTION_TYPE_CERTIFICATES_OF_DEPOSIT) && section.Components.Count > 0)
                    {
                        if (!nacChannelAbailable)
                        {
                            xml += newAccountChannel;
                            xml = xml.Replace(NEW_ACCOUNT_ZONES_PLACEHOLDER,
                                    GenerateSections(contentID.Sections, sectionsTypes.Where(s => s.Type == NEW_ACCOUNT_CENTER_SECTION_TYPE).ToList()));
                            nacChannelAbailable = true;
                        }

                    }
                    else if ((section.ID >= PWM_HERO_OFFR && section.ID <= LEARN_MORE) && section.Components.Count > 0)
                    {
                        if (!pwmChannelAbailable)
                        {
                            xml += privateWealthChannel;
                            xml = xml.Replace(PRIVATE_WEALTH_ZONES_PLACEHOLDER,
                                     GenerateSections(contentID.Sections, sectionsTypes.Where(s => s.Type == PRIVATE_WEALTH_MANAGEMENT_SECTION_TYPE).ToList()));
                            pwmChannelAbailable = true;
                        }
                    }
                    else if ((section.ID == RTO_LEARN_MORE || section.ID == RTO_PRIMARY_OFFR || section.ID == RTO_ALL_OFFER) && section.Components.Count > 0)
                    {
                        if (!rtoChannelAbailable)
                        {
                            xml += rtoMobileChannel;
                            xml = xml.Replace(RTO_MOBILE_ZONES_PLACEHOLDER,
                                    GenerateSections(contentID.Sections, sectionsTypes.Where(s => s.Type == RTO_MOBILE_SECTION_TYPE).ToList()));
                            rtoChannelAbailable = true;
                        }
                    }
                }

                return xml + closeRootNode;
            }
            catch (Exception e)
            {
                return errorNode.Replace(ERROR_PLACEHOLDER, e.Message + ". " + e.InnerException);
            }
        }

        private string GenerateSections(IList<SectionViewModel> sections, IList<SectionType> subSections)
        {
            StringBuilder sectionsSB = new StringBuilder();

            foreach (var section in sections)
            {
                var sectionType = subSections.Where(s => s.ID == section.ID).FirstOrDefault();

                if (sectionType != null)
                {
                    string sectionXml = sectionNode.Replace(SECTION_ID_PLACEHOLDER, sectionType.Code)
                            .Replace(SECTION_NAME_PLACEHOLDER, sectionType.Name);

                    string actionsXml = GenerateActions(section);

                    sectionXml = sectionXml.Replace(ACTIONS_PLACEHOLDER, actionsXml);

                    if (actionsXml == string.Empty)
                    {
                        sectionXml = sectionXml.Replace(EMPTY_ACTIONS_PLACEHOLDER, string.Empty);
                    }

                    string compsXml = GenerateComponents(section);

                    //Required by the Client
                    if (section.ID == EQUITY_SECTION)
                    {
                        compsXml = "<tns:content Name=\"OfferImage\" Type=\"image\" Uri=\"\"></tns:content>" + compsXml; 
                    }

                    sectionXml = sectionXml.Replace(COMPONENTS_PLACEHOLDER, compsXml);

                    sectionsSB.Append(sectionXml);
                   
                }
            }

            return sectionsSB.ToString();
        }

        private string GenerateComponents(SectionViewModel section)
        {
            StringBuilder compSB = new StringBuilder();

            foreach (var c in section.Components)
            {
                string compXml = string.Empty;

                //TODO: Get the component names from DB. The info should be filled.

                if (!c.Inactive)
                {
                    if (c.TypeID == MAIN_IMAGE_COMP_ID)
                    {
                        compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "OfferImage")
                                .Replace(COMP_TYPE_PLACEHOLDER, "image")
                                .Replace(COMP_URI_PLACEHOLDER, Path.GetFileName(Convert.ToString(c.Data["src"])))
                                .Replace(COMP_ALT_PLACEHOLDER, Convert.ToString(c.Data["AltText"]))
                                //.Replace(COMP_VALUE_PLACEHOLDER, getHtmlData(Convert.ToString(c.Data["OverlayText"])));
                                .Replace(COMP_VALUE_PLACEHOLDER, string.Empty);

                        if (section.ID == PRIMARY_BANNER_SECTION || section.ID == PRIMARY_OFFR || section.ID == RTO_PRIMARY_OFFR ||
                            section.ID == SIGN_OFF_SECTION || section.ID == CREDIT_CARD_SECTION
                            || section.ID == PWM_HERO_OFFR || section.ID == RTO_ALL_OFFER || section.ID == EQUITY_SECTION || section.ID == SECTION_TYPE_CERTIFICATES_OF_DEPOSIT)
                        {
                            compXml = compXml.Replace(LINKED_ACTION_PLACEHOLDER, tryToFindAnEmbededNACAction(section.Components, section));
                        }

                       
                    }
                   
                    //Agregate new component Main Image
                    /* else if (c.TypeID == CC_MAIN_IMAGE_COMP_ID)
                    {
                        compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "OfferImage")
                                .Replace(COMP_TYPE_PLACEHOLDER, "image")
                                .Replace(COMP_URI_PLACEHOLDER, Path.GetFileName(Convert.ToString(c.Data["src"])))
                                .Replace(COMP_ALT_PLACEHOLDER, Convert.ToString(c.Data["AltText"]))
                                
                                .Replace(COMP_VALUE_PLACEHOLDER, string.Empty);

                        if (section.ID == EQUITY_SECTION)
                        {
                            compXml = compXml.Replace(LINKED_ACTION_PLACEHOLDER, tryToFindAnEmbededNACAction(section.Components, section));
                        }

                      
                    }*/
                    else if (c.TypeID == DETAILS_COMP_ID)
                    {
                        compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "OfferDetails")
                                .Replace(COMP_TYPE_PLACEHOLDER, "html")
                                .Replace("Uri=\"" + COMP_URI_PLACEHOLDER + "\"", string.Empty)
                                .Replace("Alt=\"" + COMP_ALT_PLACEHOLDER + "\"", string.Empty)
                                .Replace(COMP_VALUE_PLACEHOLDER, getHtmlData(Convert.ToString(c.Data["Description"])));
                    }

                    else if (c.TypeID == OFFER_DETAILS_COMP_ID)
                    {
                        compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "OfferDetails")
                                .Replace(COMP_TYPE_PLACEHOLDER, "html")
                                .Replace("Uri=\"" + COMP_URI_PLACEHOLDER + "\"", string.Empty)
                                .Replace("Alt=\"" + COMP_ALT_PLACEHOLDER + "\"", string.Empty)
                                .Replace(COMP_VALUE_PLACEHOLDER, getHtmlData(Convert.ToString(c.Data["Description"])));
                    }
                    else if (c.TypeID == DISCLAIMER_COMP_ID)
                    {
                        compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "OfferTermsCondition")
                                .Replace(COMP_TYPE_PLACEHOLDER, "html")
                                .Replace("Uri=\"" + COMP_URI_PLACEHOLDER + "\"", string.Empty)
                                .Replace("Alt=\"" + COMP_ALT_PLACEHOLDER + "\"", string.Empty)
                                .Replace(COMP_VALUE_PLACEHOLDER, getHtmlData(Convert.ToString(c.Data["Description"])));
                    }
                    else if (c.TypeID == HEADLINE_COMP_ID)
                    {
                        if (section.ID == GHOST_OFFR_RECOMMENDED_SECTION || section.ID == GHOST_OFFR)
                        {
                            compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "Title")
                                .Replace(COMP_TYPE_PLACEHOLDER, "text")
                                .Replace("Uri=\"" + COMP_URI_PLACEHOLDER + "\"", string.Empty)
                                .Replace("Alt=\"" + COMP_ALT_PLACEHOLDER + "\"", string.Empty)
                                .Replace(COMP_VALUE_PLACEHOLDER, Convert.ToString(c.Data["selectedOption"] == null ? string.Empty : c.Data["selectedOption"]["Name"]));

                            compXml = compXml.Replace(LINKED_ACTION_PLACEHOLDER, tryToFindAnEmbededNACAction(section.Components, section));
                        }

                        else if (section.ID == SECTION_TYPE_SERVICE_BULLETIN || section.ID == PWM_BULLETIN || section.ID == PWM_BULLETIN_ZONE)
                        {
                            var textContent = Convert.ToString(c.Data["Headline"]).Replace("&", " &amp;");

                            compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "HEADER")
                                .Replace(COMP_TYPE_PLACEHOLDER, "text")
                                .Replace("Uri=\"" + COMP_URI_PLACEHOLDER + "\"", string.Empty)
                                .Replace("Alt=\"" + COMP_ALT_PLACEHOLDER + "\"", string.Empty)
                                .Replace(COMP_VALUE_PLACEHOLDER, textContent);
                        }
                        else if (section.ID == CREDIT_CARD_SECTION || section.ID == DEPOSIT_SECTION || section.ID == EQUITY_SECTION 
                                || section.ID == SECTION_TYPE_CERTIFICATES_OF_DEPOSIT || section.ID == PWM_HERO_OFFR || section.ID == PWM_EDU_OFFER) {
                            compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "Title")
                                .Replace(COMP_TYPE_PLACEHOLDER, "text")
                                .Replace("Uri=\"" + COMP_URI_PLACEHOLDER + "\"", string.Empty)
                                .Replace("Alt=\"" + COMP_ALT_PLACEHOLDER + "\"", string.Empty)
                                .Replace(COMP_VALUE_PLACEHOLDER, Convert.ToString(c.Data["Headline"]));

                            //compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "Title")
                            //    .Replace(COMP_TYPE_PLACEHOLDER, "html")
                            //    .Replace("Uri=\"" + COMP_URI_PLACEHOLDER + "\"", string.Empty)
                            //    .Replace(COMP_VALUE_PLACEHOLDER, getHtmlData(Convert.ToString(c.Data["Headline"])));
                        }
                    }
                    else if (c.TypeID == TERM_CONDITIONS_COMP_ID)
                    {
                       /* if (section.ID == EQUITY_SECTION)
                        {
                            compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "OfferTermsCondition")
                                .Replace(COMP_TYPE_PLACEHOLDER, "html")
                                .Replace("Uri=\"" + COMP_URI_PLACEHOLDER + "\"", string.Empty)
                                .Replace("Alt=\"" + COMP_ALT_PLACEHOLDER + "\"", string.Empty)
                                .Replace(COMP_VALUE_PLACEHOLDER, getHtmlData(Convert.ToString(c.Data["Conditions"])));
                        }
                        else
                        {*/
                            compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "Disclosure")
                                .Replace(COMP_TYPE_PLACEHOLDER, "link")
                                .Replace(COMP_URI_PLACEHOLDER, EncodeURL(Convert.ToString(c.Data["url"])))
                                .Replace("Alt=\"" + COMP_ALT_PLACEHOLDER + "\"", string.Empty)
                                .Replace(COMP_VALUE_PLACEHOLDER, string.Empty);
                       // }
                    }
                    //else if (c.TypeID == SUB_HEADLINE_COMP_ID)
                    //{
                    //    compXml = componentNode.Replace(COMP_NAME_PLACEHOLDER, "SubHeadline")
                    //            .Replace(COMP_TYPE_PLACEHOLDER, "html")
                    //            .Replace("Uri=\"" + COMP_URI_PLACEHOLDER + "\"", string.Empty)
                    //            .Replace("Alt=\"" + COMP_ALT_PLACEHOLDER + "\"", string.Empty)
                    //            .Replace(COMP_VALUE_PLACEHOLDER, getHtmlData(Convert.ToString(c.Data["Description"])));
                    //}


                    if (compXml != string.Empty)
                    {
                        //Clean any pending LinkedAction placeholder
                        compXml = compXml.Replace(LINKED_ACTION_PLACEHOLDER, string.Empty);

                        compSB.Append(compXml);
                    }
                }
            }

            return compSB.ToString();
        }

        private string EncodeURL(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                return url.Replace("&", "&amp;");
            }

            return string.Empty;
        }

        private string GenerateActions(SectionViewModel section)
        {
            StringBuilder xmlSB = new StringBuilder();

            var components = section.ID == DETAIL_LEARN_MORE_SECTION ? section.Components.OrderByDescending(c => c.TypeID).ToList() : section.Components;

            foreach (var c in components)
            {
                try
                {
                    string xml = string.Empty;

                    if (!c.Inactive)
                    {
                        if (c.TypeID == OFFER_REJECTION_COMP_ID || c.TypeID == REMAINDER_COMP_ID)
                        {
                            string xmlCallToActionCode = translateCTAName(Convert.ToString(c.Data["selectedOption"]["Value"]));

                            if (!string.IsNullOrWhiteSpace(xmlCallToActionCode))
                            {
                                xml = actionNode
                                    .Replace(ACTION_TYPE_PLACEHOLDER, translateCTAType(Convert.ToString(c.Data["selectedOption"]["Value"])))
                                    .Replace(ACTION_CALL_TO_PLACEHOLDER, xmlCallToActionCode)
                                    .Replace(ACTION_DESC_PLACEHOLDER, Convert.ToString(c.Data["selectedOption"]["Name"]))
                                    .Replace(ACTION_TRIGGER_PLACEHOLDER, string.Empty);
                            }
                        }
                        else if (c.TypeID == CALL_TO_ACTION_COMP_ID || c.TypeID == CALL_TO_TWO_ACTION_COMP_ID || c.TypeID == CLICK_TO_CHAT)
                        {
                            string xmlCallToActionCode = translateCTAName(Convert.ToString(c.Data["selectedOption"]["Value"]));
                            string componentValue = Convert.ToString(c.Data["selectedOption"]["Value"]);
                            
                            if (!string.IsNullOrWhiteSpace(xmlCallToActionCode))
                            {
                                if ((section.ID == DETAIL_LEARN_MORE_SECTION && c.TypeID != CLICK_TO_CHAT) ||
                                    section.ID == VIEW_ALL_OFFERS_SECTION || section.ID == VIEW_ALL ||
                                    section.ID == SPLASH_SECTION || section.ID == RTO_LEARN_MORE ||
                                    ((section.ID == LEARN_MORE || section.ID == PMW_LEARN_MORE ||
                                    section.ID == PWM_HERO_OFFR || section.ID == PWM_EDU_OFFER || section.ID == PWM_SPECIAL_OFFER ) && componentValue != "CustomURL") ||
                                    (section.ID == CREDIT_CARD_SECTION && xmlCallToActionCode == LEARN_MORE_XML_CTA_VALUE) || section.ID == SECTION_TYPE_CERTIFICATES_OF_DEPOSIT)
                                {
                                    if (componentValue == BALANCE_TRANSFER_VALUE || componentValue == OLB_BRIGHTFOLIO_VALUE || componentValue == OLB_BRIGHTFOLIO_MOB_VALUE)
                                    {
                                        xml = actionNode
                                        .Replace(ACTION_TYPE_PLACEHOLDER, translateCTAType(componentValue))
                                        .Replace(ACTION_CALL_TO_PLACEHOLDER, xmlCallToActionCode)
                                        .Replace(ACTION_DESC_PLACEHOLDER, Convert.ToString(c.Data["buttonText"]))
                                        .Replace(ACTION_TRIGGER_PLACEHOLDER, EncodeURL(Convert.ToString(c.Data["selectedOption"]["Url"])));

                                    }
                                    else
                                    {
                                        xml = actionNode
                                       .Replace(ACTION_TYPE_PLACEHOLDER, translateCTAType(componentValue))
                                       .Replace(ACTION_CALL_TO_PLACEHOLDER, xmlCallToActionCode)
                                       .Replace(ACTION_DESC_PLACEHOLDER, Convert.ToString(c.Data["buttonText"]))
                                       .Replace(ACTION_TRIGGER_PLACEHOLDER, EncodeURL(Convert.ToString(c.Data["url"])));
                                    }
                                }
                                else if (((section.ID == LEARN_MORE || section.ID == PMW_LEARN_MORE ||
                                  section.ID == PWM_HERO_OFFR || section.ID == PWM_EDU_OFFER || section.ID == PWM_SPECIAL_OFFER ) && componentValue == "CustomURL"))
                                {
                                    xml = actionNode
                                   .Replace(ACTION_TYPE_PLACEHOLDER, translateCTAType("CustomURL-Click"))
                                   .Replace(ACTION_CALL_TO_PLACEHOLDER, xmlCallToActionCode)
                                   .Replace(ACTION_DESC_PLACEHOLDER, Convert.ToString(c.Data["buttonText"]))
                                   .Replace(ACTION_TRIGGER_PLACEHOLDER, EncodeURL(Convert.ToString(c.Data["url"])));
                                }
                                else if ((section.ID == DETAIL_LEARN_MORE_SECTION && c.TypeID == CLICK_TO_CHAT))
                                {
                                    xml = actionNode
                                    .Replace(ACTION_TYPE_PLACEHOLDER, translateCTAType(componentValue))
                                    .Replace(ACTION_CALL_TO_PLACEHOLDER, xmlCallToActionCode)
                                    .Replace(ACTION_DESC_PLACEHOLDER, string.Empty)
                                    .Replace(ACTION_TRIGGER_PLACEHOLDER, EncodeURL(Convert.ToString(c.Data["url"])));
                                }
                                /*else if (section.ID == RTO_LEARN_MORE && componentValue != "CustomURL")
                                {
                                    xml = actionNode.Replace(LINKED_ACTION_PLACEHOLDER, tryToFindAnEmbededNACAction(section.Components, section));
                                }*/
                                
                            }
                        }

                        if (xml != string.Empty)
                        {
                            xmlSB.Append(xml);
                        }
                    }
                }
                catch (Exception ex) {
                    LogHelper.Log(string.Format("GenerateActions - SectionID: {0}, ComponentID: {1}, SelectedOption: {2}, ExceptionMsg: {3}", 
                            section.ID, c.TypeID, Convert.ToString(c.Data["selectedOption"]), ex.Message));
                }
            }

            return xmlSB.ToString();
        }

        private string tryToFindAnEmbededNACAction(IList<ComponentViewModel> components, SectionViewModel section)
        {
            foreach (var c in components)
            {
                if (c.TypeID == CALL_TO_ACTION_COMP_ID || c.TypeID == CALL_TO_TWO_ACTION_COMP_ID)
                {
                    if (!c.Inactive)
                    {
                        try {
                            string componentValue = Convert.ToString(c.Data["selectedOption"]["Value"]);
                            string xmlCallToActionCode = translateCTAName(componentValue);

                            if (!string.IsNullOrWhiteSpace(xmlCallToActionCode) && (section.ID != CREDIT_CARD_SECTION || xmlCallToActionCode != LEARN_MORE_XML_CTA_VALUE))
                            {
                               
                                if (componentValue == BALANCE_TRANSFER_VALUE)
                                {
                                    return actionNode
                                       .Replace(ACTION_TYPE_PLACEHOLDER, "APPLY") //Override CLICK by APPLY
                                       .Replace(ACTION_CALL_TO_PLACEHOLDER, xmlCallToActionCode)
                                       .Replace(ACTION_DESC_PLACEHOLDER, Convert.ToString(c.Data["buttonText"]))
                                       .Replace(ACTION_TRIGGER_PLACEHOLDER, EncodeURL(Convert.ToString(c.Data["selectedOption"]["Url"])));
                                }
                                else if (componentValue == OLB_BRIGHTFOLIO_VALUE || componentValue == OLB_BRIGHTFOLIO_MOB_VALUE)
                                {
                                    return actionNode
                                       .Replace(ACTION_TYPE_PLACEHOLDER, "CLICK") //Override APPLY by CLICK
                                       .Replace(ACTION_CALL_TO_PLACEHOLDER, xmlCallToActionCode)
                                       .Replace(ACTION_DESC_PLACEHOLDER, Convert.ToString(c.Data["buttonText"]))
                                       .Replace(ACTION_TRIGGER_PLACEHOLDER, EncodeURL(Convert.ToString(c.Data["selectedOption"]["Url"])));
                                }
                                else
                                {
                                    return actionNode
                                       .Replace(ACTION_TYPE_PLACEHOLDER, "APPLY") //Override CLICK by APPLY
                                       .Replace(ACTION_CALL_TO_PLACEHOLDER, xmlCallToActionCode)
                                       .Replace(ACTION_DESC_PLACEHOLDER, Convert.ToString(c.Data["buttonText"]))
                                       .Replace(ACTION_TRIGGER_PLACEHOLDER, EncodeURL(Convert.ToString(c.Data["url"])));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LogHelper.Log(string.Format("tryToFindAnEmbededNACAction - SectionID: {0}, ComponentID: {1}, SelectedOption: {2}, ExceptionMsg: {3}",
                                    section.ID, c.TypeID, Convert.ToString(c.Data["selectedOption"]), ex.Message));
                        }
                    }

                    return string.Empty;
                }
            }

            return string.Empty;
        }

        private string translateCTAName(string componentValue)
        {
            if (componentValue == "NAC")
            {
                return "goToNAC";
            }
            else if (componentValue == "CustomURL" || componentValue == "goToOLB-Brightfolio-MOB")
            {
                return "goToThirdParty";
            }
            else if (componentValue == "gotoOLB-Statement")
            {
                return "goToOLB-Statement";
            }
            else if (componentValue == "gotoOLB-BillPay")
            {
                return "goToOLB-BillPay";
            }
            else if (componentValue == "RemindMeLater")
            {
                return "REMINDMELATER";
            }
            else if (componentValue == "NoThanks")
            {
                return "DECLINE";
            }
            else if (componentValue == "LearnMore")
            {
                return "LEARNMORE";
            }
            else if (componentValue == "NULL")
            {
                return string.Empty;
            }
            else if (componentValue == "goToOLBEmailAddress")
            {
                return "goToOLBEmailAddress";
            }
            else if (componentValue == "goToOLBMobileNumber")
            {
                return "goToOLBMobileNumber";
            }
            else if (componentValue == "Go-to-Chat")
            {
                return "goToChat";
            }
            else if (componentValue == "gotoMOB-Alerts")
            {
                return "goToMOB-Alerts";
            }
            else if (componentValue == "gotoMOB-BillPay")
            {
                return "goToMOB-BillPay";
            }
            else if (componentValue == "gotoMOB-Statement")
            {
                return "goToMOB-Statement";
            }
            else if (componentValue == "gotoOLB-BalanceTransfer")
            {
                return "goToOLB-BalanceTransfer";
            }
            else if (componentValue == "goToWS-StatementDelivery")
            {
                return "goToWs-StatementDelivery";
            }
            else if (componentValue == "goToOLB-ODP")
            {
                return "goToOLB-ODP";
            }
            else if (componentValue == "goToOLB-Brightfolio")
            {
                return "goToOLB-Brightfolio";
            }
            else if (componentValue == "goToOLB-InternalTransfer" || componentValue == "goToOLB-InternalTransfers")
            {
                return "goToOLB-InternalTransfer";
            }
            else if (componentValue == "goToOLB-ExternalTransfer" || componentValue == "goToOLB-ExternalTransfers")
            {
                return "goToOLB-ExternalTransfer";
            }
            else if (componentValue == "goToOLB-OverdraftCoverage" || componentValue == "goToOLB-ODC")
            {
                return "goToOLB-ODC";
            }

            return componentValue;
        }

        private string translateCTAType(string componentValue)
        {
            if (componentValue == "NAC" || componentValue == "gotoOLB-Statement" || componentValue == "gotoOLB-BillPay" ||
                componentValue == "goToOLBEmailAddress" || componentValue == "goToOLBMobileNumber" || componentValue == "CustomURL" || componentValue == "gotoMOB-Alerts" ||
               componentValue == "gotoMOB-BillPay" || componentValue == "gotoMOB-Statement" || componentValue == "gotoOLB-BalanceTransfer" || componentValue == "goToOLB-ODP")
            {
                return "APPLY";
            }
            else if (componentValue == "LearnMore")
            {
                return "LEARNMORE";
            }
            else if (componentValue == "RemindMeLater")
            {
                return "REMINDMELATER";
            }
            else if (componentValue == "NoThanks")
            {
                return "DECLINE";
            }
            else if (componentValue == "NULL")
            {
                return string.Empty;
            }
            else if (componentValue == "CustomURL-Click" || componentValue == "goToWS-StatementDelivery" || componentValue == "goToOLB-Brightfolio")
            {
                return "CLICK";
            }
            else if (componentValue == "goToChat")
            {
                return "HIDDEN:NODISP";
            }

            return "APPLY";
        }

        private string getHtmlData(string htmlText)
        {
            return "<![CDATA[" + htmlText + "]]>";
        }
    }
}
 
 