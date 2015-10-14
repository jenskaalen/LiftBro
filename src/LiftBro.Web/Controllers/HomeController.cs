using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LiftBro.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //TODO: check if the user has any registered programs
            using (var db = new LiftBroContext())
            {
                if (!db.UserPrograms.Any(program =>
                    program.User.Username == User.Identity.Name))
                {
                    return Redirect("/Program/NewUser");
                }
            }

            return View("Profile");
        }

        public ActionResult Weight()
        {
            //TODO: check if the user has any registered programs
            using (var db = new LiftBroContext())
            {
                if (!db.UserPrograms.Any(program =>
                    program.User.Username == User.Identity.Name))
                {
                    return Redirect("/Program/NewUser");
                }
            }

            return View();
        }


    }
}
