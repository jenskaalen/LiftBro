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

        [HttpPost]
        public void UpdateDay(WorkoutDayChange workoutDayChange)
        {
            using (var db = new LiftBroContext())
            {
                ChangeModifier modifier = workoutDayChange.Modifier;
                WorkoutDay workoutDay = workoutDayChange.WorkoutDay;
                //workoutDay.Id = Guid.NewGuid();
                Program workoutProgram = workoutDayChange.Program;

                //WorkoutDay sourceWorkoutDay = db.WorkoutDays.FirstOrDefault(day => day.Id == workoutDayChange.WorkoutDay.Id);

                switch (modifier)
                {
                    case ChangeModifier.Add:
                        db.Programs.Attach(workoutProgram);

                        db.Programs
                            .Include(program => program.WorkoutDays)
                            .FirstOrDefault(program1 => program1.Id == workoutProgram.Id)
                            .WorkoutDays.Add(workoutDay);

                        //Exercise exercise = db.Exercises.Find(workoutDayExerciseChange.Exercise.Id);

                        //Guid workoutDayId = workoutDayExerciseChange.WorkoutDay.Id;
                        //WorkoutDay day = db.WorkoutDays
                        //    .Include("Exercises")
                        //    .FirstOrDefault(workoutDay => workoutDay.Id == workoutDayId);

                        //day.Exercises.Add(new WorkoutExercise()
                        //{
                        //    Id = Guid.NewGuid(),
                        //    Exercise = exercise,
                        //    Order = workoutDayExerciseChange.WorkoutDay.Exercises.Count + 1
                        //});

                        break;

                    case ChangeModifier.Delete:
                        db.WorkoutDays.Attach(workoutDay);
                        db.WorkoutDays.Remove(workoutDay);
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
                return db.CompletedWorkouts.Where(day => day.User.Username == User.Identity.Name)
                    .OrderByDescending(day => day.When)
                    .Skip(skip)
                    .Take(take)
                    .Include(day => day.Workout)
                    .ToList();
            }
        }

        [HttpGet]
        public WorkoutDay GetNextWorkoutDay()
        {
            using (var db = new LiftBroContext())
            {
                //TODO: use authenticated user
                User currentUser = GetCurrentUser();

                UserProgram currentUserProgram = db.UserPrograms
                    .Include(userProgram => userProgram.NextWorkout.Exercises)
                    .Include(userProgram => userProgram.User)
                    .FirstOrDefault(program =>
                    program.User.Username == User.Identity.Name
                    && program.CurrentlyUsing);

                WorkoutDay nextWorkoutDay = currentUserProgram.NextWorkout;

                //NOTE: seems there are no workouts. This means there workouts in the program
                if (nextWorkoutDay != null)
                { 
                    foreach (var workoutExercise in nextWorkoutDay.Exercises)
                    {
                        workoutExercise.Sets =
                            db.WorkoutExercises
                            .Include(we => we.Exercise)
                            .Include(we => we.Sets)
                            .FirstOrDefault(we => we.Id == workoutExercise.Id).Sets;
                    }
                }

                return nextWorkoutDay;
            }
        }

        [HttpPost]
        public void CompleteWorkout(WorkoutDay workoutDay)
        {
            using (var db = new LiftBroContext())
            {
                //TODO: use authenticated user
                User currentUser = db.Users.First();

                UserProgram currentUserProgram = db.UserPrograms
                    .Include(userProgram => userProgram.Program.WorkoutDays)
                    .Include(userProgram => userProgram.User)
                    .FirstOrDefault(program =>
                    program.User.Username == User.Identity.Name
                    && program.CurrentlyUsing);

                int nextOrder = workoutDay.Order + 1;

                WorkoutDay nextWorkoutDay = currentUserProgram.Program.WorkoutDays.FirstOrDefault(day => day.Order == nextOrder);

                //Program currentProgram = db.Programs
                //    .Include(program => program.WorkoutDays)
                //    .FirstOrDefault(program =>
                //    program.Id == currentUserProgram.Program.Id);

                if (nextWorkoutDay == null)
                    nextWorkoutDay = currentUserProgram.Program.WorkoutDays.OrderBy(day => day.Order).First();

                var attached = db.WorkoutDays.Find(nextWorkoutDay.Id);
                //attached.CurrentWorkoutDay = true;

                currentUserProgram.NextWorkout = attached;
                //nextWorkoutDay.CurrentWorkoutDay = true;

                db.SaveChanges();
            }
        }

        private User GetCurrentUser()
        {
            using (var db = new LiftBroContext())
                return db.Users.FirstOrDefault(user => user.Username == User.Identity.Name);
        }
    }
}
