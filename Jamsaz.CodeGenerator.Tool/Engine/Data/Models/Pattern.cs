using System.Collections.Generic;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class Pattern
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<PatternDefinition> Definitaion { get; set; }
    }
}
