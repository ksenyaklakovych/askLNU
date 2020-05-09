using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.EF;
using askLNU.DAL.Interfaces;
using askLNU.DAL.Entities;
using askLNU.DAL.Repositories;

namespace askLNU.DAL.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;
        private AnswerRepository answerRepository;
        private FacultyRepository facultyRepository;
        private LabelRepository labelRepository;
        private NotificationRepository notificationRepository;
        private QuestionRepository questionRepository;
        private QuestionTagRepository questionTagRepository;
        private TagRepository tagRepository;
        private ApplicationUserFavoriteQuestionRepository favoriteQuestionRepository;
        private QuestionVoteRepository questionVoteRepository;
        private AnswerVoteRepository answerVoteRepository;


        public EFUnitOfWork(ApplicationDbContext context)
        {
            db = context;
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

        public IRepository<QuestionTag> QuestionTag
        {
            get
            {
                if (questionTagRepository == null)
                    questionTagRepository = new QuestionTagRepository(db);
                return questionTagRepository;
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
        
        public IRepository<ApplicationUserFavoriteQuestion> ApplicationUserFavoriteQuestion
        {
            get
            {
                if (favoriteQuestionRepository == null)
                    favoriteQuestionRepository = new ApplicationUserFavoriteQuestionRepository(db);
                return favoriteQuestionRepository;
            }
        }

        public IRepository<QuestionVote> QuestionVotes
        {
            get
            {
                if (questionVoteRepository == null)
                    questionVoteRepository = new QuestionVoteRepository(db);
                return questionVoteRepository;
            }
        }

        public IRepository<AnswerVote> AnswerVotes
        {
            get
            {
                if (answerVoteRepository == null)
                    answerVoteRepository = new AnswerVoteRepository(db);
                return answerVoteRepository;
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
