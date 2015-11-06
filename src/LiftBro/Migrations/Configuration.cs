using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using LiftBro.Model;

namespace LiftBro.Migrations
{
    public class Configuration : DbMigrationsConfiguration<LiftBroContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "LiftBro.LiftBroContext";
        }
        protected override void Seed(LiftBroContext context)
        {
            //SeedWithoutUser(context);
        }

        private void SeedWithoutUser(LiftBroContext context)
        {
            //cleaning out the entire db because EF is a schtickler when it comes to updating seed
            //context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            //context.Database.ExecuteSqlCommand(
            //    "sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            //context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

            Exercise squats = new Exercise() { Name = "Squats", Id = new Guid("2a4743fa-0324-e511-82ba-10c37b6cd0db") };
            context.Exercises.AddOrUpdate(squats);
            Exercise deads = new Exercise() { Name = "Deadlifts", Id = new Guid("424743fa-0324-e511-82ba-10c37b6cd0db") };
            context.Exercises.AddOrUpdate(deads);

            context.SaveChanges();

            var squatToUse = context.Exercises.Find(squats.Id);

            var workoutDays = new List<WorkoutDay>()
            {
                new WorkoutDay()
                {
                    Id = new Guid("2a4743fa-0324-d633-82ba-16c37b6cd0db"),
                    Order = 1,
                    Exercises = new List<WorkoutExercise>()
                    {
                        new WorkoutExercise()
                        {
                            Exercise = context.Exercises.Find(squats.Id),
                            Id = new Guid("2a4743fa-0324-e514-82ba-14c37b6cd0db"),
                            Order = 1,
                            Sets = new List<Set>()
                            {
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-44ba-14c37b6cd0db"),
                                    ORMPercentage = 70,
                                    Reps = 5,
                                    Order = 1
                                },
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-45ba-14c37b6cd0db"),
                                    ORMPercentage = 80,
                                    Reps = 5,
                                    Order = 2
                                },
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-46ba-14c37b6cd0db"),
                                    ORMPercentage = 90,
                                    Reps = 5,
                                    Order = 3
                                }
                            }
                        },
                        new WorkoutExercise()
                        {
                            Exercise = squatToUse,
                            Id = new Guid("344743fa-0324-e514-82ba-14c37b6cd0db"),
                            Order = 2,
                            Sets = new List<Set>()
                            {
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-44ba-14c37b6cd0db"),
                                    ORMPercentage = 60,
                                    Reps = 10,
                                    Order = 1
                                },
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-45ba-14c37b6cd0db"),
                                    ORMPercentage = 60,
                                    Reps = 10,
                                    Order = 2
                                },
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-46ba-14c37b6cd0db"),
                                    ORMPercentage = 60,
                                    Reps = 10,
                                    Order = 3
                                }
                            }
                        }
                    }
                },

                new WorkoutDay()
                {
                    Id = new Guid("2a4743fa-0324-d633-82ba-14c37b6aa0db"),
                    Order = 2,
                    Exercises = new List<WorkoutExercise>()
                    {
                        new WorkoutExercise()
                        {
                            Exercise = context.Exercises.Find(deads.Id),
                            Id = new Guid("2a4743fa-0324-e514-82ba-14c37b6aa0db"),
                            Order = 1,
                            Sets = new List<Set>()
                            {
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-44ba-14c37b6aa0db"),
                                    ORMPercentage = 70,
                                    Reps = 3,
                                    Order = 1
                                },
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-45ba-14c37b6aa0db"),
                                    ORMPercentage = 80,
                                    Reps = 3,
                                    Order = 2
                                },
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-46ba-14c37b6aa0db"),
                                    ORMPercentage = 90,
                                    Reps = 3,
                                    Order = 3
                                }
                            }
                        },
                        new WorkoutExercise()
                        {
                            Exercise = context.Exercises.Find(deads.Id),
                            Id = new Guid("344743fa-0324-e514-82ba-14c37b6aa0db"),
                            Order = 2,
                            Sets = new List<Set>()
                            {
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-44ba-14c37b6aa0db"),
                                    ORMPercentage = 60,
                                    Reps = 2,
                                    Order = 1
                                },
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-45ba-14c37b6aa0db"),
                                    ORMPercentage = 60,
                                    Reps = 2,
                                    Order = 2
                                },
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-46ba-14c37b6aa0db"),
                                    ORMPercentage = 60,
                                    Reps = 2,
                                    Order = 3
                                }
                            }
                        }
                    }
                }
            };

            //context.WorkoutWeeks.AddOrUpdate(weekA);
            //var workoutWeekToUse = context.WorkoutWeeks.Find(weekA.Id);


            context.SaveChanges();

            var routine = new Program()
            {
                Name = "Stronglifts",
                Id = new Guid("664743fa-0324-e511-46ba-14c37b6cd0db"),
                WorkoutDays = workoutDays
            };

            context.Programs.AddOrUpdate(routine);

        }

        private void SeedWithUser(LiftBroContext context)
        {

            //cleaning out the entire db because EF is a schtickler when it comes to updating seed
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            context.Database.ExecuteSqlCommand(
                "sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

            string username = "ja.kaalen@gmail.com";
            Exercise squats = new Exercise() { Name = "Squats", Id = new Guid("2a4743fa-0324-e511-82ba-10c37b6cd0db") };
            context.Exercises.AddOrUpdate(squats);
            Exercise deads = new Exercise() { Name = "Deadlifts", Id = new Guid("424743fa-0324-e511-82ba-10c37b6cd0db") };
            context.Exercises.AddOrUpdate(deads);

            User user = new User()
            {
                Username = username,
                Name = "Jens Brohard"
            };

            context.SaveChanges();

            var squatToUse = context.Exercises.Find(squats.Id);


            context.Users.AddOrUpdate(user);

            //var weekA = new WorkoutWeek()
            //{
            //    Id = new Guid("2a4743fa-0324-d633-96af-16c37b6cd0db"),
            //    Order = 1,
            //    WorkoutDays = new List<WorkoutDay>()
            //    {
            //    }
            //};

            var workoutDays = new List<WorkoutDay>()
            {
                new WorkoutDay()
                {
                    Id = new Guid("2a4743fa-0324-d633-82ba-16c37b6cd0db"),
                    Order = 1,
                    Exercises = new List<WorkoutExercise>()
                    {
                        new WorkoutExercise()
                        {
                            Exercise = context.Exercises.Find(squats.Id),
                            Id = new Guid("2a4743fa-0324-e514-82ba-14c37b6cd0db"),
                            Order = 1,
                            Sets = new List<Set>()
                            {
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-44ba-14c37b6cd0db"),
                                    ORMPercentage = 70,
                                    Reps = 5,
                                    Order = 1
                                },
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-45ba-14c37b6cd0db"),
                                    ORMPercentage = 80,
                                    Reps = 5,
                                    Order = 2
                                },
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-46ba-14c37b6cd0db"),
                                    ORMPercentage = 90,
                                    Reps = 5,
                                    Order = 3
                                }
                            }
                        },
                        new WorkoutExercise()
                        {
                            Exercise = squatToUse,
                            Id = new Guid("344743fa-0324-e514-82ba-14c37b6cd0db"),
                            Order = 2,
                            Sets = new List<Set>()
                            {
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-44ba-14c37b6cd0db"),
                                    ORMPercentage = 60,
                                    Reps = 10,
                                    Order = 1
                                },
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-45ba-14c37b6cd0db"),
                                    ORMPercentage = 60,
                                    Reps = 10,
                                    Order = 2
                                },
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-46ba-14c37b6cd0db"),
                                    ORMPercentage = 60,
                                    Reps = 10,
                                    Order = 3
                                }
                            }
                        }
                    }
                },

                new WorkoutDay()
                {
                    Id = new Guid("2a4743fa-0324-d633-82ba-14c37b6aa0db"),
                    Order = 2,
                    Exercises = new List<WorkoutExercise>()
                    {
                        new WorkoutExercise()
                        {
                            Exercise = context.Exercises.Find(deads.Id),
                            Id = new Guid("2a4743fa-0324-e514-82ba-14c37b6aa0db"),
                            Order = 1,
                            Sets = new List<Set>()
                            {
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-44ba-14c37b6aa0db"),
                                    ORMPercentage = 70,
                                    Reps = 3,
                                    Order = 1
                                },
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-45ba-14c37b6aa0db"),
                                    ORMPercentage = 80,
                                    Reps = 3,
                                    Order = 2
                                },
                                new Set()
                                {
                                    Id = new Guid("2a4743fa-0324-e511-46ba-14c37b6aa0db"),
                                    ORMPercentage = 90,
                                    Reps = 3,
                                    Order = 3
                                }
                            }
                        },
                        new WorkoutExercise()
                        {
                            Exercise = context.Exercises.Find(deads.Id),
                            Id = new Guid("344743fa-0324-e514-82ba-14c37b6aa0db"),
                            Order = 2,
                            Sets = new List<Set>()
                            {
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-44ba-14c37b6aa0db"),
                                    ORMPercentage = 60,
                                    Reps = 2,
                                    Order = 1
                                },
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-45ba-14c37b6aa0db"),
                                    ORMPercentage = 60,
                                    Reps = 2,
                                    Order = 2
                                },
                                new Set()
                                {
                                    Id = new Guid("344743fa-0324-e511-46ba-14c37b6aa0db"),
                                    ORMPercentage = 60,
                                    Reps = 2,
                                    Order = 3
                                }
                            }
                        }
                    }
                }
            };

            //context.WorkoutWeeks.AddOrUpdate(weekA);
            //var workoutWeekToUse = context.WorkoutWeeks.Find(weekA.Id);


            context.SaveChanges();

            var routine = new Program()
            {
                Name = "Stronglifts",
                Id = new Guid("664743fa-0324-e511-46ba-14c37b6cd0db"),
                WorkoutDays = workoutDays
            };

            context.Programs.AddOrUpdate(routine);

            var dbUser = context.Users.Find(username);
            context.SaveChanges();

            var userExercise = new UserExercise()
            {
                Exercise = squatToUse,
                Id = new Guid("344743fa-0333-e511-46ba-14c37b6cd0db"),
                OneRepetitionMax = 100,
                User = dbUser
            };


            var userDeadlifts = new UserExercise()
            {
                Exercise = context.Exercises.Find(deads.Id),
                Id = new Guid("344743fa-4242-e511-46ba-14c37b6cd0db"),
                OneRepetitionMax = 233,
                User = dbUser
            };

            var firstWorkoutDay = context.Programs.FirstOrDefault().WorkoutDays.FirstOrDefault();

            var userRoutine = new UserProgram()
            {
                Program = context.Programs.Find(routine.Id),
                Id = new Guid("664743fa-0324-e511-46ba-14c37b6cd0db"),
                User = dbUser,
                CurrentlyUsing = true,
                NextWorkout = firstWorkoutDay
            };

            context.UserPrograms.AddOrUpdate(userRoutine);

            //context.SaveChanges();

            context.UserExercises.AddOrUpdate(userExercise);
            context.UserExercises.AddOrUpdate(userDeadlifts);

            WorkoutDay pastWorkoutDay = context.WorkoutDays.First();

            context.CompletedWorkouts.AddOrUpdate(
                new CompletedWorkoutDay
                {
                    User = dbUser,
                    When = new DateTime(2015, 6, 6, 13, 0, 0),
                    Workout = pastWorkoutDay
                },
                new CompletedWorkoutDay
                {
                    User = dbUser,
                    When = new DateTime(2015, 6, 2, 13, 0, 0),
                    Workout = pastWorkoutDay
                }
                );
        }
    }
}
