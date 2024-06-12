using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCraft.Common.Databases.Abstractions.Exceptions
{
    public abstract class BaseDatabaseException: Exception
    {
        protected BaseDatabaseException(string message): base(message)
        {
        }

        protected BaseDatabaseException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
