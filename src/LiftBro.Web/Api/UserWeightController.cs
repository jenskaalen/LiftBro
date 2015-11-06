using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiftBro.Model;

namespace LiftBro.Web.Api
{
    public class UserWeightController : ApiController
    {
        public IEnumerable<UserWeight> GetAll()
        {
            using (var db = new LiftBroContext())
            {
                var currentUser = User.Identity.Name;

                return db.UserWeights
                    .Include("User")
                    .Where(weight => weight.User.Username == currentUser).ToList();
            }
        }

        [HttpPost]
        public UserWeight Register(double amount)
        {
            var weight = new UserWeight
            {
                Weight = amount,
                Id = Guid.NewGuid(),
                User = User.GetApplicationUser()
            };

            if (weight.When.Equals(default(DateTime)))
                weight.When = DateTime.Now;

            using (var db = new LiftBroContext())
            {
                db.Users.Attach(weight.User);
                db.UserWeights.Add(weight);
                db.SaveChanges();
            }

            return weight;
        }

        [HttpPut]
        public void Update(UserWeight weight)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public void Delete(Guid id)
        {
            using (var db = new LiftBroContext())
            {
                var weight = db.UserWeights.Find(id);
                db.UserWeights.Remove(weight);
                db.SaveChanges();
            }
        }

        [HttpGet]
        public WeightStats GetWeightStats()
        {
            return new WeightStats(GetAll().ToList());
        }
    }

    public class WeightStats
    {
        public WeightStats(List<UserWeight> weights)
        {
            var maxDate = weights.Max(weight => weight.When);
            var minDate = weights.Min(weight => weight.When);
            double startWeight = weights.OrderBy(weight => weight.When).First().Weight;

            int daysBetween = DaysBetween(minDate, maxDate);

            int datesToCheckForCurrentAverage = weights.Count >= 3 ? 3 : weights.Count;

            //if (averageWeightLastWeek.Equals(default(double)))
            double averageWeightLastWeek = weights.OrderByDescending(weight => weight.When).Take(datesToCheckForCurrentAverage).Average(weight => weight.Weight);

            DailyGainRate = (averageWeightLastWeek - startWeight) / daysBetween;
            //(weights.Average(weight => weight.Weight - startWeight));
        }

        int DaysBetween(DateTime d1, DateTime d2)
        {
            TimeSpan span = d2.Subtract(d1);
            return (int)span.TotalDays;
        }

        public double DailyGainRate { get; set; }
        public double DailyGainRateTwoLastWeeks { get; set; }
    }
}
