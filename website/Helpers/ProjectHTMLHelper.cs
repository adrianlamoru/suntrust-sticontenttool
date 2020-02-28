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
using System.Drawing.Imaging;
using System.Web.Mvc;

namespace st1001.website.Helpers
{
    public class ProjectHTMLHelper
    {

        const string ONLINE_BANKING_SECTION_TYPE = "VO";
        const string NEW_ACCOUNT_CENTER_SECTION_TYPE = "NEW";
        const string PRIVATE_WEALTH_MANAGEMENT_SECTION_TYPE = "PWM";
        const string RTO_MOBILE_SECTION_TYPE = "RTO";
        const string LEARN_MORE_XML_CTA_VALUE = "LearnMore";

        private static HttpRequest request = HttpContext.Current.Request;
        private static string appPath = request.ApplicationPath;
        private string baseUrl = request.Url.Scheme + "://" + request.Url.Authority + appPath;
        private bool rightRailsCTA = false;

        const string SUNTRUST_LOGO_URL = "##SUNTRUST_LOGO_URL##";
        const string FACEBOOK_LOGO_URL = "##FACEBOOK_LOGO_URL##";
        const string TWITTER_LOGO_URL = "##TWITTER_LOGO_URL##";
        const string YOUTUBE_LOGO_URL = "##YOUTUBE_LOGO_URL##";
        const string CTA_CARET_URL = "##CTA_CARET_URL##";
        const string LINKEDIN_LOGO_URL = "##LINKEDIN_LOGO_URL##";
        const string MAIN_IMAGE_URL = "##MAIN_IMAGE_URL##";
        const string MAIN_IMAGE_URL_MOBILE = "##MAIN_IMAGE_URL_MOBILE##";
        const string MAIN_IMAGE_ALT = "##MAIN_IMAGE_ALT##";
        const string MAIN_IMAGE_LINK_URL = "##MAIN_IMAGE_LINK_URL##";
        const string MAIN_IMAGE_LINK_VML_URL = "##MAIN_IMAGE_LINK_VML_URL##";
        const string MAIN_CONTENT_AREA = "##MAIN_CONTENT_AREA##";
        const string RIGHT_RAIL_CONTENT_AREA = "##RIGHT_RAIL_CONTENT_AREA##";
        const string DISCLAIMER_CONTENT_AREA = "##DISCLAIMER_CONTENT_AREA##";
        const string MAIN_CONTENT_TABLE = "##MAIN_CONTENT_TABLE##";
        const string MAIN_CONTENT_TABLE_CLASS = "##MAIN_CONTENT_TABLE_CLASS##";
        const string MAIN_CONTENT_TABLE_STYLE = "##MAIN_CONTENT_TABLE_STYLE##";
        const string RIGHT_RAIL_ACTIVE_CONTENT = "##RIGHT_RAIL_ACTIVE_CONTENT##";
        const string RIGHT_RAIL_IMAGE_CONTENT_SRC = "##RIGHT_RAIL_IMAGE_CONTENT_SRC##";
        const string RIGHT_RAIL_IMAGE_CONTENT_ALT = "##RIGHT_RAIL_IMAGE_CONTENT_ALT##";
        const string RIGHT_RAIL_TEXT_CONTENT = "##RIGHT_RAIL_TEXT_CONTENT##";
        const string GO_ONLINE_CTA_HREF = "##GO_ONLINE_CTA_HREF##";
        const string CALL_CTA_HREF = "##CALL_CTA_HREF##";
        const string VISIT_CTA_HREF = "##VISIT_CTA_HREF##";
        const string GO_ONLINE_CTA_BUTTON_TEXT = "##GO_ONLINE_CTA_BUTTON_TEXT##";
        const string CALL_CTA_BUTTON_TEXT = "##CALL_CTA_BUTTON_TEXT##";
        const string VISIT_CTA_BUTTON_TEXT = "##VISIT_CTA_BUTTON_TEXT##";
        const string GO_ONLINE_CTA_BUTTON_LINK_TEXT = "##GO_ONLINE_CTA_BUTTON_LINK_TEXT##";
        const string CALL_CTA_BUTTON_LINK_TEXT = "##CALL_CTA_BUTTON_LINK_TEXT##";
        const string VISIT_CTA_BUTTON_LINK_TEXT = "##VISIT_CTA_BUTTON_LINK_TEXT##";
        const string BOTTOM_CTA_AREA_CONTENT = "##BOTTOM_CTA_AREA_CONTENT##";
        const string RIGHT_CTA_AREA_CONTENT = "##RIGHT_CTA_AREA_CONTENT##";
        const string BOTTOM_CTA_COUNT_CLASS = "##BOTTOM_CTA_COUNT_CLASS##";
        const string RIGHT_RAIL_IMAGE_TEXT_CONTENT = "##RIGHT_RAIL_IMAGE_TEXT_CONTENT##";
        const string GO_ONLINE_CTA_RIGHT_MARGIN = "##GO_ONLINE_CTA_RIGHT_MARGIN##";
        const string CALL_CTA_RIGHT_MARGIN = "##CALL_CTA_RIGHT_MARGIN##";
        const string VISIT_CTA_RIGHT_MARGIN = "##VISIT_CTA_RIGHT_MARGIN##";
        const string GO_ONLINE_CTA_ADDITIONAL_ROW = "##GO_ONLINE_CTA_ADDITIONAL_ROW##";
        const string CALL_CTA__ADDITIONAL_ROW = "##CALL_CTA__ADDITIONAL_ROW##";
        const string GO_ONLINE_CTA_RECT_WIDTH = "##GO_ONLINE_CTA_RECT_WIDTH##";
        const string CALL_CTA_RECT_WIDTH = "##CALL_CTA_RECT_WIDTH##";
        const string VISIT_CTA_RECT_WIDTH = "##VISIT_CTA_RECT_WIDTH##";
        const string RIGHT_CONTENT = "##RIGHT_CONTENT##";
        const string LEFT_CONTENT_COLUMN_WIDTH = "##LEFT_CONTENT_COLUMN_WIDTH##";

        const string GO_ONLINE_BORDER_STYLE = "##GO_ONLINE_BORDER_STYLE##";
        const string CALL_BORDER_STYLE = "##CALL_BORDER_STYLE##";
        const string VISIT_BORDER_STYLE = "##VISIT_BORDER_STYLE##";
        const string APPLY_NOW_CTA_URL = "##APPLY_NOW_CTA_URL##";
        const string APPLY_NOW_BACKGROUND_URL = "##APPLY_NOW_BACKGROUND_URL##";
        const string APPLY_NOW_TRIANGLE_URL = "##APPLY_NOW_TRIANGLE_URL##";
        const string APPLY_NOW_BUTTON_TEXT = "##APPLY_NOW_BUTTON_TEXT##";
        const string APPLY_NOW_BUTTON_WIDTH = "##APPLY_NOW_BUTTON_WIDTH##";

        const string FIRST_HEADER_LINK = "##FIRST_HEADER_LINK##";
        const string SECOND_HEADER_LINK = "##SECOND_HEADER_LINK##";
        const string THIRD_HEADER_LINK = "##THIRD_HEADER_LINK##";
        const string FIRST_HEADER_LINK_URL = "##FIRST_HEADER_LINK_URL##";
        const string SECOND_HEADER_LINK_URL = "##SECOND_HEADER_LINK_URL##";
        const string THIRD_HEADER_LINK_URL = "##THIRD_HEADER_LINK_URL##";
        const string FIRST_HEADER_LINK_TEXT = "##FIRST_HEADER_LINK_TEXT##";
        const string SECOND_HEADER_LINK_TEXT = "##SECOND_HEADER_LINK_TEXT##";
        const string THIRD_HEADER_LINK_TEXT = "##THIRD_HEADER_LINK_TEXT##";
        const string FIRST_HEADER_LINK_SEPARATOR = "##FIRST_HEADER_LINK_SEPARATOR##";
        const string SECOND_HEADER_LINK_SEPARATOR = "##SECOND_HEADER_LINK_SEPARATOR##";

        const string HEADER_OVERLAY_COPY = "##HEADER_OVERLAY_COPY##";
        const string HEADER_CONTENT = "##HEADER_CONTENT##";
        const string HEADER_CONTENT_MOBILE = "##HEADER_CONTENT_MOBILE##";
        const string HEADER_CONTENT_MOBILE_BUTTON = "##HEADER_CONTENT_MOBILE_BUTTON##";
        const string HEADER_CTA_COPY = "##HEADER_CTA_COPY##";
        const string HEADER_SOLID_COLOR_BK = "##HEADER_SOLID_COLOR_BK##";
        const string APPLY_NOW_BACKGROUD = "##APPLY_NOW_BACKGROUD##";

        const string LEFT_APPLY_NOW_TRIANGLE = "##LEFT_APPLY_NOW_TRIANGLE##";
        const string RIGHT_APPLY_NOW_TRIANGLE = "##RIGHT_APPLY_NOW_TRIANGLE##";
        const string LEARN_MORE_BUTTON_WIDTH = "##LEARN_MORE_BUTTON_WIDTH##";

        const string TEMPLATE_HEADER_IMG_URL = "##TEMPLATE_HEADER_IMG_URL##";
        const string TEMPLATE_HEADER_IMG_MOB_URL = "##TEMPLATE_HEADER_IMG_MOB_URL##";
        const string TEMPLATE_BUTTON_LEFT_IMG_URL = "##TEMPLATE_BUTTON_LEFT_IMG_URL##";
        const string TEMPLATE_BUTTON_ARROW_IMG_URL = "##TEMPLATE_BUTTON_ARROW_IMG_URL##";
        const string TEMPLATE_BUTTON_RIGHT_IMG_URL = "##TEMPLATE_BUTTON_RIGHT_IMG_URL##";
        const string TEMPLATE_BUTTON_BACKGROUND_COLOR = "##TEMPLATE_BUTTON_BACKGROUND_COLOR##";

        const string EMAIL_TEMPLATE_SECTION_TYPE = "ETMP";
        const string HEADER = "_header";
        const string ARROW = "Arrow";
        const string LEFT = "Left";
        const string RIGHT = "Right";

        private string mainContentTableRightContent = "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"display:inline-table;vertical-align:top\" class=\"html-view right-content\">" + "\n" +
                                                            RIGHT_RAIL_ACTIVE_CONTENT +
                                                        "</table>" + "\n";

        private static string rightRailImageTextContent = "<tr>" + "\n" +
                                                               "<td align=\"left\" valign=\"top\" class=\"right-content-wrapper image-component\">" + "\n" +
                                                                    RIGHT_RAIL_IMAGE_TEXT_CONTENT +
                                                                "</td>" + "\n" +
                                                            "</tr>" + "\n";

        private string rightRailImageContent = "<tr>" + "\n" +
                                                    "<td align=\"left\" valign=\"top\" class=\"right-content-wrapper\" style=\"text-align:center;  vertical-align: top;\">" + "\n" +
                                                        "<img width=\"177\" src=\"" + RIGHT_RAIL_IMAGE_CONTENT_SRC + "\" alt=\"" + RIGHT_RAIL_IMAGE_CONTENT_ALT + "\" class=\"mobimg\" style=\"display: block\" border=\"0\"/>" + "\n" +
                                                    "</td>" + "\n" +
                                               "</tr>" + "\n";

        private string rightRailTextContent = "<tr>" + "\n" +
                                                    "<td align=\"left\" valign=\"top\" class=\"right-content-wrapper\">" + "\n" +
                                                        RIGHT_RAIL_TEXT_CONTENT +
                                                    "</td>" + "\n" +
                                               "</tr>" + "\n";

        private static string bottomGoOnlineCTADesktop = "<th class=\"cta-links force-col\"" + GO_ONLINE_BORDER_STYLE + ">" + "\n" +
                                                              "<a href=\"" + GO_ONLINE_CTA_HREF + "\" target=\"_blank\" style=\"color:#ffffff;display:inline-block;font-size:16px;text-align:left;text-decoration:none;width:100%;height: auto;-webkit-text-size-adjust:none;\">" + "\n" +
                                                                  "<table style=\"width:100%;display:table-cell;border: solid 10px #DCD9DC;background-color: #DCD9DC;\">" + "\n" +
                                                                      "<tr>" + "\n" +
                                                                          "<td style=\"width: 20px\">" + "\n" +
                                                                              "<img src=\"" + CTA_CARET_URL + "\" width=\"10\" height=\"35\" style=\"vertical-align: middle\">" + "\n" +
                                                                          "</td>" + "\n" +
                                                                          "<td>" + "\n" +
                                                                            "<!--[if mso]>" + "\n" +
                                                                            "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + GO_ONLINE_CTA_HREF + "\" style=\"v-text-anchor:top;width:" + GO_ONLINE_CTA_RECT_WIDTH + "height: auto;mso-wrap-style:square;\" stroke=\"f\" target=\"_blank\" fillcolor=\"#DCD9DC\">" + "\n" +
                                                                                "<w:anchorlock/>" + "\n" +
                                                                                "<v:textbox style =\"mso-fit-shape-to-text: t\">" + "\n" +
                                                                            "<![endif]-->" + "\n" +

                                                                                  "<h3 style=\"display:block;font-size: 16px;font-weight:bold;color: #003B71; margin: 0\">" + GO_ONLINE_CTA_BUTTON_TEXT + "</h3>" + "\n" +
                                                                                  "<span style = \"display:block;font-size: 14px;color: #000000; text-decoration: underline\">" + GO_ONLINE_CTA_BUTTON_LINK_TEXT + "</span>" + "\n" +

                                                                            "<!--[if mso]>" + "\n" +
                                                                            "</v:rect>" + "\n" +
                                                                            "</v:textbox>" + "\n" +
                                                                            "<![endif]-->" + "\n" +

                                                                        "</td>" + "\n" +
                                                                      "</tr>" + "\n" +
                                                                  "</table>" + "\n" +
                                                              "</a>" + "\n" +
                                                          "</th>" + "\n";

        private static string bottomGoOnlineCTADesktop1 = "<td class=\"cta-links force-col desktop\"" + GO_ONLINE_BORDER_STYLE + ">" + "\n" +
                                                              "<a href=\"" + GO_ONLINE_CTA_HREF + "\" target=\"_blank\" style=\"color:#ffffff;display:inline-block;font-size:16px;text-align:left;text-decoration:none;width:100%;height: auto;-webkit-text-size-adjust:none;\">" + "\n" +
                                                                  "<table class=\"desktop\" style=\"width:100%;display:table-cell;border: solid 10px #DCD9DC;background-color: #DCD9DC;\">" + "\n" +
                                                                      "<tr>" + "\n" +
                                                                          "<td style=\"width: 20px\">" + "\n" +
                                                                              "<img src=\"" + CTA_CARET_URL + "\" width=\"10\" height=\"35\" style=\"vertical-align: middle\">" + "\n" +
                                                                          "</td>" + "\n" +
                                                                          "<td>" + "\n" +
                                                                            "<!--[if mso]>" + "\n" +
                                                                            "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + GO_ONLINE_CTA_HREF + "\" style=\"v-text-anchor:top;width:" + GO_ONLINE_CTA_RECT_WIDTH + "height: auto;mso-wrap-style:square;\" stroke=\"f\" target=\"_blank\" fillcolor=\"#DCD9DC\">" + "\n" +
                                                                                "<w:anchorlock/>" + "\n" +
                                                                                "<v:textbox style =\"mso-fit-shape-to-text: t\">" + "\n" +
                                                                            "<![endif]-->" + "\n" +

