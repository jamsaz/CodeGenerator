using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models.Contextes
{
    public interface IObjectContext
    {
        List<object> GetCompositeObjects(string id);
        object GetObjects(JToken item);
        object GetAllObjects(JToken item);
        object GetDataContextObjects(JToken item);
    }
}