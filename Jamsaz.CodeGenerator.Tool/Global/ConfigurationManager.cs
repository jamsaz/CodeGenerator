using System.IO;
using EnvDTE;
using EnvDTE80;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Global.Helpers;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Global
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly IUnityContainer _container;

        public ConfigurationManager(IUnityContainer container)
        {
            _container = container;
        }

        public DTE CurrentDte { get; set; }
        public DTE2 CurrentDte2 { get; set; }

        public string ConfigurationFilePath { get; set; }
        public string ProjectRootPath { get; set; }

        public double CacheTime => 120.0;
        public bool ConfigurationFileIsExist => File.Exists(ConfigurationFilePath + Constants.GeneratorConfig);
        public bool MetadataFileIsExist => File.Exists(ConfigurationFilePath + Constants.Metadata);
        public bool SolutionIsOpen => CurrentDte2.Solution.IsOpen;
        public bool ProjectWasSetting
        {
            get
            {
                var config = ConfigProvider.GetDatas();
                return !string.IsNullOrEmpty(config["Project"].Value<JObject>("Data").Value<string>("Path")) &&
                       !string.IsNullOrEmpty(config["Project"].Value<JObject>("Data").Value<string>("Name"));
            }
        }
        public IConfigProvider<JObject> ConfigProvider => _container.Resolve<IConfigProvider<JObject>>();


        public string GetProjectRootPath()
        {
            if (!string.IsNullOrEmpty(ProjectRootPath)) return ProjectRootPath;
            if (!(CurrentDte?.Solution.Projects.Count > 0)) return "";
            var rootNameSpace = "";
            foreach (Project project in CurrentDte.Solution.Projects)
            {
                rootNameSpace = DteHelper.GetRootNameSpace(project);
                break;
            }
            ProjectRootPath = $"{CurrentDte.Solution.FileName.Replace($"{rootNameSpace}.sln", "")}{rootNameSpace}\\";
            return ProjectRootPath;
        }

        public void GetConfigurationFilePath()
        {
            if (!string.IsNullOrEmpty(ConfigurationFilePath)) return;
            if (!(CurrentDte?.Solution.Projects.Count > 0)) return;
            var rootNameSpace = "";
            foreach (Project project in CurrentDte.Solution.Projects)
            {
                rootNameSpace = DteHelper.GetRootNameSpace(project);
                break;
            }
            ConfigurationFilePath =
                $"{CurrentDte.Solution.FileName.Replace($"{rootNameSpace}.sln", "")}{rootNameSpace}\\{rootNameSpace}\\";
            ProjectRootPath =
                $"{CurrentDte.Solution.FileName.Replace($"{rootNameSpace}.sln", "")}{rootNameSpace}\\";
        }

    }
}
