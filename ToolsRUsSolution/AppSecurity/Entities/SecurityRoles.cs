using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSecurity.Entities
{
    public static class SecurityRoles
    {
        public const string WebsiteAdmins = "WebsiteAdmins";
        public const string Receiving = "Receiving";
        public const string Rentals = "Rentals";
        public const string Purchasing = "Purchasing";
        public const string Sales = "Sales";
        public const string Staff = "Staff";
        public static List<string> DefaultSecurityRoles
        {
            get
            {
                List<string> value = new List<string>();
                value.Add(WebsiteAdmins);
                value.Add(Receiving);
                value.Add(Rentals);
                value.Add(Purchasing);
                value.Add(Sales);
                value.Add(Staff);
                return value;
            }
        }
    }

}
