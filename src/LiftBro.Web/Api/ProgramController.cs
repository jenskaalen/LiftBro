using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using LiftBro.Model;
using LiftBro.Web.Models;
using Microsoft.Ajax.Utilities;

namespace LiftBro.Web.Api
{
    [System.Web.Http.Authorize]
    public class ProgramController : ApiController
    {
        [System.Web.Http.HttpPut]
        public void Update(Program program)
        {
            //TODO: need to account for user not owning this program
            using (var db = new LiftBroContext())
            {
                db.Programs.Attach(program);

                db.SaveChanges();
            }
        }

        [System.Web.Http.HttpGet]
        public List<Program> GetUserPrograms()
        {
            var user = User.GetApplicationUser();

            using (var db = new LiftBroContext())
            {
                List<Program> userRoutines = db.UserPrograms
                    .Where(userRoutine => userRoutine.User.Username == User.Identity.Name)
                    .Include(b => b.User)
                    .Select(routine => routine.Program)
                    .Include(routine => routine.WorkoutDays
                        .Select(
                            day =>
                                day.Exercises.Select(exerciseDay => exerciseDay.Sets)))
                    .Include(routine => routine.WorkoutDays
                        .Select(day => day.Exercises.Select(exerciseDay => exerciseDay.Exercise)))
                    .ToList();

                //ordering
                foreach (var routine in userRoutines)
                {
                    routine.WorkoutDays = routine.WorkoutDays.OrderBy(day => day.Order).ToList();
                }

                return userRoutines;
            }
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<Program> GetAll()
        {
            using (var db = new LiftBroContext())
            {
                return db.Programs.ToList();
            }
        }

        [System.Web.Http.HttpPost]
        public void SelectProgram(Program program)
        {
            Model.User currentUser = User.GetApplicationUser();

            using (var db = new LiftBroContext())
            {
                db.Users.Attach(currentUser);
                db.Programs
                    .Attach(program);

                //disabling all other programs
                //NOTE: it should practically not be possible to have more than one program that is currently used
                var currentPrograms = db.UserPrograms.Where(userProgram => userProgram.CurrentlyUsing).ToList();
                if (currentPrograms.Any())
                {
                    currentPrograms.ForEach(userProgram => userProgram.CurrentlyUsing = false);
                }

                var existingProgram = db.UserPrograms
                    .Include(userP => userP.Program.WorkoutDays)
                    .FirstOrDefault(userProgram => userProgram.Program.Id == program.Id);

                if (existingProgram != null)
                {
                    existingProgram.CurrentlyUsing = true;

                    if (existingProgram.NextWorkout == null)
                        existingProgram.NextWorkout =
                            existingProgram.Program.WorkoutDays.OrderBy(day => day.Order).FirstOrDefault();
                }
                else
                {
                    db.UserPrograms.Add(new UserProgram()
                    {
                        CurrentlyUsing = true, 
                        Id = Guid.NewGuid(),
                        Program =  program,
                        User = currentUser,
                        NextWorkout = db.Programs
                        .Include(pr => pr.WorkoutDays)
                        .FirstOrDefault(pr => pr.Id == program.Id)
                        .WorkoutDays.OrderBy(day => day.Order).FirstOrDefault()
                        
                        //.WorkoutDays.OrderBy(day => day.Order).FirstOrDefault()
                        //TODO: might need to set currentworkout here
                    });
                }

                db.SaveChanges();
            }
        }

        [System.Web.Http.HttpGet]
        public Program GetCurrentProgram()
        {
            //TODO: whatever, returning first program. fix this
            using (var db = new LiftBroContext())
            {
                var currentUserProgram = db.UserPrograms
                    .Include(program => program.User)
                    .Include(program => program.Program)
                    .FirstOrDefault(program => program.User.Username == User.Identity.Name 
                    && program.CurrentlyUsing);

                return db.Programs
                    .Include(
                        routine => routine.WorkoutDays.Select(day => day.Exercises.Select(exercise => exercise.Sets)))
                        .Include(program => program.WorkoutDays.Select(day => day.Exercises.Select(exercise => exercise.Exercise)))
                        .FirstOrDefault(program => program.Id == currentUserProgram.Program.Id);
            }
        }

        [System.Web.Http.HttpGet]
        public List<UserExercise> GetUserExercises()
        {
            var user = User.GetApplicationUser();

            using (var db = new LiftBroContext())
            {
                var exercises = db.UserExercises.Where(exercise => exercise.User.Username == User.Identity.Name)
                    .Include(b => b.User)
                    .Include(b => b.Exercise)
                    .ToList();
                return exercises;
            }
        }

        [System.Web.Http.HttpPost]
        public void SetOrm(UserExercise exercise)
        {
            using (var db = new LiftBroContext())
            {
                var user = User.GetApplicationUser();
                exercise.User = user;

                var existingExercise = db.UserExercises.FirstOrDefault(userExercise => userExercise.User.Username == User.Identity.Name
                                                     && userExercise.Exercise.Id == exercise.Id);
                if (existingExercise == null)
                {
                    db.UserExercises.Add(exercise);
                }
                else
                {
                    existingExercise.OneRepetitionMax = exercise.OneRepetitionMax;
                }

                db.SaveChanges();
            }
        }

        [System.Web.Http.HttpPost]
        public void CreateExercise(Exercise exercise)
        {
            using (var db = new LiftBroContext())
            {
                db.Exercises.Add(exercise);
                db.SaveChanges();
            }
        }

        [System.Web.Http.HttpPost]
        public void CreateUpdateProgram(Program program)
        {
            using (var db = new LiftBroContext())
            {
                var existingEntry = db.Programs.FirstOrDefault(program1 => program1.Id == program.Id);

                if (existingEntry != null)
                {
                    db.Programs.Attach(program);
                    db.Entry(existingEntry).State = EntityState.Modified;
                }
                else
                {
                    program.Id = Guid.NewGuid();
                    program.Creator = User.GetApplicationUser();
                    db.Users.Attach(program.Creator);
                    db.Programs.Add(program);
                }

                db.SaveChanges();
            }
        }
    }
}
