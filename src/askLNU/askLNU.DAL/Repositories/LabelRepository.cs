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
    class LabelRepository : IRepository<Label>
    {
        private ApplicationDbContext db;

        public LabelRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Label> GetAll()
        {
            return db.Labels;
        }

        public Label Get(int id)
        {
            return db.Labels.Find(id);
        }

        public void Create(Label answer)
        {
            db.Labels.Add(answer);
        }

        public void Update(Label answer)
        {
            db.Entry(answer).State = EntityState.Modified;
        }

        public IEnumerable<Label> Find(Func<Label, Boolean> predicate)
        {
            return db.Labels.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Label answer = db.Labels.Find(id);
            if (answer != null)
                db.Labels.Remove(answer);
        }

        public void Remove(string id, int qId)
        {
            throw new NotImplementedException();
        }
    }
}
