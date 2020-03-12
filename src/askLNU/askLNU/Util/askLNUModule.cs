namespace askLNU.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Ninject.Modules;
    using askLNU.BLL.Interfaces;
    using askLNU.BLL.Services;

    public class askLNUModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IAnswerService>().To<AnswerService>();
            this.Bind<IFacultyService>().To<FacultyService>();
        }
    }
}
