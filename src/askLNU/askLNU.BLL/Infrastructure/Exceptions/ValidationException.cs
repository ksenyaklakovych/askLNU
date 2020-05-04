using System;
using System.Collections.Generic;
using System.Text;

namespace askLNU.BLL.Infrastructure.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public string Property { get; protected set; }
       
        public ValidationException(string message, string prop) : base(message)
        {
            Property = prop;
        }
    }
}