using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiftBro.Model;
using Newtonsoft.Json;

namespace LiftBro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public class Configuration : DbMigrationsConfiguration<LiftBro.LiftBroContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "LiftBro.LiftBroContext";
        }

        protected override void Seed(LiftBro.LiftBroContext context)
        {
            //cleaning out the entire db because EF is a schtickler when it comes to updating seed
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            context.Database.ExecuteSqlCommand(
                "sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");


            SeedData data = JsonConvert.DeserializeObject<SeedData>(File.ReadAllText("seedData.json"));
            context.UserPrograms.AddOrUpdate(data.UserPrograms.First());
            context.SaveChanges();

            var exercise = new UserExercise()
            {
                Exercise = context.Exercises.First(),
                Id = new Guid(),
                OneRepetitionMax = 44,
                User = context.Users.First()
            };

            context.UserExercises.AddOrUpdate(exercise);
            context.SaveChanges();

            //Exercise squats = new Exercise() {Name = "Squats", Id = new Guid("2a4743fa-0324-e511-82ba-10c37b6cd0db")};
            //context.Exercises.AddOrUpdate(squats);
            //Exercise deads = new Exercise() {Name = "Deadlifts", Id = new Guid("424743fa-0324-e511-82ba-10c37b6cd0db")};
            //context.Exercises.AddOrUpdate(deads);

            //User user = new User()
            //{
            //    Id = new Guid("366743fa-0324-e511-46ba-14c37b6cd0db"),
            //    Name = "Jens Brohard"
            //};

            //context.SaveChanges();

            //var squatToUse = context.Exercises.Find(squats.Id);


            //context.Users.AddOrUpdate(user);

            ////var weekA = new WorkoutWeek()
            ////{
            ////    Id = new Guid("2a4743fa-0324-d633-96af-16c37b6cd0db"),
            ////    Order = 1,
            ////    WorkoutDays = new List<WorkoutDay>()
            ////    {
            ////    }
            ////};

            //var workoutDays = new List<WorkoutDay>()
            //{
            //    new WorkoutDay()
            //    {
            //        Id = new Guid("2a4743fa-0324-d633-82ba-16c37b6cd0db"),
            //        Order = 1,
            //        Exercises = new List<WorkoutExercise>()
            //        {
            //            new WorkoutExercise()
            //            {
            //                Exercise = context.Exercises.Find(squats.Id),
            //                Id = new Guid("2a4743fa-0324-e514-82ba-14c37b6cd0db"),
            //                Order = 1,
            //                Sets = new List<Set>()
            //                {
            //                    new Set()
            //                    {
            //                        Id = new Guid("2a4743fa-0324-e511-44ba-14c37b6cd0db"),
            //                        ORMPercentage = 70,
            //                        Reps = 5,
            //                        Order = 1
            //                    },
            //                    new Set()
            //                    {
            //                        Id = new Guid("2a4743fa-0324-e511-45ba-14c37b6cd0db"),
            //                        ORMPercentage = 80,
            //                        Reps = 5,
            //                        Order = 2
            //                    },
            //                    new Set()
            //                    {
            //                        Id = new Guid("2a4743fa-0324-e511-46ba-14c37b6cd0db"),
            //                        ORMPercentage = 90,
            //                        Reps = 5,
            //                        Order = 3
            //                    }
            //                }
            //            },
            //            new WorkoutExercise()
            //            {
            //                Exercise = squatToUse,
            //                Id = new Guid("344743fa-0324-e514-82ba-14c37b6cd0db"),
            //                Order = 2,
            //                Sets = new List<Set>()
            //                {
            //                    new Set()
            //                    {
            //                        Id = new Guid("344743fa-0324-e511-44ba-14c37b6cd0db"),
            //                        ORMPercentage = 60,
            //                        Reps = 10,
            //                        Order = 1
            //                    },
            //                    new Set()
            //                    {
            //                        Id = new Guid("344743fa-0324-e511-45ba-14c37b6cd0db"),
            //                        ORMPercentage = 60,
            //                        Reps = 10,
            //                        Order = 2
            //                    },
            //                    new Set()
            //                    {
            //                        Id = new Guid("344743fa-0324-e511-46ba-14c37b6cd0db"),
            //                        ORMPercentage = 60,
            //                        Reps = 10,
            //                        Order = 3
            //                    }
            //                }
            //            }
            //        }
            //    },

            //    new WorkoutDay()
            //    {
            //        Id = new Guid("2a4743fa-0324-d633-82ba-14c37b6aa0db"),
            //        Order = 1,
            //        Exercises = new List<WorkoutExercise>()
            //        {
            //            new WorkoutExercise()
            //            {
            //                Exercise = context.Exercises.Find(deads.Id),
            //                Id = new Guid("2a4743fa-0324-e514-82ba-14c37b6aa0db"),
            //                Order = 1,
            //                Sets = new List<Set>()
            //                {
            //                    new Set()
            //                    {
            //                        Id = new Guid("2a4743fa-0324-e511-44ba-14c37b6aa0db"),
            //                        ORMPercentage = 70,
            //                        Reps = 3,
            //                        Order = 1
            //                    },
            //                    new Set()
            //                    {
            //                        Id = new Guid("2a4743fa-0324-e511-45ba-14c37b6aa0db"),
            //                        ORMPercentage = 80,
            //                        Reps = 3,
            //                        Order = 2
            //                    },
            //                    new Set()
            //                    {
            //                        Id = new Guid("2a4743fa-0324-e511-46ba-14c37b6aa0db"),
            //                        ORMPercentage = 90,
            //                        Reps = 3,
            //                        Order = 3
            //                    }
            //                }
            //            },
            //            new WorkoutExercise()
            //            {
            //                Exercise = context.Exercises.Find(deads.Id),
            //                Id = new Guid("344743fa-0324-e514-82ba-14c37b6aa0db"),
            //                Order = 2,
            //                Sets = new List<Set>()
            //                {
            //                    new Set()
            //                    {
            //                        Id = new Guid("344743fa-0324-e511-44ba-14c37b6aa0db"),
            //                        ORMPercentage = 60,
            //                        Reps = 2,
            //                        Order = 1
            //                    },
            //                    new Set()
            //                    {
            //                        Id = new Guid("344743fa-0324-e511-45ba-14c37b6aa0db"),
            //                        ORMPercentage = 60,
            //                        Reps = 2,
            //                        Order = 2
            //                    },
            //                    new Set()
            //                    {
            //                        Id = new Guid("344743fa-0324-e511-46ba-14c37b6aa0db"),
            //                        ORMPercentage = 60,
            //                        Reps = 2,
            //                        Order = 3
            //                    }
            //                }
            //            }
            //        }
            //    }
            //};

            //context.WorkoutWeeks.AddOrUpdate(weekA);
            //var workoutWeekToUse = context.WorkoutWeeks.Find(weekA.Id);


            //TODO: fix the seed of user data
            ////context.SaveChanges();

            ////var routine = new Program()
            ////{
            ////    Name = "Stronglifts",
            ////    Id = new Guid("664743fa-0324-e511-46ba-14c37b6cd0db"),
            ////    WorkoutDays = workoutDays
            ////};

            ////context.Programs.AddOrUpdate(routine);

            ////var dbUser = context.Users.Find(user.Id);
            ////context.SaveChanges();

            ////var userExercise = new UserExercise()
            ////{
            ////    Exercise = squatToUse,
            ////    Id = new Guid("344743fa-0333-e511-46ba-14c37b6cd0db"),
            ////    OneRepetationMax = 100,
            ////    User = dbUser
            ////};


            ////var userDeadlifts = new UserExercise()
            ////{
            ////    Exercise = context.Exercises.Find(deads.Id),
            ////    Id = new Guid("344743fa-4242-e511-46ba-14c37b6cd0db"),
            ////    OneRepetationMax = 233,
            ////    User = dbUser
            ////};

            ////var userRoutine = new UserProgram()
            ////{
            ////    Program = context.Programs.Find(routine.Id),
            ////    Id = new Guid("664743fa-0324-e511-46ba-14c37b6cd0db"),
            ////    User = dbUser,
            ////    CurrentlyUsing = false
            ////};

            ////context.UserPrograms.AddOrUpdate(userRoutine);


            ////context.UserExercises.AddOrUpdate(userExercise);
            ////context.UserExercises.AddOrUpdate(userDeadlifts);

        }
    }
}
