using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.Entities;
using askLNU.DAL.EF;
using askLNU.DAL.Interfaces;
namespace askLNU.DAL.Repositories
{
    class AnswerRepository : IRepository<Answer>
    {
        private ApplicationDbContext db;

        public AnswerRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Answer> GetAll()
        {
            return db.Answers;
        }

        public Answer Get(int id)
        {
            return db.Answers.Find(id);
        }

        public void Create(Answer answer)
        {
            db.Answers.Add(answer);
        }

        public void Update(Answer answer)
        {
            db.Entry(answer).State = EntityState.Modified;
        }

        public IEnumerable<Answer> Find(Func<Answer, Boolean> predicate)
        {
            return db.Answers.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Answer answer = db.Answers.Find(id);
            if (answer != null)
                db.Answers.Remove(answer);
        }
    }
}
    
    

