using System;
using System.Web;
using System.Web.Configuration;
using System.Security.Cryptography;
using System.Text;
using st1001.website.DAL.Security;
using System.Security.Principal;
using st1001.website.Helpers;

namespace st1001.website.Models
{
    public class ModelHelper
    {
        public static string cnnString = Convert.ToString(WebConfigurationManager.ConnectionStrings["st1001Entities"]);
        
        public static string EncodePasswordToBase64(string password)
        {  byte[] bytes   = Encoding.Unicode.GetBytes(password);
           byte[] inArray = HashAlgorithm.Create("SHA1").ComputeHash(bytes);
           return Convert.ToBase64String(inArray);
        }

        public static IPrincipal GetCurrentUser(){
            return HttpContext.Current.User is CustomPrincipal
                ? (st1001.website.DAL.Security.CustomPrincipal)HttpContext.Current.User
                : new CustomPrincipal(HttpContext.Current.User.Identity.Name);

        }

        public static string ConvertToDateTime(long unixDate)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date= start.AddMilliseconds(unixDate).ToLocalTime();
            return date.ToString("MMM dd, yyyy");
        }

        public static long GetCurrentTimeInUnix()
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            double diff = DateTime.UtcNow.Subtract(start).TotalMilliseconds;
            return (long)diff;
        }

        public static string GetPhotoUrl(string photo)
        {
            if (string.IsNullOrEmpty(photo))
            {
                return WebConfigurationManager.AppSettings["AbsoluteUrl"] +  WebConfigurationManager.AppSettings["DefaultPhoto"];
            }
            return WebConfigurationManager.AppSettings["AbsoluteUrl"] + WebConfigurationManager.AppSettings["PhotoRelativeUrl"] + photo;
        }

        public static string GetLinkUrl(string username)
        {
            return Convert.ToString(WebConfigurationManager.AppSettings["AbsoluteUrl"]) + "#" + username;
        }
                
        public static string GetRoleName(int roleId)
        {
            switch ((UserRolEnum)roleId)
            {
                case UserRolEnum.ADMINISTRATOR:
                    return "Administrator";
                case UserRolEnum.PROJECT_MANAGER:
                    return "Project Manager";
                case UserRolEnum.EDITOR:
                    return "Editor";
                case UserRolEnum.SUPER_ADMIN:
                    return "Super Admin";
                default:
                    return "Unknown";
            }
        }

        public static string GetUserTypeName(int roleId)
        {
            switch ((UserTypeEnum)roleId)
            {
                case UserTypeEnum.WEB_CAMPAIGN:
                    return "Web Campaign";
                case UserTypeEnum.EMAIL_CAMPAIGN:
                    return "Email Campaign";                
                default:
                    return "Unknown";
            }
        }
        public static string GetOfferTypeName(int userTypeId)
        {
            switch ((OfferTypeEnum)userTypeId)
            {
                case OfferTypeEnum.WEB_CAMPAIGN:
                    return "Web Campaign";
                case OfferTypeEnum.EMAIL_CAMPAIGN:
                    return "Email Campaign";
                default:
                    return "Unknown";
            }
        }
        public static int GetOfferTypeID(string offerTypeName)
        {
            switch (offerTypeName)
            {
                case "Web Campaign":
                    return (int)UserTypeEnum.WEB_CAMPAIGN;
                case "Email Campaign" :
                    return (int)UserTypeEnum.EMAIL_CAMPAIGN;
                default:
                    return (int)UserTypeEnum.UNKNOWN;
            }
        }
    }
}