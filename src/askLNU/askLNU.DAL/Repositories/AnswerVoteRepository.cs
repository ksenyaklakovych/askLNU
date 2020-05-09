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
    class AnswerVoteRepository : IRepository<AnswerVote>
    {
        private ApplicationDbContext db;

        public AnswerVoteRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public void Create(AnswerVote item)
        {
            db.AnswerVotes.Add(item);
        }

        public void Delete(int id)
        {
            var vote = db.AnswerVotes.Find(id);
            if (vote != null)
            {
                db.AnswerVotes.Remove(vote);
            }
        }

        public IEnumerable<AnswerVote> Find(Func<AnswerVote, bool> predicate)
        {
            return db.AnswerVotes.Where(predicate).ToList();
        }

        public AnswerVote Get(int id)
        {
            return db.AnswerVotes.Find(id);
        }

        public IEnumerable<AnswerVote> GetAll()
        {
            return db.AnswerVotes;
        }

        public void Update(AnswerVote item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
