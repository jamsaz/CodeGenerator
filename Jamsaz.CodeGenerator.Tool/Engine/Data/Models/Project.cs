using System.Collections.Generic;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class Project
    {
        public string Name { get; set; }
        public IEnumerable<NameSpace> NameSpaces { get; set; }
    }
}
