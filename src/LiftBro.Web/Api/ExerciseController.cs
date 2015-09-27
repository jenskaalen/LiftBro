using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiftBro.Model;

namespace LiftBro.Web.Api
{
    [Authorize]
    public class ExerciseController : ApiController
    {
        public IEnumerable<Exercise> GetAll()
        {
            using (var db = new LiftBroContext())
            {
                return db.Exercises.ToList();
            }
        }

        [HttpPost]
        public void Create(Exercise exercise)
        {
            using (var db = new LiftBroContext())
            {
                var user = User.GetApplicationUser();
                db.Users.Attach(user);

                exercise.Creator = user;
                db.Exercises.Add(exercise);
                db.SaveChanges();
            }
        }

        [HttpPost]
        public void UpdateUserExercise(UserExercise userExercise)
        {
            if (userExercise.User == null)
                userExercise.User = User.GetApplicationUser();

            if (userExercise.Exercise == null)
                throw new Exception("Exercise but be set");

            using (var db = new LiftBroContext())
            {
                db.Exercises.Attach(userExercise.Exercise);

                var foundExercise = db.UserExercises
                    //.Include(exc => exc.)
                    .FirstOrDefault(exercise => 
                        exercise.User.Username == userExercise.User.Username
                        && exercise.Exercise.Name == userExercise.Exercise.Name);

                if (foundExercise != null)
                {
                    foundExercise.OneRepetitionMax = userExercise.OneRepetitionMax;
                }
                else
                {
                    db.UserExercises.Add(new UserExercise()
                    {
                        User = db.Users.Find(userExercise.User.Username),
                        Exercise = userExercise.Exercise,
                        Id = Guid.NewGuid(),
                        OneRepetitionMax =  userExercise.OneRepetitionMax
                    });
                }

                db.SaveChanges();
            }
        }
    }
}
