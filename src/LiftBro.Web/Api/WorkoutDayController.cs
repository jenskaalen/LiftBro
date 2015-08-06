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
    public class WorkoutDayController : ApiController
    {
        [HttpPost]
        public void UpdateExercise(WorkoutDayExerciseChange workoutDayExerciseChange)
        {
            using (var db = new LiftBroContext())
            {
                ChangeModifier modifier = workoutDayExerciseChange.Modifier;
                WorkoutExercise workoutExercise = db.WorkoutExercises.Find(workoutDayExerciseChange.Exercise.Id);
                WorkoutDay sourceWorkoutDay = db.WorkoutDays.FirstOrDefault(day => day.Id == workoutDayExerciseChange.WorkoutDay.Id);

                switch (modifier)
                {
                    case ChangeModifier.Add:
                        Exercise exercise = db.Exercises.Find(workoutDayExerciseChange.Exercise.Id);

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
                        //TODO: this should be solved in database by making Exercise a non-null field
                        //if (exercise == null)
                        //    sourceWorkoutDay.Exercises.RemoveAll(workoutExercise => workoutExercise.Exercise == null);
                        //else
                        //    sourceWorkoutDay.Exercises.RemoveAll(workoutExercise => workoutExercise.Exercise.Id == exercise.Id);

                        db.WorkoutExercises.Remove(workoutExercise);

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
    }
}
