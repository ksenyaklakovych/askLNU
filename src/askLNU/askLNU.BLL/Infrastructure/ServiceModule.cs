using System;
using System.Collections.Generic;
using System.Text;
using Ninject.Modules;
using askLNU.DAL.Interfaces;
using askLNU.DAL.Repositories;

namespace askLNU.BLL.Infrastructure
{
    class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
        }
    }
}
