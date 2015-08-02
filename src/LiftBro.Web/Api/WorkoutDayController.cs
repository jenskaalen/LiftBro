using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiftBro.Model;
using LiftBro.Web.Models;
using LiftBro.Web.Models.Requests;

namespace LiftBro.Web.Api
{
    public class WorkoutDayController : ApiController
    {
        [HttpPost]
        public void UpdateExercise(WorkoutDayExerciseChange workoutDayExerciseChange)
        {
            using (var db = new LiftBroContext())
            {
                ChangeModifier modifier = workoutDayExerciseChange.ChangeModifier;
                Exercise exercise = workoutDayExerciseChange.Exercise;

                switch (modifier)
                {
                    case ChangeModifier.Add:
                        Guid workoutDayId = workoutDayExerciseChange.WorkoutDay.Id;
                        WorkoutDay day = db.WorkoutDays
                            .Include("Exercises")
                            .FirstOrDefault(workoutDay => workoutDay.Id == workoutDayId);

                        day.Exercises.Add(new WorkoutExercise()
                        {
                            Id = Guid.NewGuid(),
                            Exercise = exercise,
                            Order = workoutDayExerciseChange.WorkoutDay.Exercises.Count + 1
                        });

                        break;

                    case ChangeModifier.Delete:
                        throw new NotImplementedException();
                        //var exerciseToDelete =
                        //    workoutDay.Exercises.FirstOrDefault(
                        //        workoutExercise => workoutExercise.Exercise.Id == exercise.Id);

                        //db.WorkoutExercises.FirstOrDefault(workoutExercise => workoutExercise.Exercise.Id == exercise.Id 
                        //    && workoutExercise.)
                    break;
                        case ChangeModifier.Update:
                        throw new NotImplementedException();
                }

                db.SaveChanges();
            }
        }

        //[HttpPost]
        //public void UpdateSets()
    }
}
