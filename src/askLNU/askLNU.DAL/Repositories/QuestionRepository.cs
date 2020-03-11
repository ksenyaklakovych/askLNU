using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.Entities;
using askLNU.DAL.EF;
using askLNU.DAL.Interfaces;

namespace askLNU.DAL.Repositories
{

    class QuestionRepository : IRepository<Question>
    {

        private ApplicationDbContext db;

        public QuestionRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<QuestionRepository> GetAll()
        {
            return db.Questions;
        }

        public Question Get(int id)
        {
            return db.Questions.Find(id);
        }

        public void Create(Question answer)
        {
            db.Questions.Add(answer);
        }

        public void Update(Question answer)
        {
            db.Entry(answer).State = EntityState.Modified;
        }

        public IEnumerable<Question> Find(Func<Question, Boolean> predicate)
        {
            return db.Questions.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Question answer = db.Questions.Find(id);
            if (answer != null)
                db.Questions.Remove(answer);
        }
    }
}
