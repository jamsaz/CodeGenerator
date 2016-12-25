using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts
{
    public interface IMetadataProvider
    {
        JObject GetMetaData();
        void SaveMetaData(object data);
        void UpdateSource();
    }
}