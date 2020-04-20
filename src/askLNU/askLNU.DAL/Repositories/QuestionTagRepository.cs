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
    class QuestionTagRepository : IRepository<QuestionTag>
    {
        private ApplicationDbContext db;

        public QuestionTagRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<QuestionTag> GetAll()
        {
            return db.QuestionTag;
        }

        public QuestionTag Get(int id)
        {
            return db.QuestionTag.Find(id);
        }

        public void Create(QuestionTag answer)
        {
            db.QuestionTag.Add(answer);
        }

        public void Update(QuestionTag answer)
        {
            db.Entry(answer).State = EntityState.Modified;
        }

        public IEnumerable<QuestionTag> Find(Func<QuestionTag, Boolean> predicate)
        {
            return db.QuestionTag.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            QuestionTag answer = db.QuestionTag.Find(id);
            if (answer != null)
                db.QuestionTag.Remove(answer);
        }

    }
}
