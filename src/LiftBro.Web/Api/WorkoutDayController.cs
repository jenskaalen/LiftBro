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
                        db.WorkoutExercises.Remove(workoutExercise);
                    break;
                        case ChangeModifier.Update:
                        throw new NotImplementedException();
                }

                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<CompletedWorkoutDay> GetCompletedWorkouts(int take, int skip)
        {
            using (var db = new LiftBroContext())
            {
                //TODO: use authenticated user
                User currentUser = db.Users.First();
                return db.CompletedWorkouts.Where(day => day.User.Id == currentUser.Id)
                    .OrderByDescending(day => day.When)
                    .Skip(skip)
                    .Take(take)
                    .Include(day => day.Workout)
                    .ToList();
            }
        }
    }
}
