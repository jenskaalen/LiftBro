using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiftBro.Model;
using LiftBro.Web.Models;
using Microsoft.Ajax.Utilities;

namespace LiftBro.Web.Api
{
    public class ProgramController : ApiController
    {
        [HttpGet]
        public List<Program> GetUserRoutines()
        {
            var user = GetUser();

            using (var db = new LiftBroContext())
            {
                List<Program> userRoutines = db.UserPrograms
                    .Where(userRoutine => userRoutine.User.Id == user.Id)
                    .Include(b => b.User)
                    .Select(routine => routine.Program)
                    .Include(routine => routine.WorkoutDays
                        .Select(day => day.Exercises.OrderBy(exercise => exercise.Order).Select(exerciseDay => exerciseDay.Sets)))
                    .Include(routine => routine.WorkoutDays
                        .Select(day => day.Exercises.Select(exerciseDay => exerciseDay.Exercise)))
                        .ToList();

                return userRoutines;
            }
        }

        [HttpGet]
        public Program GetCurrentProgram()
        {
            //TODO: whatever, returning first program. fix this
            using (var db = new LiftBroContext())
            {
                return db.Programs
                    .Include(
                        routine => routine.WorkoutDays.Select(day => day.Exercises.Select(exercise => exercise.Sets)))
                        .Include(program => program.WorkoutDays.Select(day => day.Exercises.Select(exercise => exercise.Exercise)))
                        .FirstOrDefault();

            }
        }

        [HttpGet]
        public List<UserExercise> GetUserExercises()
        {
            var user = GetUser();

            using (var db = new LiftBroContext())
            {
                var exercises = db.UserExercises.Where(exercise => exercise.User.Id == user.Id)
                    .Include(b => b.User)
                    .Include(b => b.Exercise)
                    .ToList();
                return exercises;
            }
        }

        [HttpPost]
        public void SetOrm(UserExercise exercise)
        {
            using (var db = new LiftBroContext())
            {
                var user = GetUser();
                exercise.User = user;

                var existingExercise = db.UserExercises.FirstOrDefault(userExercise => userExercise.User.Id == user.Id
                                                     && userExercise.Exercise.Id == exercise.Id);
                if (existingExercise == null)
                {
                    db.UserExercises.Add(exercise);
                }
                else
                {
                    existingExercise.OneRepetationMax = exercise.OneRepetationMax;
                }

                db.SaveChanges();
            }
        }

        [HttpPost]
        public void CreateExercise(Exercise exercise)
        {
            using (var db = new LiftBroContext())
            {
                db.Exercises.Add(exercise);
                db.SaveChanges();
            }
        }

        [HttpPost]
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
                    db.Programs.Add(program);
                }

                db.SaveChanges();
            }
        }

        private LiftBro.Model.User GetUser()
        {
            //var id = new Guid("d18a1e68-4523-e511-82ba-10c37b6cd0db");

            using (var db = new LiftBroContext())
            {
                return db.Users.FirstOrDefault();
            }
        }
    }
}
