using System;
using System.Reflection;
using System.Reflection.Emit;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Engine.Generator;
using Jamsaz.CodeGenerator.Tool.Global;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine
{
    public class CodeManager : ICodeManager
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IConfigProvider<JObject> _configDataProvider;
        private readonly IReadableDataProvider<JArray> _metaDataReader;
        private readonly IWriteableDataProvider _metaDataWritter;

        private readonly ObjectGenerator _objectGenerator;
        private readonly IMetadataGenerator _metadataGenerator;
        private readonly Type _currenType;

        public CodeManager(IConfigurationManager configurationManager, IReadableDataProvider<JArray> metaDataReader, IMetadataGenerator metadataGenerator, IWriteableDataProvider metaDataWritter)
        {
            _configurationManager = configurationManager;
            _configDataProvider = configurationManager.ConfigProvider;
            _metaDataReader = metaDataReader;
            _metadataGenerator = metadataGenerator;
            _metaDataWritter = metaDataWritter;

            //--------------------------------------------------------------------------
            var config = _configDataProvider.GetDatas();
            var patternId = config["Project"].Value<int>("Type");
            var patternObject = config["Project"].Value<JArray>("Patterns")
                .SingleOrDefault(x => x["Id"].Value<int>() == patternId).Value<JObject>();

            var typeGenerator = new TypeGenerator(
                AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("PatternRunTimeAssembly"),
                    AssemblyBuilderAccess.Run), patternObject);
            _currenType = typeGenerator.Generate();
            _objectGenerator = new ObjectGenerator(typeGenerator);
        }

        public object CodeObjects { get; set; }

        public bool InitializeProject()
        {
            if (!_configurationManager.ConfigurationFileIsExist) return false;
            var config = _configDataProvider.GetDatas();
            CodeObjects = config["Project"].Value<JObject>("Data").Value<string>("Type").Equals("-1")
                ? CreateStaticObject()
                : CreateObjectFromDataSource();
            return true;
        }

        public bool RealoadProject()
        {
            _metadataGenerator.Generate();
            return InitializeProject();
        }

        public void SaveProjectSetting(object newObjects)
        {
            _metaDataWritter.SaveData(newObjects);
        }

        private object CreateObjectFromDataSource()
        {
            JArray jsonObjects;
            if (_configurationManager.MetadataFileIsExist)
                jsonObjects = _metaDataReader.GetDatas();
            else
            {
                _metadataGenerator.Generate();
                jsonObjects = _metaDataReader.GetDatas();
            }
            return _objectGenerator.CreateInstanceWithData(_currenType, jsonObjects);
        }

        private object CreateStaticObject()
        {
            return _objectGenerator.CreateInstance(_currenType);
        }

    }
}