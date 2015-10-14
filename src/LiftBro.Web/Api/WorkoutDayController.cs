using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using LiftBro.Model;
using LiftBro.Web.Models;
using LiftBro.Web.Models.Requests;

namespace LiftBro.Web.Api
{
    [Authorize]
    public class WorkoutDayController : ApiController
    {
        [HttpDelete]
        public void Delete(Guid id)
        {
            using (var db = new LiftBroContext())
            {
                var day = db.WorkoutDays.Find(id);

                var programs = db.UserPrograms
                    .Include(program => program.NextWorkout)
                    .Where(program => program.NextWorkout.Id == id).ToList();
                programs.ForEach(program => program.NextWorkout = null);

                db.WorkoutDays.Remove(day);
                db.SaveChanges();
            }
        }

        [HttpPut]
        public void Update(WorkoutDay workoutDay)
        {
            using (var db = new LiftBroContext())
            {
                db.WorkoutDays.Attach(workoutDay);
                db.ChangeTracker.Entries<WorkoutDay>().First(e => e.Entity == workoutDay)
                                                      .State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void Create(CreateWorkoutDay update)
        {
            using (var db = new LiftBroContext())
            {
                var program =
                    db.Programs.Include(pr => pr.WorkoutDays)
                        .FirstOrDefault(program1 => program1.Id == update.Program.Id);

                if  (program == null)
                    throw new Exception("Program not found");

                program.WorkoutDays.Add(update.WorkoutDay);
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

        [HttpPost]
        public void SetNextWorkoutDay([FromBody]string id)
        {
            using (var db = new LiftBroContext())
            {
                Guid guidId = Guid.Parse(id);

                UserProgram userProgram = db.UserPrograms
                    .Include("User")
                    //.Include("Program.WorkoutDays")
                    .FirstOrDefault(program => program.User.Username == User.Identity.Name && program.CurrentlyUsing);

                WorkoutDay workoutDay = db.WorkoutDays.Find(guidId);

                userProgram.NextWorkout = workoutDay;
                db.SaveChanges();
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
                    .Include("Program")
                    .Include("Program.WorkoutDays")
                    .FirstOrDefault(program =>
                    program.User.Username == User.Identity.Name
                    && program.CurrentlyUsing);

                WorkoutDay nextWorkoutDay = currentUserProgram.NextWorkout;

                if (nextWorkoutDay == null)
                {
                    currentUserProgram.NextWorkout = currentUserProgram.Program.WorkoutDays
                        .OrderBy(day => day.Order).FirstOrDefault();
                    nextWorkoutDay = currentUserProgram.NextWorkout;
                    db.SaveChanges();
                }

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
                //TOOD:unecessary attach?
                db.Users.Attach(currentUser);

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

                db.CompletedWorkouts.Add(new CompletedWorkoutDay()
                {
                    User = currentUser, 
                    When = DateTime.Now,
                    Workout = db.WorkoutDays.FirstOrDefault(day => day.Id == workoutDay.Id)
                });

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
