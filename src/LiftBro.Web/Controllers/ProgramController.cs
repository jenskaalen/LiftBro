using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}