using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace st1001.website.DAL.Security
{
    [Serializable]
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            if (Role == role)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
                {
                    return string.Format("{0} {1}", FirstName, LastName);
                }
                else if (!string.IsNullOrWhiteSpace(LastName))
                {
                    return LastName;
                } else if (!string.IsNullOrWhiteSpace(FirstName))
                {
                    return FirstName;
                }                    

                return Email;
            }
        }
        public string Role { get; set; }
        public string UserType { get; set; }
        //public string OfferType { get; set; }
    } 
}