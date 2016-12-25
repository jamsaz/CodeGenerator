using System;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Concrete
{
    public class MetadataProvider : IMetadataProvider
    {
        private readonly IReadableDataProvider<JArray> _metaDataReader;
        private readonly IWriteableDataProvider _metaDataWriter;

        public MetadataProvider(IReadableDataProvider<JArray> metaDataReader, IWriteableDataProvider metaDataWriter)
        {
            _metaDataReader = metaDataReader;
            _metaDataWriter = metaDataWriter;
            _metaDataReader.HasCash = true;
        }

        public JObject GetMetaData()
        {
            var data = _metaDataReader.GetDatas();
            if (data == null)
                throw new Exception("Your metadata file not exist");
            return data[0].Value<JObject>();
        }

        public void SaveMetaData(object data)
        {
            _metaDataWriter.SaveData(data);
        }

        public void UpdateSource()
        {
            _metaDataReader.HasCash = false;
            _metaDataReader.GetDatas();
            _metaDataReader.HasCash = true;
        }
    }
}