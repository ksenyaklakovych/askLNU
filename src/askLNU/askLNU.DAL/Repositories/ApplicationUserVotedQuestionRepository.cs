using askLNU.DAL.EF;
using askLNU.DAL.Entities;
using askLNU.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace askLNU.DAL.Repositories
{
    public class ApplicationUserVotedQuestionRepository : IRepository<ApplicationUserVotedQuestion>
    {
        private ApplicationDbContext db;

        public ApplicationUserVotedQuestionRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(ApplicationUserVotedQuestion item)
        {
            db.ApplicationUserVotedQuestions.Add(item);
        }

        public void Delete(int id)
        {
            var vote = db.ApplicationUserVotedQuestions.Find(id);
            if (vote != null)
            {
                db.ApplicationUserVotedQuestions.Remove(vote);
            }
        }

        public IEnumerable<ApplicationUserVotedQuestion> Find(Func<ApplicationUserVotedQuestion, bool> predicate)
        {
            return db.ApplicationUserVotedQuestions.Where(predicate).ToList();
        }

        public ApplicationUserVotedQuestion Get(int id)
        {
            return db.ApplicationUserVotedQuestions.Find(id);
        }

        public IEnumerable<ApplicationUserVotedQuestion> GetAll()
        {
            return db.ApplicationUserVotedQuestions;
        }

        public void Update(ApplicationUserVotedQuestion item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
