using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiftBro.Model;
using LiftBro.Web.Models.Requests;

namespace LiftBro.Web.Api
{
    [Authorize]
    public class SetController : ApiController
    {
        [HttpDelete]
        public void Delete(Guid id)
        {
            using (var db = new LiftBroContext())
            {
                var set = db.Sets.Find(id);
                db.Sets.Remove(set);
                db.SaveChanges();
            }
        }

        [HttpPut]
        public void Update(Set set)
        {
            using (var db = new LiftBroContext())
            {
                db.Sets.Attach(set);
                db.ChangeTracker.Entries<Set>().First(e => e.Entity == set)
                                                      .State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void Create(CreateSet update)
        {
            using (var db = new LiftBroContext())
            {
                db.WorkoutExercises.Attach(update.Exercise);
                update.Exercise.Sets.Add(update.Set);
                db.SaveChanges();
            }
        }
    }
}
