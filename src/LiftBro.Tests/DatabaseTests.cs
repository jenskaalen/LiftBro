using System;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LiftBro.Model;
using Newtonsoft.Json;

namespace LiftBro.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        //[TestMethod]
        //public void Database_creation()
        //{
        //    using (var db = new LiftBroContext())
        //    {
        //        db.Exercises.Add(new Exercise());
        //        db.SaveChanges();
        //    }
        //}

        [TestMethod]
        public void Deserialize_seed_data()
        {
            //var file = File.Open("seedData.json", FileMode.Open);

            using (var db = new LiftBroContext())
            {
                SeedData data = JsonConvert.DeserializeObject<SeedData>(File.ReadAllText("seedData.json"));
                db.UserPrograms.AddOrUpdate(data.UserPrograms.First());
                db.SaveChanges();

                var exercise = new UserExercise()
                {
                    Exercise = db.Exercises.First(),
                    Id = new Guid(),
                    OneRepetationMax = 44,
                    User = db.Users.First()
                };

                db.UserExercises.AddOrUpdate(exercise);
                db.SaveChanges();
            }
        }
    }
}
