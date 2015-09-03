using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LiftBro.Model;

namespace LiftBro.Web.Controllers
{
    [Authorize]
    public class ProgramController : Controller
    {
        // GET: Program
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Exercises()
        {
            return View();
        }

        public ActionResult NewUser()
        {
            //first we register the user 
            
            //make sure the user does not exist
            var user = User.GetApplicationUser();

            if (user == null)
            {
                //TODO: move this behind a repository or something
                using (var db = new LiftBroContext())
                {
                    //get the users name somehow
                    db.Users.Add(
                        new User()
                        {
                            Username = User.Identity.Name,
                            Name = User.Identity.Name
                        });

                    db.SaveChanges();
                }
            }

            return View();
        }

        public ActionResult Editor()
        {
            return View();
        }

        public ActionResult Creator()
        {
            return View();
        }
    }
}