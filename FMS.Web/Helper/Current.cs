using FMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMS.Web.Helper
{
    public class Current
    {
        private const string _CURRENTUSER = "CurrentUser";
        private const string _CURRENTMEMBERSHIP = "CurrentMembership";

        public static User User
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENTUSER] == null) return null;

                return (User)HttpContext.Current.Session[_CURRENTUSER];
            }
            set
            {
                HttpContext.Current.Session[_CURRENTUSER] = value;
            }
        }

        public static Member Membership
        {
            get
            {
                if (HttpContext.Current.Session[_CURRENTMEMBERSHIP] == null) return null;

                return (Member)HttpContext.Current.Session[_CURRENTMEMBERSHIP];
            }
            set
            {
                HttpContext.Current.Session[_CURRENTMEMBERSHIP] = value;
            }
        }
    }
}