                                                                                  "<h3 style=\"display:block;font-size: 16px;font-weight:bold;color: #003B71; margin: 0\">" + GO_ONLINE_CTA_BUTTON_TEXT + "</h3>" + "\n" +
                                                                                  "<span style = \"display:block;font-size: 14px;color: #000000; text-decoration: underline\">" + GO_ONLINE_CTA_BUTTON_LINK_TEXT + "</span>" + "\n" +

                                                                            "<!--[if mso]>" + "\n" +
                                                                            "</v:rect>" + "\n" +
                                                                            "</v:textbox>" + "\n" +
                                                                            "<![endif]-->" + "\n" +

                                                                        "</td>" + "\n" +
                                                                      "</tr>" + "\n" +
                                                                  "</table>" + "\n" +
                                                              "</a>" + "\n" +
                                                          "</td>" + "\n";

        private static string bottomGoOnlineCTAMobile = "<td class=\"cta-links force-col mobile\">" + "\n" +
                                                            "<a href=\"" + GO_ONLINE_CTA_HREF + "\" target=\"_blank\" style=\"color:#ffffff;display:inline-block;font-size:16px;text-align:left;text-decoration:none;width:100%;height: auto;-webkit-text-size-adjust:none;\">" + "\n" +
                                                                "<table class=\"mobile\" style=\"width:100%;display:table-cell;border: solid 10px #DCD9DC;background-color: #DCD9DC;\">" + "\n" +
                                                                    "<tr>" + "\n" +
                                                                        "<td style=\"width: 20px\">" + "\n" +
                                                                            "<img src=\"" + CTA_CARET_URL + "\" width=\"10\" height=\"35\" style=\"vertical-align: middle\">" + "\n" +
                                                                        "</td>" + "\n" +
                                                                        "<td>" + "\n" +
                                                                        "<!--[if mso]>" + "\n" +
                                                                        "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + GO_ONLINE_CTA_HREF + "\" style=\"v-text-anchor:top;width:320px;height: auto;mso-wrap-style:square;\" target=\"_blank\" stroke=\"f\" fillcolor=\"#DCD9DC\">" + "\n" +
                                                                            "<w:anchorlock/>" + "\n" +
                                                                            "<v:textbox style =\"mso-fit-shape-to-text: t\">" + "\n" +
                                                                        "<![endif]-->" + "\n" +

                                                                                "<h3 style=\"display:block;font-size: 16px;font-weight:bold;color: #003B71; margin: 0\">" + GO_ONLINE_CTA_BUTTON_TEXT + "</h3>" + "\n" +
                                                                                "<span style = \"display:block;font-size: 14px;color: #000000; text-decoration: underline\">" + GO_ONLINE_CTA_BUTTON_LINK_TEXT + "</span>" + "\n" +

                                                                        "<!--[if mso]>" + "\n" +
                                                                        "</v:rect>" + "\n" +
                                                                        "</v:textbox>" + "\n" +
                                                                        "<![endif]-->" + "\n" +

                                                                    "</td>" + "\n" +
                                                                    "</tr>" + "\n" +
                                                                "</table>" + "\n" +
                                                            "</a>" + "\n" +
                                                        "</td>" + "\n";



        private static string bottomCallCTADesktop = "<th class=\"cta-links force-col\"" + CALL_BORDER_STYLE + ">" + "\n" +
                                                          "<a href=\"" + CALL_CTA_HREF + "\" target=\"_blank\" style=\"background-color:#DCD9DC;color:#ffffff;display:inline-block;font-size:16px;text-align:left;text-decoration:none;width:100%;height: auto;-webkit-text-size-adjust:none;\">" + "\n" +
                                                              "<table width=\"100%\" style=\"width:100%;display:table-cell;border: solid 10px #DCD9DC;background-color: #DCD9DC;\">" + "\n" +
                                                                  "<tr>" + "\n" +
                                                                      "<td style = \"width: 20px\">" + "\n" +
                                                                          "<img src=\"" + CTA_CARET_URL + "\" width=\"10\" height=\"35\" style=\"vertical-align: middle\">" + "\n" +
                                                                      "</td>" + "\n" +
                                                                      "<td>" + "\n" +
                                                                            "<!--[if mso]>" + "\n" +
                                                                            "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + CALL_CTA_HREF + "\" style=\"v-text-anchor:top;width:" + CALL_CTA_RECT_WIDTH + "height: auto;mso-wrap-style:square;\" target=\"_blank\" stroke=\"f\" fillcolor=\"#DCD9DC\">" + "\n" +
                                                                                "<w:anchorlock/>" + "\n" +
                                                                                "<v:textbox style =\"mso-fit-shape-to-text: t\">" + "\n" +
                                                                            "<![endif]-->" + "\n" +
                                                                                  "<h3 style=\"display:block;font-size: 16px;font-weight:bold;color: #003B71; margin: 0\">" + CALL_CTA_BUTTON_TEXT + "</h3>" + "\n" +
                                                                                  "<span style = \"display:block;font-size: 14px;color: #000000; text-decoration: none\">" + CALL_CTA_BUTTON_LINK_TEXT + "</span>" + "\n" +
                                                                            "<!--[if mso]>" + "\n" +
                                                                            "</v:rect>" + "\n" +
                                                                            "</v:textbox>" + "\n" +
                                                                            "<![endif]-->" + "\n" +
                                                                    "</td>" + "\n" +
                                                                  "</tr>" + "\n" +
                                                              "</table>" + "\n" +
                                                          "</a>" + "\n" +
                                                      "</th>" + "\n";

        private static string bottomCallCTADesktop1 = "<td class=\"cta-links force-col desktop\"" + CALL_BORDER_STYLE + ">" +
                                                         "<a href=\"" + CALL_CTA_HREF + "\" target=\"_blank\" style=\"background-color:#DCD9DC;color:#ffffff;display:inline-block;font-size:16px;text-align:left;text-decoration:none;width:100%;height: auto;-webkit-text-size-adjust:none;\">" +
                                                             "<table class=\"desktop\" width=\"100%\" style=\"width:100%;display:table-cell;border: solid 10px #DCD9DC;background-color: #DCD9DC;\">" +
                                                                 "<tr>" +
                                                                     "<td style = \"width: 20px\">" +
                                                                         "<img src=\"" + CTA_CARET_URL + "\" width=\"10\" height=\"35\" style=\"vertical-align: middle\">" +
                                                                     "</td>" +
                                                                     "<td>" +

                                                                           "<!--[if mso]>" +
                                                                           "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + CALL_CTA_HREF + "\" style=\"v-text-anchor:top;width:" + CALL_CTA_RECT_WIDTH + "height: auto;mso-wrap-style:square;\" target=\"_blank\" stroke=\"f\" fillcolor=\"#DCD9DC\">" +
                                                                               "<w:anchorlock/>" +
                                                                               "<v:textbox style =\"mso-fit-shape-to-text: t\">" +
                                                                           "<![endif]-->" +

                                                                                 "<h3 style=\"display:block;font-size: 16px;font-weight:bold;color: #003B71; margin: 0\">" + CALL_CTA_BUTTON_TEXT + "</h3>" +
                                                                                 "<span style = \"display:block;font-size: 14px;color: #000000; text-decoration: none\">" + CALL_CTA_BUTTON_LINK_TEXT + "</span>" +


                                                                           "<!--[if mso]>" +
                                                                           "</v:rect>" +
                                                                           "</v:textbox>" +
                                                                           "<![endif]-->" +

                                                                   "</td>" +
                                                                 "</tr>" +
                                                             "</table>" +
                                                         "</a>" +
                                                     "</td>";

        private static string bottomCallCTAMobile = "<td class=\"cta-links force-col mobile\">" + "\n" +
                                                      "<a href=\"" + CALL_CTA_HREF + "\" target=\"_blank\" style=\"background-color:#DCD9DC;color:#ffffff;display:inline-block;font-size:16px;text-align:left;text-decoration:none;width:100%;height: auto;-webkit-text-size-adjust:none;\">" + "\n" +
                                                          "<table class=\"mobile\" width=\"100%\" style=\"width:100%;display:table-cell;border: solid 10px #DCD9DC;background-color: #DCD9DC;\">" + "\n" +
                                                              "<tr>" + "\n" +
                                                                  "<td style = \"width: 20px\">" + "\n" +
                                                                      "<img src=\"" + CTA_CARET_URL + "\" width=\"10\" height=\"35\" style=\"vertical-align: middle\">" + "\n" +
                                                                  "</td>" + "\n" +
                                                                  "<td>" + "\n" +
                                                                        "<!--[if mso]>" + "\n" +
                                                                        "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + CALL_CTA_HREF + "\" style=\"v-text-anchor:top;width:320px;height: auto;mso-wrap-style:square;\" target=\"_blank\" stroke=\"f\" fillcolor=\"#DCD9DC\">" + "\n" +
                                                                            "<w:anchorlock/>" + "\n" +
                                                                            "<v:textbox style =\"mso-fit-shape-to-text: t\">" + "\n" +
                                                                        "<![endif]-->" + "\n" +
                                                                              "<h3 style=\"display:block;font-size: 16px;font-weight:bold;color: #003B71; margin: 0\">" + CALL_CTA_BUTTON_TEXT + "</h3>" + "\n" +
                                                                              "<span style = \"display:block;font-size: 14px;color: #000000; text-decoration: none\">" + CALL_CTA_BUTTON_LINK_TEXT + "</span>" + "\n" +
                                                                        "<!--[if mso]>" + "\n" +
                                                                        "</v:rect>" + "\n" +
                                                                        "</v:textbox>" + "\n" +
                                                                        "<![endif]-->" + "\n" +
                                                                "</td>" + "\n" +
                                                              "</tr>" + "\n" +
                                                          "</table>" + "\n" +
                                                      "</a>" + "\n" +
                                                  "</td>" + "\n";


        private static string bottomVisitCTADesktop = "<th class=\"cta-links force-col\"" + VISIT_BORDER_STYLE + ">" + "\n" +
                                                           "<a href=\"" + VISIT_CTA_HREF + "\" target=\"_blank\" style=\"background-color:#DCD9DC;color:#ffffff;display:inline-block;font-size:16px;text-align:left;text-decoration:none;width:100%;height: auto;-webkit-text-size-adjust:none;\">" + "\n" +
                                                               "<table style=\"width:100%;display:table-cell;border: solid 10px #DCD9DC;background-color: #DCD9DC;\">" + "\n" +
                                                                   "<tr>" + "\n" +
                                                                       "<td style = \"width: 20px\">" + "\n" +
                                                                           "<img src=\"" + CTA_CARET_URL + "\" width=\"10\" height=\"35\" style=\"vertical-align: middle\">" + "\n" +
                                                                       "</td>" + "\n" +
                                                                       "<td>" + "\n" +
                                                                            "<!--[if mso]>" + "\n" +
                                                                            "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + VISIT_CTA_HREF + "\" style=\"v-text-anchor:top;width:" + VISIT_CTA_RECT_WIDTH + "height: auto;mso-wrap-style:square;\" target=\"_blank\" stroke=\"f\" fillcolor=\"#DCD9DC\">" + "\n" +
                                                                                "<w:anchorlock/>" + "\n" +
                                                                                "<v:textbox style =\"mso-fit-shape-to-text: t\">" + "\n" +
                                                                            "<![endif]-->" + "\n" +
                                                                               "<h3 style=\"display:block;font-size: 16px;font-weight:bold;color: #003B71; margin: 0\">" + VISIT_CTA_BUTTON_TEXT + "</h3>" + "\n" +
                                                                               "<span style = \"display:block;font-size: 14px;color: #000000; text-decoration: underline\">" + VISIT_CTA_BUTTON_LINK_TEXT + "</span>" + "\n" +
                                                                                "<!--[if mso]>" + "\n" +
                                                                                "</v:rect>" + "\n" +
                                                                                "</v:textbox>" + "\n" +
                                                                                "<![endif]-->" + "\n" +
                                                                    "</td>" + "\n" +
                                                                   "</tr>" + "\n" +
                                                                "</table>" + "\n" +
                                                           "</a>" + "\n" +
                                                       "</th>" + "\n";

        private static string bottomVisitCTADesktop1 = "<td class=\"cta-links force-col desktop\"" + VISIT_BORDER_STYLE + ">" +
                                                           "<a href=\"" + VISIT_CTA_HREF + "\" target=\"_blank\" style=\"background-color:#DCD9DC;color:#ffffff;display:inline-block;font-size:16px;text-align:left;text-decoration:none;width:100%;height: auto;-webkit-text-size-adjust:none;\">" +
                                                               "<table class=\"desktop\" style=\"width:100%;display:table-cell;border: solid 10px #DCD9DC;background-color: #DCD9DC;\">" +
                                                                   "<tr>" +
                                                                       "<td style = \"width: 20px\">" +
                                                                           "<img src=\"" + CTA_CARET_URL + "\" width=\"10\" height=\"35\" style=\"vertical-align: middle\">" +
                                                                       "</td>" +
                                                                       "<td>" +

                                                                            "<!--[if mso]>" +
                                                                            "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + VISIT_CTA_HREF + "\" style=\"v-text-anchor:top;width:" + VISIT_CTA_RECT_WIDTH + "height: auto;mso-wrap-style:square;\" target=\"_blank\" stroke=\"f\" fillcolor=\"#DCD9DC\">" +
                                                                                "<w:anchorlock/>" +
                                                                                "<v:textbox style =\"mso-fit-shape-to-text: t\">" +
                                                                            "<![endif]-->" +

                                                                               "<h3 style=\"display:block;font-size: 16px;font-weight:bold;color: #003B71; margin: 0\">" + VISIT_CTA_BUTTON_TEXT + "</h3>" +
                                                                               "<span style = \"display:block;font-size: 14px;color: #000000; text-decoration: underline\">" + VISIT_CTA_BUTTON_LINK_TEXT + "</span>" +


                                                                                "<!--[if mso]>" +
                                                                                "</v:rect>" +
                                                                                "</v:textbox>" +
                                                                                "<![endif]-->" +

                                                                    "</td>" +
                                                                   "</tr>" +
                                                                "</table>" +
                                                           "</a>" +
                                                       "</td>";

        private static string bottomVisitCTAMobile = "<td class=\"cta-links force-col mobile\">" + "\n" +
                                                           "<a href=\"" + VISIT_CTA_HREF + "\" target=\"_blank\" style=\"background-color:#DCD9DC;color:#ffffff;display:inline-block;font-size:16px;text-align:left;text-decoration:none;width:100%;height: auto;-webkit-text-size-adjust:none;\">" + "\n" +
                                                               "<table class=\"mobile\" style=\"width:100%;display:table-cell;border: solid 10px #DCD9DC;background-color: #DCD9DC;\">" + "\n" +
                                                                   "<tr>" + "\n" +
                                                                       "<td style = \"width: 20px\">" + "\n" +
                                                                           "<img src=\"" + CTA_CARET_URL + "\" width=\"10\" height=\"35\" style=\"vertical-align: middle\">" + "\n" +
                                                                       "</td>" + "\n" +
                                                                       "<td>" + "\n" +
                                                                            "<!--[if mso]>" + "\n" +
                                                                            "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" href=\"" + VISIT_CTA_HREF + "\" style=\"v-text-anchor:top;width:320px;height: auto;mso-wrap-style:square;\" target=\"_blank\" stroke=\"f\" fillcolor=\"#DCD9DC\">" + "\n" +
                                                                                "<w:anchorlock/>" + "\n" +
                                                                                "<v:textbox style =\"mso-fit-shape-to-text: t\">" + "\n" +
                                                                            "<![endif]-->" + "\n" +
                                                                               "<h3 style=\"display:block;font-size: 16px;font-weight:bold;color: #003B71; margin: 0\">" + VISIT_CTA_BUTTON_TEXT + "</h3>" + "\n" +
                                                                               "<span style = \"display:block;font-size: 14px;color: #000000; text-decoration: underline\">" + VISIT_CTA_BUTTON_LINK_TEXT + "</span>" + "\n" +
                                                                                "<!--[if mso]>" + "\n" +
                                                                                "</v:rect>" + "\n" +
                                                                                "</v:textbox>" + "\n" +
                                                                                "<![endif]-->" + "\n" +
                                                                    "</td>" + "\n" +
                                                                   "</tr>" + "\n" +
                                                                "</table>" + "\n" +
                                                           "</a>" + "\n" +
                                                       "</td>" + "\n";

