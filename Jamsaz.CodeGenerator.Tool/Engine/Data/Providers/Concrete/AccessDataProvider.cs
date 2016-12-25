using System.Linq;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Global;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Concrete
{
    public class AccessDataProvider : IReadableDataProvider<IQueryable<Models.Data>>, IDataProvider
    {
        private JObject _config;
        public AccessDataProvider(IConfigurationManager configurationManager)
        {
            _config = configurationManager.ConfigProvider.GetDatas();
        }

        public bool HasCash { get; set; }

        public IQueryable<Models.Data> GetDatas()
        {
            throw new System.NotImplementedException();
        }
    }
}