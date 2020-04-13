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

    class ApplicationUserFavoriteQuestionRepository : IRepository<ApplicationUserFavoriteQuestion>
    {

        private ApplicationDbContext db;

        public ApplicationUserFavoriteQuestionRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<ApplicationUserFavoriteQuestion> GetAll()
        {
            return db.ApplicationUserFavoriteQuestion;
        }

        public ApplicationUserFavoriteQuestion Get(int id)
        {
            return db.ApplicationUserFavoriteQuestion.Find(id);
        }
        

        public void Create(ApplicationUserFavoriteQuestion answer)
        {
            db.ApplicationUserFavoriteQuestion.Add(answer);
        }

        public void Update(ApplicationUserFavoriteQuestion answer)
        {
            db.Entry(answer).State = EntityState.Modified;
        }

        public IEnumerable<ApplicationUserFavoriteQuestion> Find(Func<ApplicationUserFavoriteQuestion, Boolean> predicate)
        {
            return db.ApplicationUserFavoriteQuestion.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            ApplicationUserFavoriteQuestion answer = db.ApplicationUserFavoriteQuestion.Find(id);
            if (answer != null)
                db.ApplicationUserFavoriteQuestion.Remove(answer);
        }
       
    }
}
