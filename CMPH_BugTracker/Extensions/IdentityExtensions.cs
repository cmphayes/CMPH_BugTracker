using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace CMPH_BugTracker.Extensions
{
    public static class IdentityExtensions
    {

        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            return (claim != null) ? claim.Value : "FirstName";
        }

        public static string GetLastName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("LastName");
            return (claim != null) ? claim.Value : "LastName";
        }

        public static string GetFullName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FullName");
            return (claim != null) ? claim.Value : "FullName";
        }

        public static string GetUserName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("UserName");
            return (claim != null) ? claim.Value : "UserName";
        }

        public static string GetDisplayName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("DisplayName");
            return (claim != null) ? claim.Value : "DisplayName";
        }

        public static string GetProfileImagePath(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ProfileImagePath");
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}