using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiftBro.Model;

namespace LiftBro.Web.Api
{
    [Authorize]
    public class ExerciseController : ApiController
    {
        public IEnumerable<Exercise> GetAll()
        {
            using (var db = new LiftBroContext())
            {
                return db.Exercises.ToList();
            }
        }
    }
}
