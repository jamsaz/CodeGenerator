using EnvDTE;
using EnvDTE80;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Global
{
    public interface IConfigurationManager
    {
        DTE CurrentDte { get; set; }
        DTE2 CurrentDte2 { get; set; }

        string ConfigurationFilePath { get; set; }
        string ProjectRootPath { get; set; }
        double CacheTime { get; }

        bool SolutionIsOpen { get; }
        bool ConfigurationFileIsExist { get; }
        bool MetadataFileIsExist { get; }
        bool ProjectWasSetting { get; }
        IConfigProvider<JObject> ConfigProvider { get; }

        string GetProjectRootPath();
        void GetConfigurationFilePath();


    }
}