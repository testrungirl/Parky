using Microsoft.EntityFrameworkCore;
using ParkyAPI.Data;
using ParkyAPI.Models;
using ParkyAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Repository
{
    public class TrailRepository : ITrailRepository
    {
        private readonly ApplicationDbContext _db;
        public TrailRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool CreateTrail(Trail trail)
        {
            _db.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _db.Remove(trail);
            return Save();
        }

        public Trail GetTrail(Guid TrailId)
        {
            return _db.Trails.FirstOrDefault(x => x.Id == TrailId);
        }

        public ICollection<Trail> GetTrails()
        {
            return _db.Trails.ToList();
        }

        public bool TrailExists(string name)
        {
            return _db.Trails.Any(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public bool UpdateTrail(Trail trail)
        {
            _db.Trails.Update(trail);
            return Save();
        }

        public ICollection<Trail> GetTrailsInNationalPark(Guid NationalParkId)
        {
            return _db.Trails.Include(x=>x.NationalPark).Where(x => x.NationalPark.Id == NationalParkId).ToList();
        }
    }
}
