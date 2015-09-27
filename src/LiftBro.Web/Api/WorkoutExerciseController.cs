using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiftBro.Model;
using LiftBro.Web.Models;
using LiftBro.Web.Models.Requests;

namespace LiftBro.Web.Api
{
    [Authorize]
    public class WorkoutExerciseController : ApiController
    {
        [HttpDelete]
        public void Delete(Guid id)
        {
            using (var db = new LiftBroContext())
            {
                var exercise = db.WorkoutExercises.Find(id);
                db.WorkoutExercises.Remove(exercise);
                db.SaveChanges();
            }
        }

        [HttpPut]
        public void Update(WorkoutExercise exercise)
        {
            using (var db = new LiftBroContext())
            {
                db.WorkoutExercises.Attach(exercise);
                db.ChangeTracker.Entries<WorkoutExercise>().First(e => e.Entity == exercise)
                                                      .State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void Create(CreateWorkoutExercise update)
        {
            using (var db = new LiftBroContext())
            {
                var workoutDay = db.WorkoutDays
                    .Include(day => day.Exercises).FirstOrDefault(day => day.Id == update.WorkoutDay.Id);
                    //db.WorkoutDays.Attach(update.WorkoutDay);
                db.Exercises.Attach(update.Exercise.Exercise);
                workoutDay.Exercises.Add(update.Exercise);
                db.SaveChanges();
            }
        }
    }
}