        private static string rightContent = "<th width=\"35%\" align=\"left\" valign=\"top\" class=\"right-content-wrapper force-col\" style=\"padding-left: 5px !important\">" + "\n" +
                                                RIGHT_RAIL_CONTENT_AREA +
                                             "</th>" + "\n";


        private string bottomCTAArea = "<tr>" + "\n" +
                                            "<td class=\"bottom-cta-container " + BOTTOM_CTA_COUNT_CLASS + "\">" + "\n" +
                                                "<table style=\"table-layout: fixed;\" border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">" + "\n" +
                                                    "<tr>" + "\n" +
                                                        bottomGoOnlineCTADesktop +
                                                        bottomCallCTADesktop +
                                                        bottomVisitCTADesktop +
                                                    "</tr>" + "\n" +
                                                "</table>" + "\n" +
                                            "</td>" + "\n" +
                                        "</tr>" + "\n";


        private string bottomCTAArea1 = "<tr>" + "\n" +
                                           "<td class=\"bottom-cta-container " + BOTTOM_CTA_COUNT_CLASS + "\">" + "\n" +
                                               "<table style=\"table-layout: fixed;\" border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">" + "\n" +
                                                   "<tr class=\"desktop\">" + "\n" +
                                                       bottomGoOnlineCTADesktop +
                                                       bottomCallCTADesktop +
                                                       bottomVisitCTADesktop +
                                                   "</tr>" + "\n" +
                                                    "<tr class=\"mobile\">" + "\n" +
                                                       bottomGoOnlineCTAMobile +
                                                       bottomCallCTAMobile +
                                                       bottomVisitCTAMobile +
                                                   "</tr>" + "\n" +
                                               "</table>" + "\n" +
                                           "</td>" + "\n" +
                                       "</tr>" + "\n";

        private string RightCTAArea = "<tr class=\"desktop\">" + bottomGoOnlineCTADesktop + "</tr>" + "\n" +
                                      "<tr class=\"mobile\">" + bottomGoOnlineCTAMobile + "</tr>" + "\n" +
                                       GO_ONLINE_CTA_ADDITIONAL_ROW +

                                       "<tr class=\"desktop\">" + bottomCallCTADesktop + "</tr>" + "\n" +
                                       "<tr class=\"mobile\">" + bottomCallCTAMobile + "</tr>" + "\n" +
                                       CALL_CTA__ADDITIONAL_ROW +

                                       "<tr class=\"desktop\">" + bottomVisitCTADesktop + "</tr>" + "\n" +
                                       "<tr class=\"mobile\">" + bottomVisitCTAMobile + "</tr>" + "\n"; 

        private string betweenCtas = "<tr><td height=\"10\" style=\"background-color: #FFFFFF;\"></td></tr>" + "\n";

        private string borderGoOnline = " style=\"border-right: solid 7px #FFFFFF\"";
        private string borderCall = " style=\"border-right: solid 3px #FFFFFF; border-left: solid 3px #FFFFFF\"";
        private string borderVisit = " style=\"border-left: solid 7px #FFFFFF\"";




        private string applyNowCTA = "<!--[if !mso]><!-- -->" + "\n" +
                                        "<a href=\"" + APPLY_NOW_CTA_URL + "\" target=\"_blank\" style=\"background-size: 100% 100%; width:150px; height:32px; color:#ffffff; display: inline-block;font-size:0;line-height:32px;border: none;border: 0;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;\"> " + "\n" +
                                           "<img src=\"" + TEMPLATE_BUTTON_LEFT_IMG_URL + "\"style=\"height:32px;width:20px;border: none;vertical-align:top;\" height=\"32\" width=\"20\"/>" + "\n" +
                                           "<span style=\"padding-right: 20px; display: inline-block;height: 32px;line-height:32px;font-size:13px; color:#ffffff; vertical-align: top;background-color:" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\">&nbsp;" + APPLY_NOW_BUTTON_TEXT + "&nbsp;&nbsp;</span>" + "\n" +
                                           "<img style=\"height: 32px;border: none;\" src=\"" + TEMPLATE_BUTTON_ARROW_IMG_URL + "\"/>" + "\n" +
                                             "<img src=\"" + TEMPLATE_BUTTON_RIGHT_IMG_URL + "\"style=\"height:32px;width:20px;border: none;vertical-align:top;\" height=\"32\" width=\"20\"/>" + "\n" +
                                        "</a>" + "\n" +
                                      "<!--<![endif]-->" + "\n" +
                                      "<!--[if mso]>" + "\n" +
                                           "<a href=\"" + APPLY_NOW_CTA_URL + "\" target=\"_blank\" style=\"text-align:center;text-decoration:none;-webkit-text-size-adjust:none; width:150px;font-size:0;\">" + "\n" +
                                                "<center style=\"vertical-align: middle\">" + "\n" +
                                                        "<img src=\"" + TEMPLATE_BUTTON_LEFT_IMG_URL + "\" style=\"height:32px;width:20px;border: none;vertical-align:middle;float: left;display: table-cell;\"height=\"32\" width=\"20\"/>" + "\n" +
                                                    "<span style=\"vertical-align: middle;font-size:13px;background-color:" + TEMPLATE_BUTTON_BACKGROUND_COLOR + ";padding: 0;margin: 0;float: right;display: table-cell;color:#FFFFFF;\">&nbsp;&nbsp;" + APPLY_NOW_BUTTON_TEXT + " &nbsp;&nbsp;&nbsp;&nbsp;</span>" + "\n" +
                                                        "<img src=\"" + TEMPLATE_BUTTON_ARROW_IMG_URL + "\" style=\"height:32px;width:10px;border: none;vertical-align:middle;float: right;display: table-cell;\"height=\"32\" width=\"10\"/>" + "\n" +
                                                    "<img src=\"" + TEMPLATE_BUTTON_RIGHT_IMG_URL + "\" style=\"height:32px;width:20px;border: none;vertical-align:middle;float: right;display: table-cell;\"height=\"32\" width=\"20\"/>" + "\n" +
                                            "</a>" + "\n" +
                                        "<![endif]-->" + "\n";


        private string firstLink = "<a href = \"" + FIRST_HEADER_LINK_URL + "\" target=\"_blank\" style=\"color:#003B73; text-decoration: underline;\">" + FIRST_HEADER_LINK_TEXT + "</a>" + "\n";
        private string secondLink = "<a href = \"" + SECOND_HEADER_LINK_URL + "\" target=\"_blank\" style=\"color:#003B73; text-decoration: underline;\">" + SECOND_HEADER_LINK_TEXT + "</a>" + "\n";
        private string thirdLink = "<a href = \"" + THIRD_HEADER_LINK_URL + "\" target=\"_blank\" style=\"color:#003B73; text-decoration: underline;\">" + THIRD_HEADER_LINK_TEXT + "</a>" + "\n";
        private string linksSeparator = "<span style=\"color:#003B73\" > | </span>";

        string headerImageNoCTA = "<td background=\"" + MAIN_IMAGE_URL + "\" bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\" width=\"600\" height=\"268\" valign=\"top\" style=\"background-size: cover;\">" + "\n" +
                                      "<a " + MAIN_IMAGE_LINK_URL + " target=\"_blank\" style=\"display:block;text-decoration: none\">" + "\n" +
                                        "<!--[if gte mso 9]>" + "\n" +
                                             "<v:image xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"true\" stroke=\"false\" style=\"border: 0;display: inline-block; width: 600px; height: 268px;\" src=\"" + MAIN_IMAGE_URL + "\" />" + "\n" +
                                                "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" " + MAIN_IMAGE_LINK_VML_URL + " fill=\"true\" stroke=\"false\" style=\"border: 0;display: inline-block;position: absolute; width: 600px; height: 268px;\">" + "\n" +
                                                    "<v:fill opacity = \"0%\" color=\"transparent\" />" + "\n" +
                                        "<![endif]-->" + "\n" +
                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                    "<tbody>" + "\n" +
                                                        "<tr>" + "\n" +
                                                             "<td align=\"center\" valign=\"top\"style=\"padding-top: 95px;\">" + "\n" +
                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                        "<tr>" + "\n" +
                                                                            "<td align=\"left\" valign=\"top\" style=\"padding-left: 30px;\">" + "\n" +
                                                                               "<p class=\"header-left-content\" style=\"font-size:18px;color:#FFFFFF; word-break: break-word;font-family: 'Trebuchet MS', Helvetica, sans-serif;line-height: 21px;\">" + HEADER_OVERLAY_COPY + "</p>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                "</table>" + "\n" +
                                                            "</td>" + "\n" +
                                                        "</tr>" + "\n" +
                                                    "</tbody>" + "\n" + "\n" +
                                                "</table>" + "\n" +
                                        "<!--[if gte mso 9]>" + "\n" +
                                            "</v:fill>" + "\n" +
                                            "</v:rect>" + "\n" +
                                            "</v:image>" + "\n" +
                                        "<![endif]-->" + "\n" +
                                            "</a>" + "\n" +
                                    "</td>" + "\n";
                                

        string headerImageNoCTAMobile = "<tr>" + "\n" +
                                            "<td align=\"center\" valign=\"middle\" style=\"background-image: url('" + MAIN_IMAGE_URL_MOBILE + "'); background-size: cover; height: 168px;\">" + "\n" +
                                                "<a " + MAIN_IMAGE_LINK_URL + " target=\"_blank\" style=\"display:block;text-decoration: none;height: 100%\">" + "\n" +
                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                    "<tbody>" + "\n" +
                                                        "<tr>" + "\n" +
                                                             "<td align=\"center\" valign=\"middle\">" + "\n" +
                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                        "<tr>" + "\n" +
                                                                             "<td align=\"left\" valign=\"middle\" style=\"padding: 20px;\">" + "\n" +
                                                                                "<p class=\"header-left-content\" style=\"font-size:18px;color:#FFFFFF; word-break: break-word;font-family: 'Trebuchet MS', Helvetica, sans-serif;line-height: 21px;\">" + HEADER_OVERLAY_COPY + "</p>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "</tbody>" + "\n" +
                                                                "</table>" + "\n" +
                                                            "</td>" + "\n" +
                                                        "</tr>" + "\n" +
                                                    "</tbody>" + "\n" +
                                                "</table>" + "\n" +
                                             "</a>" + "\n" +
                                            "</td>" + "\n" +
                                        "<tr>" + "\n";

        string headerImageWithCTA = "<td background=\"" + MAIN_IMAGE_URL + "\" bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\" width=\"600\" height=\"268\" valign=\"top\" style=\"background-size: cover;\">" + "\n" +
                                        "<!--[if gte mso 9]>" + "\n" +
                                             "<v:image xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"true\" stroke=\"false\" style=\"border: 0;display: inline-block; width: 600px; height: 268px;\" src=\"" + MAIN_IMAGE_URL + "\" />" + "\n" +
                                                "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"true\" stroke=\"false\" style=\"border: 0;display: inline-block;position: absolute; width: 600px; height: 268px;\">" + "\n" +
                                                    "<v:fill opacity = \"0%\" color=\"transparent\" />" + "\n" +
                                        "<![endif]-->" + "\n" +
                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                    "<tbody>" + "\n" +
                                                        "<tr>" + "\n" +
                                                             "<td align=\"center\" valign=\"top\"style=\"padding-top: 95px;\">" + "\n" +
                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                        "<tr>" + "\n" +
                                                                            "<td align=\"left\" valign=\"top\" style=\"padding-left: 30px;\">" + "\n" +
                                                                               "<p class=\"header-left-content\" style=\"font-size:18px;color:#FFFFFF; word-break: break-word;font-family: 'Trebuchet MS', Helvetica, sans-serif;line-height: 21px;\">" + HEADER_OVERLAY_COPY + "</p>" + "\n" +
                                                                            "</td>" + "\n" + "\n" +
                                                                            "<td align=\"right\" valign=\"top\" style=\"padding-right: 15px;padding-top: 90px\">" + "\n" +
                                                                                "<!--[if gte mso 9]>" + "\n" +
                                                                                    "<center style=\"vertical-align: middle\">" + "\n" +
                                                                                    "<w:anchorlock/>" + "\n" +
                                                                                "<![endif]-->" + "\n" +
                                                                                    "<table width=\"150\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                                        "<tbody>" + "\n" +
                                                                                            "<tr>" + "\n" +
                                                                                                "<td align=\"right\" valign=\"top\" width=\"20\">" + "\n" +
                                                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;width:20px;height:32px\" target=\"_blank\">" + "\n" +
                                                                                                        "<img width=\"20\" height=\"32\" border=\"0\" style=\"height: 32px; width: 20px;display: block;\" src=\"" + TEMPLATE_BUTTON_LEFT_IMG_URL + "\"/>" + "\n" +
                                                                                                    "</a>" + "\n" +
                                                                                                "</td>" + "\n" +
                                                                                                "<td bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\" align=\"left\" valign=\"top\">" + "\n" +
                                                                                                    "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                                                        "<tbody>" + "\n" +
                                                                                                            "<tr>" + "\n" +
                                                                                                                "<td align=\"center\" valign=\"middle\" height=\"32\" bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\" style=\"vertical-align: middle; color: #ffffff; padding: 0px 0px 0px 0px;\">" + "\n" +
                                                                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;width:110px;font-size:15px;line-height:32px; color: #ffffff; font-family: 'Trebuchet MS', Helvetica, sans-serif;\" target=\"_blank\">" + "\n" +
                                                                                                                        HEADER_CTA_COPY +
                                                                                                                    "</a>" + "\n" +
                                                                                                                "</td>" + "\n" +
                                                                                                                "<td align=\"left\" valign=\"top\" width=\"10\" height=\"32\">" + "\n" +
                                                                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;width:10px;height:32px\" target=\"_blank\">" + "\n" +
                                                                                                                        "<img border=\"0\" width=\"10\" height=\"32\" style=\"height: 32px; width: 10px;display: block;\" src=\"" + TEMPLATE_BUTTON_ARROW_IMG_URL + "\"/>" + "\n" +
                                                                                                                    "</a>" + "\n" +
                                                                                                                "</td>" + "\n" +
                                                                                                            "</tr>" + "\n" +
                                                                                                        "</tbody>" + "\n" +
                                                                                                    "</table>" + "\n" +
                                                                                                "</td>" + "\n" +
                                                                                                "<td align=\"left\" valign=\"top\" width=\"20\">" + "\n" +
                                                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;width:20px;height:32px\" target=\"_blank\">" + "\n" +
                                                                                                        "<img border=\"0\" width=\"20\" height=\"32\" style=\"height: 32px; width: 20px;display: block;\" src=\"" + TEMPLATE_BUTTON_RIGHT_IMG_URL + "\"/>" + "\n" +
                                                                                                    "</a>" + "\n" +
                                                                                                "</td>" + "\n" +
                                                                                            "</tr>" + "\n" +
                                                                                        "</tbody>" + "\n" +
                                                                                    "</table>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                "</table>" + "\n" +
                                                            "</td>" + "\n" +
                                                        "</tr>" + "\n" +
                                                    "</tbody>" + "\n" +
                                                "</table>" + "\n" +
                                        "<!--[if gte mso 9]>" + "\n" +
                                            "</v:fill>" + "\n" +
                                                    "</v:rect>" +
                                            "</v:image>" +
                                        "<![endif]-->" +
                                    "</td>" + "\n";

