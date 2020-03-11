using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.Entities;

namespace askLNU.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Answer> Answers { get; }
        IRepository<ApplicationUser> ApplicationUsers { get; }
        IRepository<ApplicationUserFavoriteQuestion> ApplicationUserFavoriteQuestions { get; }
        IRepository<ApplicationUserLabel> ApplicationUserLabels { get; }
        IRepository<ErrorViewModel> ErrorViewModels { get; }
        IRepository<Faculty> Faculties { get; }
        IRepository<Label> Labels { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<Question> Questions { get; }
        IRepository<QuestionTag> QuestionTags { get; }
        IRepository<Tag> Tags { get; }



        void Save();
    }
}