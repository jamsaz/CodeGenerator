using System.Collections.Generic;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class NameSpace
    {
        public string Name { get; set; }
        public string SchemaName { get; set; }
        public string DisplayName { get; set; }
        public string ModuleName { get; set; }
        public int ModuleId { get; set; }
        public IEnumerable<Object> Objects { get; set; }
    }
}