        string headerImageWithCTAMobile = "<tr>" + "\n" +
                                            "<td align=\"center\" valign=\"middle\" style=\"background-image: url('" + MAIN_IMAGE_URL_MOBILE + "'); background-size: 100% 100%; height: 168px;\">" + "\n" +
                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                    "<tbody>" + "\n" +
                                                        "<tr>" + "\n" +
                                                             "<td align=\"center\" valign=\"middle\">" + "\n" +
                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                        "<tr>" + "\n" +
                                                                             "<td align=\"left\" valign=\"middle\" style=\"padding: 20px;\">" + "\n" +
                                                                                "<p class=\"header-left-content\" style=\"font-size:18px;color:#FFFFFF; word-break: break-word;font-family: 'Trebuchet MS', Helvetica, sans-serif;line-height: 21px;\">" + HEADER_OVERLAY_COPY + "</p>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "</tbody>" + "\n" +
                                                                "</table>" + "\n" +
                                                            "</td>" + "\n" +
                                                        "</tr>" + "\n" +
                                                    "</tbody>" + "\n" +
                                                "</table>" + "\n" +
                                            "</td>" + "\n" +
                                        "<tr>" + "\n" +
                                        "<tr>" + "\n" +
                                            "<td align=\"center\" valign=\"middle\" height=\"80\" style=\"background-color: #FFFFFF;vertical-align:middle;\">" + "\n" +
                                                    "<table width=\"170\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                        "<tbody>" + "\n" +
                                                            "<tr>" + "\n" +
                                                                "<td align=\"left\" valign=\"top\" width=\"20\">" + "\n" +
                                                                    "<img border=\"0\" style=\"height: 32px; width: 20px;display: block;\" src=\"" + TEMPLATE_BUTTON_LEFT_IMG_URL + "\"/>" + "\n" +
                                                                "</td>" + "\n" +
                                                                "<td align=\"left\" valign=\"top\">" + "\n" +
                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;\" target=\"_blank\">" + "\n" +
                                                                        "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                            "<tbody>" + "\n" +
                                                                                "<tr>" + "\n" +
                                                                                    "<td align=\"center\" valign=\"middle\" height=\"32\" bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\" style=\"vertical-align: middle; color: #ffffff; padding: 0px 0px 0px 0px; font-family: 'Trebuchet MS', Helvetica, sans-serif;font-size:15px; line-height: 18px;\">" + "\n" +
                                                                                        HEADER_CTA_COPY +
                                                                                    "</td>" + "\n" +
                                                                                    "<td align=\"left\" valign=\"top\" width=\"10\" height=\"32\">" + "\n" +
                                                                                        "<img border=\"0\" style=\"height: 32px; width: 10px;display: block;\" src=\"" + TEMPLATE_BUTTON_ARROW_IMG_URL + "\"/>" + "\n" +
                                                                                    "</td>" + "\n" +
                                                                                "</tr>" + "\n" +
                                                                            "</tbody>" + "\n" +
                                                                            "</table>" + "\n" +
                                                                    "</a>" + "\n" +
                                                                "</td>" + "\n" +
                                                                "<td align=\"left\" valign=\"top\" width=\"20\">" + "\n" +
                                                                    "<img border=\"0\" style=\"height: 32px; width: 20px;display: block;\" src=\"" + TEMPLATE_BUTTON_RIGHT_IMG_URL + "\"/>" + "\n" +
                                                                "</td>" + "\n" +
                                                            "</tr>" + "\n" +
                                                        "</tbody>" + "\n" +
                                                    "</table>" + "\n" +
                                                "</td>" + "\n" +
                                        "<tr>" + "\n";

        string backgroundColorWithCTA = "<td background=\"" + TEMPLATE_HEADER_IMG_URL + "\" bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\" width=\"600\" height=\"268\" valign=\"top\" style=\"background-size: 100% 100%;\">" + "\n" +
                                        "<!--[if gte mso 9]>" + "\n" +
                                             "<v:image xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"true\" stroke=\"false\" style=\"border: 0;display: inline-block; width: 600px; height: 268px;\" src=\"" + TEMPLATE_HEADER_IMG_URL + "\" />" + "\n" +
                                                "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"true\" stroke=\"false\" style=\"border: 0;display: inline-block;position: absolute; width: 600px; height: 268px;\">" + "\n" +
                                                    "<v:fill opacity = \"0%\" color=\"transparent\" />" + "\n" +
                                        "<![endif]-->" + "\n" +
                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                    "<tbody>" + "\n" +
                                                        "<tr>" + "\n" +
                                                             "<td align=\"center\" valign=\"top\"style=\"padding-top: 95px;\">" + "\n" +
                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                        "<tr>" + "\n" +
                                                                            "<td align=\"left\" valign=\"top\" style=\"padding-left: 30px;\">" + "\n" +
                                                                               "<p class=\"header-left-content\" style=\"font-size:18px;color:#FFFFFF; word-break: break-word;font-family: 'Trebuchet MS', Helvetica, sans-serif;line-height: 21px;\">" + HEADER_OVERLAY_COPY + "</p>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                            "<td align=\"right\" valign=\"top\" style=\"padding-right: 15px;;padding-top: 90px\">" + "\n" +
                                                                               
                                                                                    "<table width=\"150\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                                        "<tbody>" + "\n" +
                                                                                            "<tr>" + "\n" +
                                                                                                "<td align=\"right\" valign=\"top\" width=\"20\">" + "\n" +
                                                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;width:20px;height:32px\" target=\"_blank\">" + "\n" +
                                                                                                        "<img width=\"20\" height=\"32\" border=\"0\" style=\"height: 32px; width: 20px;display: block;\" src=\"" + TEMPLATE_BUTTON_LEFT_IMG_URL + "\"/>" + "\n" +
                                                                                                    "</a>" + "\n" +
                                                                                                "</td>" + "\n" +
                                                                                                "<td align=\"left\" valign=\"top\" bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\">" + "\n" +
                                                                                                    "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                                                        "<tbody>" + "\n" +
                                                                                                            "<tr>" + "\n" +
                                                                                                                "<td align=\"center\" valign=\"middle\" height=\"32\" bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\" style=\"vertical-align: middle; padding: 0px 0px 0px 0px;\">" + "\n" +
                                                                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;width:110px;font-size:15px;line-height:32px; color: #ffffff; font-family: 'Trebuchet MS', Helvetica, sans-serif;\" target=\"_blank\">" + "\n" +
                                                                                                                        HEADER_CTA_COPY +
                                                                                                                    "</a>" + "\n" +
                                                                                                                "</td>" + "\n" +
                                                                                                                "<td align=\"left\" valign=\"top\" width=\"10\" height=\"32\">" + "\n" +
                                                                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;width:10px;height:32px\" target=\"_blank\">" + "\n" +
                                                                                                                        "<img border=\"0\" width=\"10\" height=\"32\" style=\"height: 32px; width: 10px;display: block;\" src=\"" + TEMPLATE_BUTTON_ARROW_IMG_URL + "\"/>" + "\n" +
                                                                                                                    "</a>" + "\n" +
                                                                                                                "</td>" + "\n" +
                                                                                                            "</tr>" + "\n" +
                                                                                                        "</tbody>" + "\n" +
                                                                                                    "</table>" + "\n" +
                                                                                                "</td>" + "\n" +
                                                                                                "<td align=\"left\" valign=\"top\" width=\"20\">" + "\n" +
                                                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;width:20px;height:32px\" target=\"_blank\">" + "\n" +
                                                                                                        "<img border=\"0\" width=\"20\" height=\"32\" style=\"height: 32px; width: 20px;display: block;\" src=\"" + TEMPLATE_BUTTON_RIGHT_IMG_URL + "\"/>" + "\n" +
                                                                                                    "</a>" + "\n" +
                                                                                                "</td>" + "\n" +
                                                                                            "</tr>" + "\n" +
                                                                                        "</tbody>" + "\n" +
                                                                                    "</table>" + "\n" +
                                                                                "</a>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                "</table>" + "\n" +
                                                            "</td>" + "\n" +
                                                        "</tr>" + "\n" +
                                                    "</tbody>" + "\n" +
                                                "</table>" + "\n" +
                                        "<!--[if gte mso 9]>" + "\n" +
                                            "</v:fill>" + "\n" +
                                            "</v:rect>" + "\n" +
                                            "</v:image>" + "\n" +
                                        "<![endif]-->" + "\n" +
                                    "</td>" + "\n";

        string backgroundColorWithCTAMobile = "<tr>" + "\n" +
                                            "<td align=\"center\" valign=\"top\" style=\"background-image: url('" + TEMPLATE_HEADER_IMG_MOB_URL + "'); background-size: 100% 100%; height: 168px;\">" + "\n" +
                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                    "<tbody>" + "\n" +
                                                        "<tr>" + "\n" +
                                                             "<td align=\"center\" valign=\"top\">" + "\n" +
                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                        "<tr>" + "\n" +
                                                                             "<td align=\"left\" valign=\"top\" style=\"padding: 20px;\">" + "\n" +
                                                                                "<p class=\"header-left-content\" style=\"font-size:18px;color:#FFFFFF; word-break: break-word;font-family: 'Trebuchet MS', Helvetica, sans-serif;line-height: 21px;\">" + HEADER_OVERLAY_COPY + "</p>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "</tbody>" + "\n" +
                                                                "</table>" + "\n" +
                                                            "</td>" + "\n" +
                                                        "</tr>" + "\n" +
                                                    "</tbody>" + "\n" +
                                                "</table>" + "\n" +
                                            "</td>" + "\n" +
                                        "<tr>" + "\n" +
                                        "<tr>" + "\n" +
                                            "<td align=\"center\" valign=\"middle\" height=\"80\" style=\"background-color: #FFFFFF;vertical-align:middle;\">" + "\n" +
                                                    "<table width=\"170\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                        "<tbody>" + "\n" +
                                                            "<tr>" + "\n" +
                                                                "<td align=\"left\" valign=\"top\" width=\"20\">" + "\n" +
                                                                    "<img border=\"0\" style=\"height: 32px; width: 20px;display: block;\" src=\"" + TEMPLATE_BUTTON_LEFT_IMG_URL + "\"/>" + "\n" +
                                                                "</td>" + "\n" +
                                                                "<td align=\"left\" valign=\"top\">" + "\n" +
                                                                    "<a " + MAIN_IMAGE_LINK_URL + "style=\"display:block;text-align:center;text-decoration:none;-webkit-text-size-adjust:none;\" target=\"_blank\">" + "\n" +
                                                                        "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                            "<tbody>" + "\n" +
                                                                                "<tr>" + "\n" +
                                                                                    "<td align=\"center\" valign=\"middle\" height=\"32\" bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\" style=\"vertical-align: middle; color: #ffffff; padding: 0px 0px 0px 0px; font-family: 'Trebuchet MS', Helvetica, sans-serif;font-size:15px; line-height: 18px;\">" + "\n" +
                                                                                        HEADER_CTA_COPY +
                                                                                    "</td>" + "\n" +
                                                                                    "<td align=\"left\" valign=\"top\" width=\"10\" height=\"32\">" + "\n" +
                                                                                        "<img border=\"0\" style=\"height: 32px; width: 10px;display: block;\" src=\"" + TEMPLATE_BUTTON_ARROW_IMG_URL + "\"/>" + "\n" +
                                                                                    "</td>" + "\n" +
                                                                                "</tr>" + "\n" +
                                                                            "</tbody>" + "\n" +
                                                                            "</table>" + "\n" +
                                                                    "</a>" + "\n" +
                                                                "</td>" + "\n" +
                                                                "<td align=\"left\" valign=\"top\" width=\"20\">" + "\n" +
                                                                    "<img border=\"0\" style=\"height: 32px; width: 20px;display: block;\" src=\"" + TEMPLATE_BUTTON_RIGHT_IMG_URL + "\"/>" + "\n" +
                                                                "</td>" + "\n" +
                                                            "</tr>" + "\n" +
                                                        "</tbody>" + "\n" +
                                                    "</table>" + "\n" +
                                                "</td>" + "\n" +
                                        "<tr>" + "\n";

        string backgroundColorNoCTA = "<td background=\"" + TEMPLATE_HEADER_IMG_URL + "\" bgcolor=\"" + TEMPLATE_BUTTON_BACKGROUND_COLOR + "\" width=\"600\" height=\"268\" valign=\"top\" style=\"background-size: 100% 100%;\">" + "\n" +
                                     "<a " + MAIN_IMAGE_LINK_URL + " target=\"_blank\" style=\"display:block;text-decoration: none;height: 100%\">" + "\n" +
                                       "<!--[if gte mso 9]>" + "\n" +
                                            "<v:image xmlns:v=\"urn:schemas-microsoft-com:vml\" fill=\"true\" stroke=\"false\" style=\"border: 0;display: inline-block; width: 600px; height: 268px;\" src=\"" + TEMPLATE_HEADER_IMG_URL + "\" />" + "\n" +
                                               "<v:rect xmlns:v=\"urn:schemas-microsoft-com:vml\"" + MAIN_IMAGE_LINK_VML_URL + " fill=\"true\" stroke=\"false\" style=\"border: 0;display: inline-block;position: absolute; width: 600px; height: 268px;\">" + "\n" +
                                                   "<v:fill opacity = \"0%\" color=\"transparent\" />" + "\n" +
                                       "<![endif]-->" + "\n" +
                                               "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                   "<tbody>" + "\n" +
                                                       "<tr>" + "\n" +
                                                            "<td align=\"center\" valign=\"top\"style=\"padding-top: 95px;\">" + "\n" +
                                                               "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                   "<tbody>" + "\n" +
                                                                       "<tr>" + "\n" +
                                                                           "<td align=\"left\" valign=\"top\" style=\"padding-left: 30px;\">" + "\n" +
                                                                              "<p class=\"header-left-content\" style=\"font-size:18px;color:#FFFFFF; word-break: break-word;font-family: 'Trebuchet MS', Helvetica, sans-serif;line-height: 21px;\">" + HEADER_OVERLAY_COPY + "</p>" + "\n" +
                                                                           "</td>" + "\n" +
                                                                       "</tr>" + "\n" +
                                                                   "<tbody>" + "\n" +
                                                               "</table>" + "\n" +
                                                           "</td>" + "\n" +
                                                       "</tr>" + "\n" +
                                                   "</tbody>" + "\n" +
                                               "</table>" + "\n" +
                                       "<!--[if gte mso 9]>" + "\n" +
                                           "</v:fill>" + "\n" +
                                           "</v:rect>" + "\n" +
                                           "</v:image>" + "\n" +
                                       "<![endif]-->" + "\n" +
                                           "</a>" + "\n" +
                                   "</td>" + "\n";


