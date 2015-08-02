﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiftBro.Model;
using LiftBro.Web.Models;
using LiftBro.Web.Models.Requests;

namespace LiftBro.Web.Api
{
    public class WorkoutExerciseController : ApiController
    {
        [HttpPost]
        public void UpdateSet(SetUpdate update)
        {
            WorkoutExercise exercise = update.Exercise;
            Set set = update.Set;
            ChangeModifier modifier = update.Modifier;

            using (var db = new LiftBroContext())
            {
                switch (modifier)
                {
                    case ChangeModifier.Add:
                        //set.Id = Guid.NewGuid();

                        db.WorkoutExercises.Attach(exercise);
                        exercise.Sets.Add(set);

                        break;
                    default:
                        throw new NotImplementedException();
                }

                db.SaveChanges();
            }
        }
    }
}
