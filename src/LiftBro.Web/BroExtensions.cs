using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace LiftBro.Web
{
    public static class BroExtensions
    {
        public static LiftBro.Model.User GetApplicationUser(this IPrincipal principal)
        {
            using (var db = new LiftBroContext())
            {
                return db.Users.FirstOrDefault(user => user.Username == principal.Identity.Name);
            }
        }
    }
}