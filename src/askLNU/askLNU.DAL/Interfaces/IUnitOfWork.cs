using System;
using System.Collections.Generic;
using System.Text;
using askLNU.DAL.Entities;

namespace askLNU.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Answer> Answers { get; }
        IRepository<Faculty> Faculties { get; }
        void Save();
    }
}