        string backgroundColorNoCTAMobile = "<tr>" + "\n" +
                                            "<td align=\"center\" valign=\"middle\" style=\"background-image: url('" + TEMPLATE_HEADER_IMG_MOB_URL + "'); background-size: 100% 100%; height: 168px;\">" + "\n" +
                                                "<a " + MAIN_IMAGE_LINK_URL + " target=\"_blank\" style=\"display:block;text-decoration: none;height: 100%\">" + "\n" +
                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                    "<tbody>" + "\n" +
                                                        "<tr>" + "\n" +
                                                             "<td align=\"center\" valign=\"top\">" + "\n" +
                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                    "<tbody>" + "\n" +
                                                                        "<tr>" + "\n" +
                                                                             "<td align=\"left\" valign=\"top\" style=\"padding: 20px;\">" + "\n" +
                                                                                "<p class=\"header-left-content\" style=\"font-size:18px;color:#FFFFFF; word-break: break-word;font-family: 'Trebuchet MS', Helvetica, sans-serif;line-height: 21px;\">" + HEADER_OVERLAY_COPY + "</p>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "</tbody>" + "\n" +
                                                                "</table>" + "\n" +
                                                            "</td>" + "\n" +
                                                        "</tr>" + "\n" +
                                                    "</tbody>" + "\n" +
                                                "</table>" + "\n" +
                                             "</a>" + "\n" +
                                            "</td>" + "\n" +
                                        "<tr>" + "\n";



        /*-- Email Template sections --*/
        const int EMAIL_TEMPLATE_SECTION = 24;

        /*-- Components IDs --*/
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
        const int RIGHT_RAIL_COMP_ID = 13;
        const int EMAIL_CALL_TO_ACTION_COMP_ID = 14;
        const int EMAIL_HEADER_LINKS_COMP_ID = 15;


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



        private st1001Entities db = new st1001Entities();
        private DbSet<SectionType> sectionsTypes;

        public ProjectHTMLHelper()
        {
            this.sectionsTypes = db.SectionTypes;
        }

        public string GetAsHTML(string projectID, string projectName, LayoutViewModel contentID, string emailFolderPath)
        {
            string html = "<!DOCTYPE html>" + "\n" +
            "<html lang=\"en\">" + "\n" +
            "<head>" + "\n" +
            "<meta http-equiv=\"Content-Type\" content=\"text/html charset=UTF-8\"/>" + "\n" +
            "<meta name = \"viewport\" content=\"width=device-width, initial-scale=1\" />" + "\n" +
            "<title>Email Template</title>" + "\n" +
            "<style type = \"text/css\" >" + "\n" +
                "body{font-family: \"Trebuchet MS\", Helvetica, sans-serif;font-size: 14px;color: #555555;}" + "\n" +
                ".logo { width: 198px }" + "\n" +
                ".applelinke a {color:inherit; text-decoration: inherit;}" + "\n" +
                ".right-content .cta-links.desktop, .bottom-cta-container .cta-links.desktop{display:table-cell;}" + "\n" +
                ".right-content .cta-links.mobile, .bottom-cta-container .cta-links.mobile, .header-text.mobile{display:none !important;}" + "\n" +
                ".wrapper { background-color: #DCD9DC; max-width: 600px; width: 600px; margin: 0 auto;display: block;}" + "\n" +
                ".content-container {padding: 20px;}" + "\n" +
                ".section.logo-container { text-align: left;}" + "\n" +
                ".links-container {text-align: right;display: table-cell;}" + "\n" +
                ".links-wrapper .last-social-net {padding-right: 20px;}" + "\n" +
                ".links-wrapper .social-container {border-top: #DCD9DC solid 15px;}" + "\n" +
                ".right-content .cta-links {width: 100%;display: block;border: none !important;}" + "\n" +
                ".main-content-wrapper.right-content-active tr td.left-content-wrapper {font-size: 14px;}" + "\n" +
                ".right-content th.right-content-wrapper.image-component {padding-top: 20px;text-align: left;}" + "\n" +
                ".html-view .img {width: 100%;vertical-align: top;}" + "\n" +
                ".bottom-cta-container {padding: 0 20px 20px 20px;}" + "\n" +
                ".cta-links a {background-color: #DCD9DC;}" + "\n" +
                ".cta-links span {word-break: break-all;}" + "\n" +
                ".three-items .cta-links {width: 33.3%}" + "\n" +
                ".two-items .cta-links { width: 50%;} .two-items .cta-links:first-child {margin-right: 10px;} .one-item .cta-links {width: 100%;margin: 0;border: none !important;}" + "\n" +
                "a, table, table tr, table tr td {vertical-align: top;border-collapse: collapse;margin: 0;padding: 0;}" + "\n" +
                ".disclaimer-container {padding: 20px;}" + "\n" +
                ".disclaimer-container a {text-decoration: underline;color: #428bca;}" + "\n" +
                ".desktop {display: table-cell !important;width:auto !important;height:auto !important;overflow:visible !important;float:none !important;visibility:visible !important;}" + "\n" +
                "tr.desktop{display: table-row !important}" + "\n" +
                ".mobile {display:none !important;mso-hide:all !important;overflow:hidden !important;width:0 !important;height:0 !important;float:none !important;visibility:hidden !important;}" + "\n" +
                ".right-content .cta-links.desktop, .bottom-cta-container .cta-links table.desktop{width: 100% !important;}" + "\n" +
                ".bottom-cta-container.three-items .cta-links.desktop {width: 33.3% !important;}" + "\n" +
                ".bottom-cta-container.two-items .cta-links.desktop {width: 50% !important;}" + "\n" +
                ".bottom-cta-container.one-item .cta-links.desktop {width: 100% !important;}" + "\n" +
                ".apply-now.hidden {display: none !important;}" + "\n" +
                ".header-content {display: none;}" + "\n" +
                ".header-content.default-height {display: table;}" + "\n" +
                ".header-cta-container {width: 50%;height: 268px;vertical-align: middle;text-align:center}" + "\n" +
                ".header-cta-container {width: 40%;padding-top: 50px;}" + "\n" +
                ".header-cta-container .apply-now {margin-top: 150px !important; width: 159px;}" + "\n" +
                ".header-cta-container .header-cta {line-height: 32px;}" + "\n" +
                "th {font-weight: normal !important;padding: 0px !important;margin: 0px !important;}" + "\n" +
                "@media screen {" + "\n" +
                    "@import url(http://fonts.googleapis.com/css?family=Trebuchet+MS);" + "\n" +
                    "body,table,td,a,span,b,p,div {font-family: \"Trebuchet MS\", Helvetica, sans-serif !important;}" + "\n" +
                "}" + "\n" +
                "@media screen and (max-width: 520px) {" + "\n" +
                    ".logo { width: 120px} .section.logo, .section.links {width: 100%;} .links-container {text-align: center;}" + "\n" +
                    ".main-content-container {padding: 10px;}" + "\n" +
                    ".wrapper { width: 100%;}" + "\n" +
                    ".right-content .cta-links.mobile, .bottom-cta-container .cta-links.mobile, .header-text.mobile{display:block !important;}" + "\n" +
                    ".right-content .cta-links.desktop, .bottom-cta-container .cta-links.desktop{display:none !important;}" + "\n" +
                    ".content-container {padding: 10px;}" + "\n" +
                    ".mobile-center-block {float: none !important;margin: 0 auto !important;}" + "\n" +
                    "td[class=\"force-col-center\"] {text-align: center !important;width: 100% !important;display: block;}" + "\n" +
                    ".links-wrapper .last-social-net {padding-right: 0 !important;}" + "\n" +
                    "*[class].force-col, .force-col {width: 100% !important;display: block !important;clear: both !important;float: inherit !important;}" + "\n" +
                    "#main-content th.right-content-wrapper.force-col {padding: 20px 0 0 0 !important; border-top: 1px solid #DCD9DC;}" + "\n" +
                    ".main-content-wrapper.right-content-active {width: 100%; border-right: 0 none !important;}" + "\n" +
                    ".main-content-wrapper.right-content-active tr td.left-content-wrapper {padding: 0 0 20px 0 !important}" + "\n" +
                    ".right-content {width: 100%;}" + "\n" +
                    "#main-content th.right-content-wrapper {width: 100% !important;}" + "\n" +
                    ".bottom-cta-container {padding: 10px;}" + "\n" +
                    ".disclaimer-container {padding: 10px;}" + "\n" +
                    ".one-item .cta-links, .two-items .cta-links, .three-items .cta-links { width: 100%;margin: 10px 0 !important;border: none !important;}" + "\n" +
                    ".mobile {display: table-cell !important;width:auto !important;height:auto !important;overflow:visible !important;float:none !important;visibility:visible !important;}" + "\n" +
                    "tr.desktop, .desktop {display:none !important;mso-hide:all !important;overflow:hidden !important;display: none !important;width:0 !important;height:0 !important;overflow:hidden !important;float:none !important;visibility:hidden !important;}" + "\n" +
                    ".apply-now {display: block !important;text-align: center !important;}.apply-now.hidden {display: none !important;}" + "\n" +
                    ".header-cta-container {width: 100%;display: inline-table;height: 180px;text-align: center;}" + "\n" +
                    ".header-text.mobile {display: block !important;height: 176px !important;}" + "\n" +
                    ".header-cta-container {height: 60px;background-color: #FFFFFF !important;padding: 0 10px;}" + "\n" +
                    ".header-cta-container .apply-now {margin-top: 20px !important;width: 162px}" + "\n" +
                    ".header-cta-container.force-col .apply-now {margin-top: 0px !important;width: 162px; padding-top: 20px;}" + "\n" +
                    ".header-cta-container.force-col {width: auto !important;}" + "\n" +
                    ".header-cta-container.hidden-mobile {display: none;}" + "\n" +
                    ".header-cta-container .header-cta {line-height: 30px;}" + "\n" +
                    ".header-text .header-left-content {padding-top: 20px;}" + "\n" +
                    ".mobimg {width: 100% !important; height: auto !important;}" + "\n" +
                    "table[class=\"tbl\"], .tbl {width: 100% !important;padding: 0 0 0 0 !important;border: none !important;min-width: 100% !important;}" + "\n" +
                    "*[class].displayBlock, .displayBlock {width: 100% !important;display: block !important;}" + "\n" +
                    "*[class].table-main, .table-main {width:100% !important;min-width: 100% !important;}" + "\n" +
                    "*[class].f12P, .f12P{font-size:13px !important; line-height: 15px !important;}" + "\n" +
                    "*[class].Acenter, .Acenter{text-align:center !important;}" + "\n" +
                    "*[class].pdbtm10, .pdbtm10{padding-bottom:10px !important;}" + "\n" +
                    "*[class].mobileshow, .mobileshow {display: block !important;width: auto !important;overflow: visible !important;float: none !important;max-height: none !important;line-height: normal !important;visibility: visible !important;font-size: inherit !important;}" + "\n" +
                    ".fullgmailfix, .fullgmailfix{width:100% !important; min-width: 100% !important;}" + "\n" +
                    "}" + "\n" +
            "</style>" + "\n" +
            "<!--[if mso | ie]>" + "\n" +
                "<style>" + "\n" +
                ".sup {vertical-align: 1px !important; font-size: 100% !important;}" + "\n" +
                "</style>" + "\n" +
            "<![endif]-->" + "\n" +
            "<!--[if ie]>" + "\n" +
                "<style>" + "\n" +
                    ".sup {vertical-align: 6px !important; font-size: 60% !important;}" + "\n" +
                "</style>" + "\n" +
            "<![endif]-->" + "\n" +
            "<!--[if mso]>" + "\n" +
                "<style type=\"text/css\">" + "\n" +
                    ".tbloutlook1 {width: 297px !important;}" + "\n" +
                "</style>" + "\n" +
            "<![endif]-->" + "\n" +

            "<!--[if (gte mso 15)]>" + "\n" +
                "<style type=\"text/css\">" + "\n" +
                    ".tbloutlook1 {width: 297px !important;}" + "\n" +
                "</style>" + "\n" +
            "<![endif]-->" + "\n" +
            "</head>" + "\n" +
            "<body style=\"margin: 0px; padding: 0px;\">" + "\n" +
                "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                    "<tr>" + "\n" +
                        "<td align=\"center\" valign=\"top\">" + "\n" +
                            "<table width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"min-width: 600px;\" class=\"fullgmailfix\">" + "\n" +
                                "<tbody>" + "\n" +
                                    "<tr>" + "\n" +
                                        "<td align=\"center\" valign=\"top\">" + "\n" +
                                            "<table class=\"wrapper\" id=\"content-wrapper\" align=\"center\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\">" + "\n" +
                                                "<tr>" + "\n" +
                                                    "<td class=\"content-container\">" + "\n" +
                                                        "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">" + "\n" +
                                                            "<tr>" + "\n" +
                                                                "<td align=\"center\" valign=\"top\">" + "\n" +
                                                                    "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">" + "\n" +
                                                                        "<tr>" + "\n" +
                                                                            "<td align =\"center\" valign=\"top\" style=\"font-size: 10px; padding-bottom:10px;font-family: 'Trebuchet MS', Helvetica, sans-serif; line-height: 13px;\">If you are unable to see the message below, <a href= \"" + baseUrl + emailFolderPath + "\" target=\"_blank\" style=\"color:#003B73;\">click here</a> to view</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "</table>" +
                                                                "</td>" + "\n" +
                                                            "</tr>" + "\n" +
                                                            "<tr>" + "\n" +
                                                                "<td align=\"center\" valign=\"top\">" + "\n" +
                                                                    "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">" + "\n" +
                                                                        "<tbody>" + "\n" +
                                                                            "<tr>" + "\n" +
                                                                                "<td align=\"center\" valign=\"top\" style=\"padding-bottom: 10px;\">" + "\n" +
                                                                                    "<table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" + "\n" +
                                                                                        "<tr>" + "\n" +
                                                                                            "<th align=\"center\" width=\"250\" class=\"displayBlock\">" + "\n" +
                                                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\"cellpadding=\"0\"> " + "\n" +
                                                                                                    "<tbody>" + "\n" +
                                                                                                        "<tr>" + "\n" +
                                                                                                            "<td class=\"force-col-center Acenter\" align=\"center\" valign=\"top\" style=\"padding: 0;text-align: left;\">" + "\n" +
                                                                                                                "<a href=\"http://suntrust.com\" style=\"display: inline-block;\" target=\"_blank\">" + "\n" +
                                                                                                                    "<img class=\"logo\" src=\"" + SUNTRUST_LOGO_URL + "\" alt=\"Suntrust logo\" style=\"display: block\" border=\"0\">" + "\n" +
                                                                                                                "</a>" + "\n" +
                                                                                                            "</td>" + "\n" +
                                                                                                        "</tr>" + "\n" +
                                                                                                     "</tbody>" + "\n" +
                                                                                                "</table>" + "\n" +
                                                                                            "</th>" + "\n" +
                                                                                            "<th align=\"center\" valign=\"bottom\" width=\"309\" class=\"displayBlock pdbtm10\">" + "\n" +
                                                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\"cellpadding=\"0\">" + "\n" +
                                                                                                    "<tbody>" + "\n" +
                                                                                                        "<tr>" + "\n" +
                                                                                                            "<td align=\"right\" valign=\"bottom\">" + "\n" +
                                                                                                                "<table width=\"100%\" border=\"0\" cellspacing=\"0\"cellpadding=\"0\">" + "\n" +
                                                                                                                    "<tbody>" + "\n" +
                                                                                                                        "<tr>" + "\n" +
                                                                                                                            "<td align=\"center\" valign=\"bottom\" class=\"force-col-center f12P Acenter\" style=\"padding-top:10px;vertical-align: bottom; text-align: right; font-family: 'Trebuchet MS', Helvetica, sans-serif;font-size:14px; line-height: 17px; font-weight: normal;\">" + "\n" +
                                                                                                                                    FIRST_HEADER_LINK +
                                                                                                                                    FIRST_HEADER_LINK_SEPARATOR +
                                                                                                                                    SECOND_HEADER_LINK +
                                                                                                                                    SECOND_HEADER_LINK_SEPARATOR +
                                                                                                                                    THIRD_HEADER_LINK +
                                                                                                                            "</td>" + "\n" +
                                                                                                                        "</tr>" + "\n" +
                                                                                                                    "</tbody>" + "\n" +
                                                                                                                "</table>" + "\n" +
                                                                                                            "</td>" + "\n" +
                                                                                                        "</tr>" + "\n" +
                                                                                                    "</tbody>" + "\n" +
                                                                                                "</table>" + "\n" +
                                                                                            "</th>" + "\n" +
                                                                                        "</tr>" + "\n" +
                                                                                    "</table>" + "\n" +
                                                                                "</td>" + "\n" +
                                                                            "</tr>" + "\n" +
                                                                        "</tbody>" + "\n" +
                                                                    "</table>" + "\n" +
                                                                "</td>" + "\n" +
                                                            "</tr>" + "\n" +
                                                            "<tr>" + "\n" +
                                                                "<td align=\"center\" valign=\"top\">" + "\n" +
                                                                    "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"desktop\">" + "\n" +
                                                                        "<tbody>" + "\n" +
                                                                            "<tr>" + "\n" +
                                                                                HEADER_CONTENT +
                                                                            "</tr>" + "\n" +
                                                                        "</tbody>" + "\n" +
                                                                    "</table>" + "\n" +
                                                                "<!--[if !mso]><!-- -->" + "\n" +
                                                                    "<div class=\"mobileshow\" style=\"width: 0; max-height: 0; overflow: hidden; display: none\">" + "\n" +
                                                                        "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" align=\"center\" class=\"table-main\">" + "\n" +
                                                                             HEADER_CONTENT_MOBILE +
                                                                        "</table>" + "\n" +
                                                                    "</div>" + "\n" +
                                                                    "<!--<![endif]-->" + "\n" +
                                                                "</td>" + "\n" +
                                                            "</tr>" + "\n" +
                                                            "<tr>" + "\n" +
                                                                "<td>" + "\n" +
                                                                    "<table  id=\"main-content\" border=\"0\" width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" style=\"background-color:#FFFFFF;display:inline-table\">" + "\n" +
                                                                        "<tr class=\"left\" style=\"display: table-cell;\">" + "\n" +
                                                                            "<td class=\"main-content-container\" style=\"padding: 20px;\">" + "\n" +
                                                                                "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" + "\n" +
                                                                                    "<tr>" + "\n" +
                                                                                        "<th width=\"" + LEFT_CONTENT_COLUMN_WIDTH + "\" class=\"main-content-wrapper force-col " + MAIN_CONTENT_TABLE_CLASS + "\" " + MAIN_CONTENT_TABLE_STYLE + ">" + "\n" +
                                                                                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"display: inline-table; vertical-align:top\" class=\"left-content\">" + "\n" +
                                                                                                "<tr>" + "\n" +
                                                                                                    "<td class=\"left-content-wrapper\" style=\"padding-right: 5px; font-family: 'Trebuchet MS', Helvetica, sans-serif;font-size:14px; line-height: 17px; color: #555555;font-weight: normal;\">" + "\n" +
                                                                                                            MAIN_CONTENT_AREA +
                                                                                                    "</td>" + "\n" +
                                                                                                "</tr>" + "\n" +
                                                                                            "</table>" + "\n" + "\n" +
                                                                                        "</th>" + "\n" +
                                                                                            RIGHT_CONTENT +
                                                                                    "</tr>" + "\n" +
                                                                                "</table>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                        BOTTOM_CTA_AREA_CONTENT +
                                                                    "</table>" + "\n" +
                                                                "</td>" + "\n" +
                                                            "</tr>" + "\n" +
                                                                "<tr>" + "\n" +
                                                                "<td class=\"links-wrapper\">" + "\n" +
                                                                    "<table class=\"mobile-center-block\" align=\"right\">" + "\n" +
                                                                        "<tr class=\"links-container\">" + "\n" +
                                                                            "<td align=\"right\" class=\"social-container\">" + "\n" +
                                                                                "<a href = \"https://www.facebook.com/suntrust\" target=\"_blank\">" + "\n" +
                                                                                    "<img src=\"" + FACEBOOK_LOGO_URL + "\" width=\"10\" height=\"21\" alt=\"Facebook\" style=\"display: block\" border=\"0\"/>" + "\n" +
                                                                                "</a>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                            "<td align=\"right\" style=\"padding-left:12px\" class=\"social-container\">" + "\n" +
                                                                                "<a href=\"https://twitter.com/SunTrust\" target=\"_blank\">" + "\n" +
                                                                                    "<img src=\"" + TWITTER_LOGO_URL + "\" width=\"21\" height=\"21\" alt=\"Twitter\" style=\"display: block\" border=\"0\">" + "\n" +
                                                                                "</a>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                            "<td align=\"right\" style=\"padding-left:12px\" class=\"social-container\">" + "\n" +
                                                                                "<a href=\"https://www.youtube.com/user/LiveSolid\" target=\"_blank\">" + "\n" +
                                                                                    "<img src = \"" + YOUTUBE_LOGO_URL + "\"  width=\"26\" height=\"21\" alt=\"YouTube\" style=\"display: block\" border=\"0\"/>" + "\n" +
                                                                                "</a>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                            "<td align=\"right\" style=\"padding: 0 20px 0 12px\" class=\"social-container last-social-net\">" + "\n" +
                                                                                "<a href=\"https://www.linkedin.com/company/suntrust-bank\" target=\"_blank\">" + "\n" +
                                                                                    "<img src = \"" + LINKEDIN_LOGO_URL + "\" width=\"21\" height=\"21\" alt=\"LinkedIn\" style=\"display: block\" border=\"0\"/>" + "\n" +
                                                                                "</a>" + "\n" +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "</table>" + "\n" +
                                                                "</td>" + "\n" +
                                                            "</tr>" + "\n" +
                                                            "<tr>" + "\n" +
                                                                "<td>" + "\n" +
                                                                    "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">" + "\n" +
                                                                        "<tr>" + "\n" +
                                                                            "<td style=\"font-size:10.5px; line-height:16px;\" class=\"disclaimer-container\">" + "\n" +
                                                                                    DISCLAIMER_CONTENT_AREA +
                                                                            "</td>" + "\n" +
                                                                        "</tr>" + "\n" +
                                                                    "</table>" + "\n" +
                                                                "</td>" + "\n" +
                                                            "</tr>" + "\n" +
                                                        "</table>" + "\n" +
                                                    "</td>" + "\n" +
                                                "</tr>" + "\n" +
                                            "</table>" + "\n" +
                                        "</td>" + "\n" +
                                    "</tr>" + "\n" +
                                "</tbody>" + "\n" +
                            "</table>" + "\n" +
                        "</td>" + "\n" +
                    "</tr>" + "\n" +
                "</table>" + "\n" +
            "</body>" + "\n" +
        "</html>";
            foreach (var section in contentID.Sections)
            {
                switch (section.ID)
                {
                    case EMAIL_TEMPLATE_SECTION:
                        html = GenerateSections(contentID.Sections, sectionsTypes.Where(s => s.Type == EMAIL_TEMPLATE_SECTION_TYPE).ToList(), html);

                        html = html.Replace(SUNTRUST_LOGO_URL, baseUrl + "/Content/images/email-template/SunTrust_Bank_logo.png")
                           .Replace(FACEBOOK_LOGO_URL, baseUrl + "/Content/images/email-template/facebook-black.png")
                           .Replace(TWITTER_LOGO_URL, baseUrl + "/Content/images/email-template/twitter-black.png")
                           .Replace(YOUTUBE_LOGO_URL, baseUrl + "/Content/images/email-template/youtube-black.png")
                           .Replace(LINKEDIN_LOGO_URL, baseUrl + "/Content/images/email-template/Linkedin-Black.png")
                           .Replace(CTA_CARET_URL, baseUrl + "/Content/images/email-template/right_pointing_angle_bracket.png");
                        break;
                }
            }
            return html;
        }

