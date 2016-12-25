using System;
using System.Collections.Generic;
using System.IO;
using Jamsaz.CodeGenerator.Tool.Global;
using Mustache;

namespace Jamsaz.CodeGenerator.Tool.Engine.TemplateEngines.Mustache.Tags
{
    public class CallTagDefinition : InlineTagDefinition
    {
        public CallTagDefinition()
            : base("call")
        {
        }

        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new[] { new TagParameter("type"), new TagParameter("prop"), };
        }

        public override void GetText(TextWriter writer, Dictionary<string, object> arguments, Scope context)
        {
            var type = Convert.ToString(arguments["type"]);
            var prop = Convert.ToString(arguments["prop"]);
            var output = "";
            switch (type)
            {
                case "Capitalize":
                    output = Inflector.Inflector.Capitalize(prop);
                    break;
                case "LowercaseFirst":
                    output = prop.LowercaseFirst();
                    break;

            }
            writer.Write(output);
        }
    }
}
