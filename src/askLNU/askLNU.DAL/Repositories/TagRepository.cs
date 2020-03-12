using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.Entities;
using askLNU.DAL.EF;
using askLNU.DAL.Interfaces;

namespace askLNU.DAL.Repositories
{

    class TagRepository : IRepository<Tag>
    {

        private ApplicationDbContext db;

        public TagRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Tag> GetAll()
        {
            return db.Tags;
        }

        public Tag Get(int id)
        {
            return db.Tags.Find(id);
        }

        public void Create(Tag answer)
        {
            db.Tags.Add(answer);
        }

        public void Update(Tag answer)
        {
            db.Entry(answer).State = EntityState.Modified;
        }

        public IEnumerable<Tag> Find(Func<Tag, Boolean> predicate)
        {
            return db.Tags.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Tag answer = db.Tags.Find(id);
            if (answer != null)
                db.Tags.Remove(answer);
        }
    }
}