        private string GenerateSections(IList<SectionViewModel> sections, IList<SectionType> subSections, string html)
        {
            foreach (var section in sections)
            {
                var sectionType = subSections.Where(s => s.ID == section.ID).FirstOrDefault();

                if (sectionType != null)
                {
                    html = GenerateComponents(section, html);
                }
            }
            return html;
        }

        private string GenerateComponents(SectionViewModel section, string html)
        {
            StringBuilder compSB = new StringBuilder();
            var headerSolidImg = baseUrl + "/Content/images/email-template/headers/RobertoDkBlue_headerD.jpg";
            var headerSolidMobImg = baseUrl + "/Content/images/email-template/headers/RobertoDkBlue_headerM.jpg";
            var buttonLeftImg = baseUrl + "/Content/images/email-template/buttonPieces/dkBlueLeft.png";
            var buttonArrowImg = baseUrl + "/Content/images/email-template/buttonPieces/dkBlueArrow.gif";
            var buttonRightImg = baseUrl + "/Content/images/email-template/buttonPieces/dkBlueRight.png";
            var buttonBgColor = "#003B71";

            foreach (var c in section.Components)
            {
                if (c.TypeID == MAIN_IMAGE_COMP_ID)
                {
                    headerSolidImg = getTemplateImagePiece(HEADER, "D", Convert.ToString(c.Data["selectedOption"]["Header"]));
                    headerSolidMobImg = getTemplateImagePiece(HEADER, "M", Convert.ToString(c.Data["selectedOption"]["Header"]));
                    buttonLeftImg = getTemplateImagePiece(LEFT, "D", Convert.ToString(c.Data["selectedOption"]["PiecePrefix"]));
                    buttonArrowImg = getTemplateImagePiece(ARROW, "D", Convert.ToString(c.Data["selectedOption"]["PiecePrefix"]));
                    buttonRightImg = getTemplateImagePiece(RIGHT, "D", Convert.ToString(c.Data["selectedOption"]["PiecePrefix"]));
                    buttonBgColor = Convert.ToString(c.Data["selectedOption"]["BaseColor"]);
                }
                else if (c.TypeID == RIGHT_RAIL_COMP_ID)
                {
                    string activeContentType = Convert.ToString(c.Data["selectedOption"]["Name"]);
                    if (activeContentType == "CTA buttons") {
                        rightRailsCTA = true;
                    }           
                }
            }
                
            foreach (var c in section.Components)
            {

                //TODO: Get the component names from DB. The info should be filled.
                if (!c.Inactive)
                {
                    if (c.TypeID == MAIN_IMAGE_COMP_ID)
                    {
                        var useSolidColor = (string.IsNullOrEmpty(Convert.ToString(c.Data["UseSolidColor"])) || string.IsNullOrWhiteSpace(Convert.ToString(c.Data["UseSolidColor"]))) ? false : Boolean.Parse(Convert.ToString(c.Data["UseSolidColor"]));
                        var showCTAButton = (string.IsNullOrEmpty(Convert.ToString(c.Data["DisplayHeaderCta"])) || string.IsNullOrWhiteSpace(Convert.ToString(c.Data["DisplayHeaderCta"]))) ? false : Boolean.Parse(Convert.ToString(c.Data["DisplayHeaderCta"]));

                        var headerSolidColor = Convert.ToString(c.Data["HeaderColor"]);
                        var mainImageUrl = string.IsNullOrEmpty((Convert.ToString(c.Data["src"])).Replace("\\", "/")) || string.IsNullOrWhiteSpace((Convert.ToString(c.Data["src"])).Replace("\\", "/")) ? headerSolidImg : baseUrl + (Convert.ToString(c.Data["src"])).Replace("\\", "/");
                        var mainImageUrlMobile = string.IsNullOrEmpty((Convert.ToString(c.Data["src"])).Replace("\\", "/")) || string.IsNullOrWhiteSpace((Convert.ToString(c.Data["src"])).Replace("\\", "/")) ? headerSolidMobImg : baseUrl + (Convert.ToString(c.Data["src"])).Replace("\\", "/");
                        var headerOverlayCopy = Convert.ToString(c.Data["HeaderOverlayCopy"]);
                        var mainImageLinkUrl = (string.IsNullOrEmpty(Convert.ToString(c.Data["LearnMoreUrl"])) || string.IsNullOrWhiteSpace(Convert.ToString(c.Data["LearnMoreUrl"]))) ? string.Empty : "href=\"" + Convert.ToString(c.Data["LearnMoreUrl"]) + "\"";
                        var mainImageLinkVMLUrl = Convert.ToString(c.Data["LearnMoreUrl"]);
                        var headerCtaCopy = Convert.ToString(c.Data["LearnMoreText"]);
                        var applyNowTriangleUrl = baseUrl + "/Content/images/email-template/right-white-triangle.png";
                        var applyNowBackground = baseUrl + "/Content/images/email-template/apply-now-background-transparent.png";
                        var applyNowLeftTriangle = baseUrl + "/Content/images/email-template/apply-now-left-2-colors.png";
                        var applyNowRightTriangle = baseUrl + "/Content/images/email-template/apply-now-right-2-colors.png";

                        

                         var mainImageAlt = Convert.ToString(c.Data["AltText"]);

                        if (useSolidColor)
                        {
                            if (showCTAButton)
                            {
                                backgroundColorWithCTA = backgroundColorWithCTA.Replace(TEMPLATE_HEADER_IMG_URL, headerSolidImg)
                                    
                                    .Replace(TEMPLATE_BUTTON_LEFT_IMG_URL, buttonLeftImg)
                                    .Replace(TEMPLATE_BUTTON_BACKGROUND_COLOR, buttonBgColor)
                                    .Replace(TEMPLATE_BUTTON_ARROW_IMG_URL, buttonArrowImg)
                                    .Replace(TEMPLATE_BUTTON_RIGHT_IMG_URL, buttonRightImg)
                                    .Replace(HEADER_OVERLAY_COPY, headerOverlayCopy)
                                    .Replace(MAIN_IMAGE_LINK_URL, mainImageLinkUrl)
                                    .Replace(HEADER_CTA_COPY, headerCtaCopy);

                                backgroundColorWithCTAMobile = backgroundColorWithCTAMobile.Replace(TEMPLATE_HEADER_IMG_MOB_URL, headerSolidMobImg)
                                   .Replace(TEMPLATE_BUTTON_LEFT_IMG_URL, buttonLeftImg)
                                   .Replace(TEMPLATE_BUTTON_BACKGROUND_COLOR, buttonBgColor)
                                   .Replace(TEMPLATE_BUTTON_ARROW_IMG_URL, buttonArrowImg)
                                   .Replace(TEMPLATE_BUTTON_RIGHT_IMG_URL, buttonRightImg)
                                   .Replace(HEADER_OVERLAY_COPY, headerOverlayCopy)
                                   .Replace(MAIN_IMAGE_LINK_URL, mainImageLinkUrl)
                                   .Replace(HEADER_CTA_COPY, headerCtaCopy);

                                html = html.Replace(HEADER_CONTENT, backgroundColorWithCTA)
                                    .Replace(HEADER_CONTENT_MOBILE, backgroundColorWithCTAMobile);
                            }
                            else
                            {
                                backgroundColorNoCTA = backgroundColorNoCTA.Replace(MAIN_IMAGE_LINK_URL, mainImageLinkUrl)
                                    .Replace(HEADER_SOLID_COLOR_BK, headerSolidColor)
                                    .Replace(TEMPLATE_HEADER_IMG_URL, headerSolidImg)
                                    .Replace(MAIN_IMAGE_LINK_VML_URL, mainImageLinkVMLUrl)
                                    .Replace(TEMPLATE_BUTTON_BACKGROUND_COLOR, buttonBgColor)
                                    .Replace(HEADER_OVERLAY_COPY, headerOverlayCopy);

                                backgroundColorNoCTAMobile = backgroundColorNoCTAMobile.Replace(MAIN_IMAGE_LINK_URL, mainImageLinkUrl)
                                    .Replace(TEMPLATE_HEADER_IMG_MOB_URL, headerSolidMobImg)
                                    .Replace(MAIN_IMAGE_LINK_VML_URL, mainImageLinkVMLUrl)
                                    .Replace(HEADER_OVERLAY_COPY, headerOverlayCopy);

                                html = html.Replace(HEADER_CONTENT, backgroundColorNoCTA)
                                    .Replace(HEADER_CONTENT_MOBILE, backgroundColorNoCTAMobile);
                            }
                        } else if(showCTAButton)
                        {
                            headerImageWithCTA = headerImageWithCTA.Replace(MAIN_IMAGE_URL, mainImageUrl)
                                .Replace(TEMPLATE_BUTTON_BACKGROUND_COLOR, buttonBgColor)
                                .Replace(HEADER_CTA_COPY, headerCtaCopy)
                                .Replace(MAIN_IMAGE_LINK_URL, mainImageLinkUrl)
                                .Replace(TEMPLATE_BUTTON_LEFT_IMG_URL, buttonLeftImg)
                                .Replace(TEMPLATE_BUTTON_ARROW_IMG_URL, buttonArrowImg)
                                .Replace(TEMPLATE_BUTTON_RIGHT_IMG_URL, buttonRightImg)
                                .Replace(HEADER_OVERLAY_COPY, headerOverlayCopy);

                            headerImageWithCTAMobile = headerImageWithCTAMobile.Replace(MAIN_IMAGE_URL_MOBILE, mainImageUrlMobile)
                                 .Replace(HEADER_OVERLAY_COPY, headerOverlayCopy)
                                 .Replace(MAIN_IMAGE_LINK_URL, mainImageLinkUrl)
                                .Replace(HEADER_CTA_COPY, headerCtaCopy)
                                .Replace(TEMPLATE_BUTTON_LEFT_IMG_URL, buttonLeftImg)
                                .Replace(TEMPLATE_BUTTON_BACKGROUND_COLOR, buttonBgColor)
                                .Replace(TEMPLATE_BUTTON_ARROW_IMG_URL, buttonArrowImg)
                                .Replace(TEMPLATE_BUTTON_RIGHT_IMG_URL, buttonRightImg);

                            html = html.Replace(HEADER_CONTENT, headerImageWithCTA)
                                .Replace(HEADER_CONTENT_MOBILE, headerImageWithCTAMobile);
                        } else
                        {
                            headerImageNoCTA = headerImageNoCTA.Replace(MAIN_IMAGE_LINK_URL, mainImageLinkUrl)
                                .Replace(MAIN_IMAGE_URL, mainImageUrl)
                                .Replace(TEMPLATE_BUTTON_BACKGROUND_COLOR, buttonBgColor)
                                .Replace(MAIN_IMAGE_LINK_VML_URL, mainImageLinkVMLUrl)
                                .Replace(HEADER_OVERLAY_COPY, headerOverlayCopy);

                            headerImageNoCTAMobile = headerImageNoCTAMobile.Replace(MAIN_IMAGE_LINK_URL, mainImageLinkUrl)
                                .Replace(MAIN_IMAGE_URL_MOBILE, mainImageUrlMobile)
                                .Replace(MAIN_IMAGE_LINK_VML_URL, mainImageLinkVMLUrl)
                                .Replace(HEADER_OVERLAY_COPY, headerOverlayCopy);

                            html = html.Replace(HEADER_CONTENT, headerImageNoCTA)
                                .Replace(HEADER_CONTENT_MOBILE, headerImageNoCTAMobile);
                        }
                    }
                    else if (c.TypeID == EMAIL_HEADER_LINKS_COMP_ID)
                    {
                        var activeLinks = Convert.ToString(c.Data["NumberOfHeaderLinks"]);

                        switch (activeLinks)
                        {
                            case "0":
                                html = html.Replace(FIRST_HEADER_LINK, string.Empty)
                                    .Replace(SECOND_HEADER_LINK, string.Empty)
                                    .Replace(THIRD_HEADER_LINK, string.Empty)
                                    .Replace(FIRST_HEADER_LINK_SEPARATOR, string.Empty)
                                    .Replace(SECOND_HEADER_LINK_SEPARATOR, string.Empty);
                                break;
                            case "1":
                                firstLink = firstLink.Replace(FIRST_HEADER_LINK_URL, Convert.ToString(c.Data["FirstLink"]["url"]))
                                    .Replace(FIRST_HEADER_LINK_TEXT, Convert.ToString(c.Data["FirstLink"]["text"]));

                                html = html.Replace(FIRST_HEADER_LINK, firstLink)
                                   .Replace(SECOND_HEADER_LINK, string.Empty)
                                   .Replace(THIRD_HEADER_LINK, string.Empty)
                                   .Replace(FIRST_HEADER_LINK_SEPARATOR, string.Empty)
                                   .Replace(SECOND_HEADER_LINK_SEPARATOR, string.Empty);
                                break;
                            case "2":
                                firstLink = firstLink.Replace(FIRST_HEADER_LINK_URL, Convert.ToString(c.Data["FirstLink"]["url"]))
                                    .Replace(FIRST_HEADER_LINK_TEXT, Convert.ToString(c.Data["FirstLink"]["text"]));

                                secondLink = secondLink.Replace(SECOND_HEADER_LINK_URL, Convert.ToString(c.Data["SecondLink"]["url"]))
                                    .Replace(SECOND_HEADER_LINK_TEXT, Convert.ToString(c.Data["SecondLink"]["text"]));

                                html = html.Replace(FIRST_HEADER_LINK, firstLink)
                                   .Replace(SECOND_HEADER_LINK, secondLink)
                                   .Replace(THIRD_HEADER_LINK, string.Empty)
                                   .Replace(FIRST_HEADER_LINK_SEPARATOR, linksSeparator)
                                   .Replace(SECOND_HEADER_LINK_SEPARATOR, string.Empty);
                                break;
                            case "3":
                                firstLink = firstLink.Replace(FIRST_HEADER_LINK_URL, Convert.ToString(c.Data["FirstLink"]["url"]))
                                    .Replace(FIRST_HEADER_LINK_TEXT, Convert.ToString(c.Data["FirstLink"]["text"]));

                                secondLink = secondLink.Replace(SECOND_HEADER_LINK_URL, Convert.ToString(c.Data["SecondLink"]["url"]))
                                    .Replace(SECOND_HEADER_LINK_TEXT, Convert.ToString(c.Data["SecondLink"]["text"]));

                                thirdLink = thirdLink.Replace(THIRD_HEADER_LINK_URL, Convert.ToString(c.Data["ThirdLink"]["url"]))
                                    .Replace(THIRD_HEADER_LINK_TEXT, Convert.ToString(c.Data["ThirdLink"]["text"]));

                                html = html.Replace(FIRST_HEADER_LINK, firstLink)
                                   .Replace(SECOND_HEADER_LINK, secondLink)
                                   .Replace(THIRD_HEADER_LINK, thirdLink)
                                   .Replace(FIRST_HEADER_LINK_SEPARATOR, linksSeparator)
                                   .Replace(SECOND_HEADER_LINK_SEPARATOR, linksSeparator);
                                break;
                            default:
                                html = html.Replace(FIRST_HEADER_LINK, string.Empty)
                                    .Replace(SECOND_HEADER_LINK, string.Empty)
                                    .Replace(THIRD_HEADER_LINK, string.Empty)
                                    .Replace(FIRST_HEADER_LINK_SEPARATOR, string.Empty)
                                    .Replace(SECOND_HEADER_LINK_SEPARATOR, string.Empty);
                                break;
                        }
                        html = html.Replace(MAIN_IMAGE_URL, (baseUrl + (Convert.ToString(c.Data["src"])).Replace("\\", "/")))
                            .Replace(MAIN_IMAGE_ALT, Convert.ToString(c.Data["AltText"]))
                            .Replace(MAIN_IMAGE_LINK_URL, Convert.ToString(c.Data["LinkUrl"]));
                    }
                    else if (c.TypeID == DETAILS_COMP_ID)
                    {
                        string applyNowUrl = Convert.ToString(c.Data["Apply"]["url"]);
                        string applyNowButonText = (string.IsNullOrEmpty(Convert.ToString(c.Data["Apply"]["buttonText"])) || string.IsNullOrWhiteSpace(Convert.ToString(c.Data["Apply"]["buttonText"]))) ? "Apply Now" : Convert.ToString(c.Data["Apply"]["buttonText"]);
                        string applyNowHorizontalPosition = (string.IsNullOrEmpty(Convert.ToString(c.Data["Apply"]["DesktopAlignment"]["Value"])) || string.IsNullOrWhiteSpace(Convert.ToString(c.Data["Apply"]["DesktopAlignment"]["Value"]))) ? "NULL" : Convert.ToString(c.Data["Apply"]["DesktopAlignment"]["Value"]);
                        string ctaReplaced = "<span class=\"apply-"+ applyNowHorizontalPosition +"\">[[CTA-APPLY-NOW]]</span>";
                        string ctaToReplace = "<div class=\"apply-now\" style=\"display:inline-block;text-align:center;margin:10px 0;vertical-align:middle\">[[CTA-APPLY-NOW]]</div>";


                        if (!string.IsNullOrEmpty(applyNowUrl))
                        {
                            
                            applyNowCTA = applyNowCTA.Replace(APPLY_NOW_CTA_URL, applyNowUrl)
                                .Replace(APPLY_NOW_BACKGROUND_URL, baseUrl + "/Content/images/email-template/apply-now-background.png")
                                .Replace(APPLY_NOW_TRIANGLE_URL, baseUrl + "/Content/images/email-template/right-white-triangle.png")
                                .Replace(APPLY_NOW_BACKGROUND_URL, baseUrl + "/Content/images/email-template/apply-now-background.png")
                                .Replace(APPLY_NOW_BUTTON_TEXT, applyNowButonText)
                                .Replace(APPLY_NOW_BUTTON_WIDTH, Convert.ToString(c.Data["Apply"]["width"]))
                                .Replace(LEFT_APPLY_NOW_TRIANGLE, baseUrl + "/Content/images/email-template/apply-now-left-2-colors.png")
                                .Replace(RIGHT_APPLY_NOW_TRIANGLE, baseUrl + "/Content/images/email-template/apply-now-right-2-colors.png")
                                .Replace(TEMPLATE_BUTTON_LEFT_IMG_URL, buttonLeftImg)
                                .Replace(TEMPLATE_BUTTON_BACKGROUND_COLOR, buttonBgColor)
                                .Replace(TEMPLATE_BUTTON_ARROW_IMG_URL, buttonArrowImg)
                                .Replace(TEMPLATE_BUTTON_RIGHT_IMG_URL, buttonRightImg);

                            if (applyNowHorizontalPosition != "NULL" && applyNowHorizontalPosition != "default")
                            {
                                ctaToReplace = "<div class=\"apply-now\" style=\"margin:10px 0;text-align:" + applyNowHorizontalPosition + ";display:block;\">[[CTA-APPLY-NOW]]</div>";
                            }
                            html = html.Replace(MAIN_CONTENT_AREA, Convert.ToString(c.Data["Description"]))
                               .Replace(ctaReplaced, ctaToReplace)
                               .Replace("<sup", "<sup class=\"sup\"");

                            html = html.Replace(MAIN_CONTENT_AREA, Convert.ToString(c.Data["Description"]))
                                .Replace("[[CTA-APPLY-NOW]]", applyNowCTA)
                                .Replace("<sup", "<sup class=\"sup\"");
                        } else
                        {
                            html = html.Replace(MAIN_CONTENT_AREA, Convert.ToString(c.Data["Description"]))
                                .Replace("apply-now", "apply-now hidden")
                                .Replace("<sup", "<sup class=\"sup\"");
                        }
                        
                    }

                    else if (c.TypeID == RIGHT_RAIL_COMP_ID)
                    {
                        string activeContentType = Convert.ToString(c.Data["selectedOption"]["Name"]);
                        var sectionActive = false;
                        switch (activeContentType)
                        {
                            case "Text content":
                                sectionActive = !string.IsNullOrEmpty(Convert.ToString(c.Data["TextEditor"]["Description"])) && !string.IsNullOrWhiteSpace(Convert.ToString(c.Data["TextEditor"]["Description"]));
                                if (sectionActive)
                                {
                                    rightRailTextContent = rightRailTextContent.Replace(RIGHT_RAIL_TEXT_CONTENT, Convert.ToString(c.Data["TextEditor"]["Description"]))
                                        .Replace("<sup", "<sup class=\"sup\"");
                                    mainContentTableRightContent = mainContentTableRightContent.Replace(RIGHT_RAIL_ACTIVE_CONTENT, rightRailTextContent);
                                    html = html.Replace(RIGHT_CONTENT, rightContent)
                                        .Replace(RIGHT_RAIL_CONTENT_AREA, mainContentTableRightContent)
                                        .Replace(MAIN_CONTENT_TABLE_CLASS, "right-content-active")
                                        .Replace(MAIN_CONTENT_TABLE_STYLE, "style=\"border-right: 1px solid #DCD9DC;\"")
                                        .Replace(LEFT_CONTENT_COLUMN_WIDTH, "65%");
                                }
                                else
                                {                                    
                                    html = html.Replace(RIGHT_CONTENT, string.Empty)
                                    .Replace(MAIN_CONTENT_TABLE_CLASS, string.Empty)
                                    .Replace(MAIN_CONTENT_TABLE_STYLE, string.Empty)
                                    .Replace(LEFT_CONTENT_COLUMN_WIDTH, "100%");
                                }
                                break;

                            case "Image content":
                                var textPartActive = !string.IsNullOrEmpty(Convert.ToString(c.Data["Image"]["Description"])) && !string.IsNullOrWhiteSpace(Convert.ToString(c.Data["Image"]["Description"]));
                                var imagePartActive = !string.IsNullOrEmpty(Convert.ToString(c.Data["Image"]["src"])) && !string.IsNullOrWhiteSpace(Convert.ToString(c.Data["Image"]["src"]));
                                sectionActive = textPartActive || imagePartActive;
                                if (sectionActive)
                                {
                                    
                                    rightRailImageContent = rightRailImageContent.Replace(RIGHT_RAIL_IMAGE_CONTENT_SRC, (baseUrl + Convert.ToString(c.Data["Image"]["src"])).Replace("\\", "/"))
                                    .Replace(RIGHT_RAIL_IMAGE_CONTENT_ALT, Convert.ToString(c.Data["Image"]["AltText"]));
                                    
                                    if (textPartActive)
                                    {
                                        rightRailImageTextContent = rightRailImageTextContent.Replace(RIGHT_RAIL_IMAGE_TEXT_CONTENT, Convert.ToString(c.Data["Image"]["Description"]))
                                            .Replace("<sup", "<sup class=\"sup\"");
                                    } else
                                    {
                                        rightRailImageTextContent = string.Empty;
                                    }

                                    mainContentTableRightContent = mainContentTableRightContent.Replace(RIGHT_RAIL_ACTIVE_CONTENT, rightRailImageContent + rightRailImageTextContent);
                                    html = html.Replace(RIGHT_CONTENT, rightContent)
                                        .Replace(RIGHT_RAIL_CONTENT_AREA, mainContentTableRightContent)
                                        .Replace(MAIN_CONTENT_TABLE_CLASS, "right-content-active")
                                        .Replace(MAIN_CONTENT_TABLE_STYLE, "style=\"border-right: 1px solid #DCD9DC;\"")
                                        .Replace(LEFT_CONTENT_COLUMN_WIDTH, "65%");
                                }
                                else
                                {
                                    html = html.Replace(RIGHT_CONTENT, string.Empty)
                                    .Replace(MAIN_CONTENT_TABLE_CLASS, string.Empty)
                                        .Replace(MAIN_CONTENT_TABLE_STYLE, string.Empty)
                                    .Replace(LEFT_CONTENT_COLUMN_WIDTH, "100%");
                                }
                                break;

                            case "CTA buttons":
                                rightRailsCTA = true;
                                break;

                            default:
                                html = html.Replace(RIGHT_CONTENT, rightContent)
                                    .Replace(MAIN_CONTENT_TABLE_CLASS, string.Empty)
                                        .Replace(MAIN_CONTENT_TABLE_STYLE, string.Empty);
                                break;
                        }
                    }

                    else if (c.TypeID == EMAIL_CALL_TO_ACTION_COMP_ID)
                    {
                        var goOnlineActiveByContent = !(string.IsNullOrEmpty(Convert.ToString(c.Data["GoOnline"]["url"])) && string.IsNullOrWhiteSpace(Convert.ToString(c.Data["GoOnline"]["url"]))) && !(string.IsNullOrEmpty(Convert.ToString(c.Data["GoOnline"]["buttonText"])) && string.IsNullOrWhiteSpace(Convert.ToString(c.Data["GoOnline"]["buttonText"])));

                        var callActiveByContent = !(string.IsNullOrEmpty(Convert.ToString(c.Data["Call"]["url"])) && string.IsNullOrWhiteSpace(Convert.ToString(c.Data["Call"]["url"])))
                                             && !(string.IsNullOrEmpty(Convert.ToString(c.Data["Call"]["buttonText"])) && string.IsNullOrWhiteSpace(Convert.ToString(c.Data["Call"]["buttonText"]))); ;

                        var visitActiveByContent = !(string.IsNullOrEmpty(Convert.ToString(c.Data["Visit"]["url"])) && string.IsNullOrWhiteSpace(Convert.ToString(c.Data["Visit"]["url"])))
                                             && !(string.IsNullOrEmpty(Convert.ToString(c.Data["Visit"]["buttonText"])) && string.IsNullOrWhiteSpace(Convert.ToString(c.Data["Visit"]["buttonText"]))); ;

                        var goOnlineInactive = Boolean.Parse(Convert.ToString(c.Data["GoOnline"]["Inactive"])) || !goOnlineActiveByContent;
                        var callInactive = Boolean.Parse(Convert.ToString(c.Data["Call"]["Inactive"])) || !callActiveByContent;
                        var visitInactive = Boolean.Parse(Convert.ToString(c.Data["Visit"]["Inactive"])) || !visitActiveByContent;
                        int ctaCount = 0;

                        if (!goOnlineInactive)
                        {
                            bottomCTAArea = bottomCTAArea.Replace(GO_ONLINE_CTA_HREF, Convert.ToString(c.Data["GoOnline"]["url"]))
                                .Replace(GO_ONLINE_CTA_BUTTON_TEXT, Convert.ToString(c.Data["GoOnline"]["buttonText"]))
                                .Replace(GO_ONLINE_CTA_BUTTON_LINK_TEXT, Convert.ToString(c.Data["GoOnline"]["LinkText"]));

                            RightCTAArea = RightCTAArea.Replace(GO_ONLINE_CTA_HREF, Convert.ToString(c.Data["GoOnline"]["url"]))
                                .Replace(GO_ONLINE_CTA_BUTTON_TEXT, Convert.ToString(c.Data["GoOnline"]["buttonText"]))
                                .Replace(GO_ONLINE_CTA_BUTTON_LINK_TEXT, Convert.ToString(c.Data["GoOnline"]["LinkText"]))
                                .Replace(GO_ONLINE_CTA_ADDITIONAL_ROW, betweenCtas);

                            ctaCount++;
                        }
                        else
                        {
                            bottomCTAArea = bottomCTAArea.Replace(bottomGoOnlineCTADesktop, string.Empty)
                                .Replace(bottomGoOnlineCTAMobile, string.Empty);
                            RightCTAArea = RightCTAArea.Replace(bottomGoOnlineCTADesktop, string.Empty)
                                .Replace(bottomGoOnlineCTAMobile, string.Empty)
                                .Replace(GO_ONLINE_CTA_ADDITIONAL_ROW, string.Empty);
                        }
                        if (!callInactive)
                        {
                            bottomCTAArea = bottomCTAArea.Replace(CALL_CTA_HREF, Convert.ToString(c.Data["Call"]["url"]))
                                .Replace(CALL_CTA_BUTTON_TEXT, Convert.ToString(c.Data["Call"]["buttonText"]))
                                .Replace(CALL_CTA_BUTTON_LINK_TEXT, Convert.ToString(c.Data["Call"]["LinkText"]));

                            RightCTAArea = RightCTAArea.Replace(CALL_CTA_HREF, Convert.ToString(c.Data["Call"]["url"]))
                                .Replace(CALL_CTA_BUTTON_TEXT, Convert.ToString(c.Data["Call"]["buttonText"]))
                                .Replace(CALL_CTA_BUTTON_LINK_TEXT, Convert.ToString(c.Data["Call"]["LinkText"]))
                                .Replace(CALL_CTA__ADDITIONAL_ROW, betweenCtas);
                            ctaCount++;
                        }
                        else
                        {
                            bottomCTAArea = bottomCTAArea.Replace(bottomCallCTADesktop, string.Empty)
                                .Replace(bottomCallCTAMobile, string.Empty);
                            RightCTAArea = RightCTAArea.Replace(bottomCallCTADesktop, string.Empty)
                                .Replace(bottomCallCTAMobile, string.Empty)
                                .Replace(CALL_CTA__ADDITIONAL_ROW, string.Empty);
                        }
                        if (!visitInactive)
                        {
                            bottomCTAArea = bottomCTAArea.Replace(VISIT_CTA_HREF, Convert.ToString(c.Data["Visit"]["url"]))
                                .Replace(VISIT_CTA_BUTTON_TEXT, Convert.ToString(c.Data["Visit"]["buttonText"]))
                                .Replace(VISIT_CTA_BUTTON_LINK_TEXT, Convert.ToString(c.Data["Visit"]["LinkText"]));

                            RightCTAArea = RightCTAArea.Replace(VISIT_CTA_HREF, Convert.ToString(c.Data["Visit"]["url"]))
                                .Replace(VISIT_CTA_BUTTON_TEXT, Convert.ToString(c.Data["Visit"]["buttonText"]))
                                .Replace(VISIT_CTA_BUTTON_LINK_TEXT, Convert.ToString(c.Data["Visit"]["LinkText"]));
                            ctaCount++;
                        }
                        else
                        {
                            bottomCTAArea = bottomCTAArea.Replace(bottomVisitCTADesktop, string.Empty)
                                .Replace(bottomVisitCTAMobile, string.Empty);
                            RightCTAArea = RightCTAArea.Replace(bottomVisitCTADesktop, string.Empty)
                                .Replace(bottomVisitCTAMobile, string.Empty);
                        }

                        if (ctaCount == 0)
                        {
                            bottomCTAArea = string.Empty;
                        } else
                        {
                            switch (ctaCount)
                            {
                                case 1:
                                    bottomCTAArea = bottomCTAArea.Replace(BOTTOM_CTA_COUNT_CLASS, "one-item")
                                        .Replace(GO_ONLINE_CTA_RIGHT_MARGIN, string.Empty)
                                        .Replace(CALL_CTA_RIGHT_MARGIN, string.Empty)
                                        .Replace(GO_ONLINE_CTA_RECT_WIDTH, "480px;")
                                        .Replace(CALL_CTA_RECT_WIDTH, "480px;")
                                        .Replace(VISIT_CTA_RECT_WIDTH, "480px;");

                                    break;
                                case 2:
                                    bottomCTAArea = bottomCTAArea.Replace(BOTTOM_CTA_COUNT_CLASS, "two-items")
                                        .Replace(GO_ONLINE_CTA_RECT_WIDTH, "240px;")
                                        .Replace(CALL_CTA_RECT_WIDTH, "240px;")
                                        .Replace(VISIT_CTA_RECT_WIDTH, "240px;");
                                    if (goOnlineInactive)
                                    {
                                        bottomCTAArea = bottomCTAArea.Replace(GO_ONLINE_CTA_RIGHT_MARGIN, "margin-right:10px")
                                        .Replace(CALL_CTA_RIGHT_MARGIN, string.Empty);
                                    } else
                                    {
                                        bottomCTAArea = bottomCTAArea.Replace(CALL_CTA_RIGHT_MARGIN, "margin-right:10px")
                                            .Replace(GO_ONLINE_CTA_RIGHT_MARGIN, string.Empty);
                                    }
                                    
                                    break;
                                case 3:
                                    bottomCTAArea = bottomCTAArea.Replace(BOTTOM_CTA_COUNT_CLASS, "three-items")
                                        .Replace(GO_ONLINE_CTA_RIGHT_MARGIN, "margin-right:9px")
                                        .Replace(CALL_CTA_RIGHT_MARGIN, "margin-right:9px")
                                        .Replace(GO_ONLINE_CTA_RECT_WIDTH, "150px;")
                                        .Replace(CALL_CTA_RECT_WIDTH, "150px;")
                                        .Replace(VISIT_CTA_RECT_WIDTH, "150px;");
                                    break;
                                default:
                                    bottomCTAArea = bottomCTAArea.Replace(bottomVisitCTADesktop, string.Empty)
                                        .Replace(bottomVisitCTAMobile, string.Empty);
                                    break;
                            }
                        }
                        if (rightRailsCTA)
                        {
                            if(ctaCount > 0)
                            {
                                RightCTAArea = RightCTAArea.Replace(GO_ONLINE_CTA_RECT_WIDTH, "170px;")
                                       .Replace(CALL_CTA_RECT_WIDTH, "170px;")
                                       .Replace(VISIT_CTA_RECT_WIDTH, "170px;");
                                mainContentTableRightContent = mainContentTableRightContent.Replace(RIGHT_RAIL_ACTIVE_CONTENT, RightCTAArea);
                                html = html.Replace(RIGHT_CONTENT, rightContent)
                                    .Replace(GO_ONLINE_BORDER_STYLE, string.Empty)
                                    .Replace(CALL_BORDER_STYLE, string.Empty)
                                    .Replace(VISIT_BORDER_STYLE, string.Empty)
                                    .Replace(RIGHT_RAIL_CONTENT_AREA, mainContentTableRightContent)
                                    .Replace(MAIN_CONTENT_TABLE_CLASS, "right-content-active")
                                    .Replace(MAIN_CONTENT_TABLE_STYLE, "style=\"border-right: 1px solid #DCD9DC;\"")
                                    .Replace(LEFT_CONTENT_COLUMN_WIDTH, "65%");
                            } else
                            {
                                html = html.Replace(RIGHT_CONTENT, string.Empty)
                                    .Replace(MAIN_CONTENT_TABLE_CLASS, string.Empty)
                                    .Replace(MAIN_CONTENT_TABLE_STYLE, string.Empty)
                                    .Replace(LEFT_CONTENT_COLUMN_WIDTH, "100%")
                                    .Replace(GO_ONLINE_BORDER_STYLE, borderGoOnline)
                                    .Replace(CALL_BORDER_STYLE, borderCall)
                                    .Replace(VISIT_BORDER_STYLE, borderVisit);
                            }
                            html = html.Replace(BOTTOM_CTA_AREA_CONTENT, string.Empty);

                        }
                        else
                        {
                            if (ctaCount > 0)
                            {
                                html = html.Replace(BOTTOM_CTA_AREA_CONTENT, bottomCTAArea)
                                    .Replace(GO_ONLINE_BORDER_STYLE, borderGoOnline)
                                    .Replace(CALL_BORDER_STYLE, borderCall)
                                    .Replace(VISIT_BORDER_STYLE, borderVisit);
                            } else
                            {
                                html = html.Replace(BOTTOM_CTA_AREA_CONTENT, string.Empty)
                                    .Replace(GO_ONLINE_BORDER_STYLE, string.Empty)
                                    .Replace(CALL_BORDER_STYLE, string.Empty)
                                    .Replace(VISIT_BORDER_STYLE, string.Empty);
                            }                            
                        }
                    }
                    else if (c.TypeID == DISCLAIMER_COMP_ID)
                    {
                        html = html.Replace(DISCLAIMER_CONTENT_AREA, Convert.ToString(c.Data["Description"]));
                    }
                }
                else if (c.TypeID == RIGHT_RAIL_COMP_ID)
                {
                    html = html.Replace(RIGHT_CONTENT, string.Empty)
                        .Replace(MAIN_CONTENT_TABLE_CLASS, string.Empty)
                        .Replace(MAIN_CONTENT_TABLE_STYLE, string.Empty)
                        .Replace(LEFT_CONTENT_COLUMN_WIDTH, "100%");
                }
            }

            return html;
        }

