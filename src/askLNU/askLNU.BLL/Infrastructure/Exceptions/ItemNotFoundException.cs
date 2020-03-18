using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Infrastructure.Exceptions
{
    public class ItemNotFoundException : ApplicationException
    {
        public ItemNotFoundException(string message)
            : base(message)
        { }
    }
}
