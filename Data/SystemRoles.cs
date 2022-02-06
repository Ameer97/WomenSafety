using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferencesManagement.Data
{
    public class SystemRoles
    {
        public const string User = "User";
        public const string Admin = "Admin";

        public static List<string> All()
        {
            return new List<string>
            {
                User,Admin
            };
        }
    }
}
