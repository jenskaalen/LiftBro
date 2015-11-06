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

        [HttpPost]
        public Guid Log(WorkoutExerciseLog log)
        {
            using (var db = new LiftBroContext())
            {
                log.When = DateTime.Now;
                db.WorkoutExercises.Attach(log.WorkoutExercise);

                //update an already saved one
                if (!log.Id.Equals(Guid.Empty))
                {
                    db.WorkoutExerciseLogs.Attach(log);
                    db.Entry(log).State = EntityState.Modified;
                }
                else
                {
                    db.WorkoutExerciseLogs.Add(log);
                    log.Id = Guid.NewGuid();
                }

                db.SaveChanges();
                return log.Id;
            }
        }
    }
}
