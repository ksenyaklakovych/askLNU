using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.Entities;
using askLNU.DAL.EF;
using askLNU.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;



namespace askLNU.DAL.Repositories
{
    class FacultyRepository : IRepository<Faculty>
    {
        private ApplicationDbContext db;

        public FacultyRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Faculty> GetAll()
        {
            return db.Faculties;
        }

        public Faculty Get(int id)
        {
            return db.Faculties.Find(id);
        }

        public void Create(Faculty answer)
        {
            db.Faculties.Add(answer);
        }

        public void Update(Faculty answer)
        {
            db.Entry(answer).State = EntityState.Modified;
        }

        public IEnumerable<Faculty> Find(Func<Faculty, Boolean> predicate)
        {
            return db.Faculties.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Faculty answer = db.Faculties.Find(id);
            if (answer != null)
                db.Faculties.Remove(answer);
        }
    }
}
