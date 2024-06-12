using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCraft.Common.Databases.Abstractions.Exceptions
{
    public class ReadDatabaseException: BaseDatabaseException
    {
        public ReadDatabaseException(string message): base(message)
        {
        }

        public ReadDatabaseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
