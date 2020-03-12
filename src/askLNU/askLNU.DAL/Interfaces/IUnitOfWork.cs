﻿using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.Entities;

namespace askLNU.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Answer> Answers { get; }
        IRepository<Faculty> Faculties { get; }
        IRepository<Label> Labels { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<Question> Questions { get; }
        IRepository<Tag> Tags { get; }

        void Save();
    }
}