        private string EncodeURL(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                return url.Replace("&", "&amp;");
            }

            return string.Empty;
        }

        private string translateCTAName(string componentValue)
        {
            if (componentValue == "NAC")
            {
                return "goToNAC";
            }
            else if (componentValue == "CustomURL")
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

            return componentValue;
        }

        private string translateCTAType(string componentValue)
        {
            if (componentValue == "NAC" || componentValue == "gotoOLB-Statement" || componentValue == "gotoOLB-BillPay" || componentValue == "goToOLBEmailAddress" || componentValue == "goToOLBMobileNumber" || componentValue == "CustomURL")
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
            else if (componentValue == "CustomURL-Click")
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
        private string getTemplateImagePiece(string pieceRequired, string device, string prefix)
        {
            switch (pieceRequired)
            {
                case HEADER:
                    return baseUrl + "/Content/images/email-template/headers/" + prefix + device + ".jpg";
                    //if (device == "D")
                    //    return "http://staging.somnio.com/somnio/somnio1002/stqa//Content/images/email-template/headers/RobertoDkBlue_headerD.jpg";
                    //else
                    //    return "http://staging.somnio.com/somnio/somnio1002/stqa//Content/images/email-template/headers/RobertoDkBlue_headerM.jpg";
                case ARROW:
                    return baseUrl + "/Content/images/email-template/buttonPieces/" + prefix + pieceRequired + ".gif";
                //return "http://staging.somnio.com/somnio/somnio1002/stqa//Content/images/email-template/buttonPieces/dkBlueArrow.gif";
                case LEFT:
                    return baseUrl + "/Content/images/email-template/buttonPieces/" + prefix + pieceRequired + ".png";
                    //return "http://staging.somnio.com/somnio/somnio1002/stqa//Content/images/email-template/buttonPieces/dkBlueLeft.png";
                case RIGHT:
                    return baseUrl + "/Content/images/email-template/buttonPieces/" + prefix + pieceRequired + ".png";
                    //return "http://staging.somnio.com/somnio/somnio1002/stqa//Content/images/email-template/buttonPieces/dkBlueRight.png";
            }

            return "";
        }
    }
}

