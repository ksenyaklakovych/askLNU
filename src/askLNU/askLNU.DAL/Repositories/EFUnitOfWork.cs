using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.EF;
using askLNU.DAL.Interfaces;
using askLNU.DAL.Entities;

 
namespace askLNU.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;
        private AnswerRepository answerRepository;
        private FacultyRepository facultyRepository;
        private LabelRepository labelRepository;
        private NotificationRepository notificationRepository;
        private QuestionRepository questionRepository;
        private TagRepository tagRepository;




        public EFUnitOfWork(string connectionString)
        {
            db = new ApplicationDbContext(connectionString);
        }
        public IRepository<Answer> Answers
        {
            get
            {
                if (answerRepository == null)
                    answerRepository = new AnswerRepository(db);
                return answerRepository;
            }
        }

        public IRepository<Faculty> Faculties
        {
            get
            {
                if (facultyRepository == null)
                    facultyRepository = new FacultyRepository(db);
                return facultyRepository;
            }
        }

        public IRepository<Label> Labels
        {
            get
            {
                if (labelRepository == null)
                    labelRepository = new LabelRepository(db);
                return labelRepository;
            }
        }
        public IRepository<Notification> Notifications
        {
            get
            {
                if (notificationRepository == null)
                    notificationRepository = new NotificationRepository(db);
                return notificationRepository;
            }
        }
        public IRepository<Question> Questions
        {
            get
            {
                if (questionRepository == null)
                    questionRepository = new QuestionRepository(db);
                return questionRepository;
            }
        }
        public IRepository<Tag> Tags
        {
            get
            {
                if (tagRepository == null)
                    tagRepository = new TagRepository(db);
                return tagRepository;
            }
        }









        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
