using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Global;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Concrete
{
    public class DataProvider
    {
        private readonly IConfigurationManager _configurationManager;
        public DataProvider(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public IDataProvider GetDataProvider()
        {
            var config = _configurationManager.ConfigProvider.GetDatas();
            switch (config["Project"].Value<JObject>("Data").Value<int>("Type"))
            {
                case (int)DataSources.MySql:
                    return new MySqlDataProvider(_configurationManager);
                case (int)DataSources.Json:
                    return new JsonDataProvider();
                case (int)DataSources.Access:
                    return new AccessDataProvider(_configurationManager);
                case (int)DataSources.Oracle:
                    return new OracleDataProvider(_configurationManager);
                case (int)DataSources.Csv:
                    return new CsvDataProvider(_configurationManager);
                default:
                    return new SqlServerDataProvider(_configurationManager);
            }
        }
    }
}