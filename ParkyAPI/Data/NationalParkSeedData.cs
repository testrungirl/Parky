using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Data
{
    public static class NationalParkSeedData
    {
        public static void Initialize(ApplicationDbContext dbContext)
        {
            var Data = new List<NationalPark>()
            {
                new NationalPark{ Name = "Mongo Park", Established = new DateTime(2010, 10, 01), Created= new DateTime(2010, 10, 01), State = "Delta Guniea" },
                new NationalPark{ Name = "Ketu Park", Established = new DateTime(2013, 9, 1), Created= new DateTime(2013, 9, 1), State = "Lagos" },
                new NationalPark{ Name = "Urgi Park", Established = new DateTime(2019, 5, 1), Created= new DateTime(2019, 5, 1), State = "Edo" },
            };
            if(dbContext.NationalParks.ToList().Count() == 0)
            {
                dbContext.AddRange(Data);
                dbContext.SaveChanges();
            }
        }
    }
}

