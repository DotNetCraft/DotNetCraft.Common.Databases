using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCraft.Common.Databases.Abstractions.Options
{
    public class SearchDefinition
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
    }
}
