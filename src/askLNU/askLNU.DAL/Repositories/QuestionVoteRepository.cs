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
    public class QuestionVoteRepository : IRepository<QuestionVote>
    {
        private ApplicationDbContext db;

        public QuestionVoteRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(QuestionVote item)
        {
            db.QuestionVotes.Add(item);
        }

        public void Delete(int id)
        {
            var vote = db.QuestionVotes.Find(id);
            if (vote != null)
            {
                db.QuestionVotes.Remove(vote);
            }
        }

        public IEnumerable<QuestionVote> Find(Func<QuestionVote, bool> predicate)
        {
            return db.QuestionVotes.Where(predicate).ToList();
        }

        public QuestionVote Get(int id)
        {
            return db.QuestionVotes.Find(id);
        }

        public IEnumerable<QuestionVote> GetAll()
        {
            return db.QuestionVotes;
        }

        public void Remove(string id, int qId)
        {
            throw new NotImplementedException();
        }

        public void Update(QuestionVote item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
