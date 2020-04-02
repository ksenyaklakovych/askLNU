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
    class NotificationRepository : IRepository<Notification>
    {

        private ApplicationDbContext db;

        public NotificationRepository(ApplicationDbContext context)
        {
            this.db = context;
        }

        public IEnumerable<Notification> GetAll()
        {
            return db.Notifications;
        }

        public Notification Get(int id)
        {
            return db.Notifications.Find(id);
        }

        public void Create(Notification answer)
        {
            db.Notifications.Add(answer);
        }

        public void Update(Notification answer)
        {
            db.Entry(answer).State = EntityState.Modified;
        }

        public IEnumerable<Notification> Find(Func<Notification, Boolean> predicate)
        {
            return db.Notifications.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Notification answer = db.Notifications.Find(id);
            if (answer != null)
                db.Notifications.Remove(answer);
        }

        public void Remove(string id, int qId)
        {
            throw new NotImplementedException();
        }
    }
}
