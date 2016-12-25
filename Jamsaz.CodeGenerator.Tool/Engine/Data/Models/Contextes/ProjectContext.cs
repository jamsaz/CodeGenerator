using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EnvDTE80;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Global.Helpers;
using Jamsaz.CodeGenerator.Tool.Properties;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models.Contextes
{
    public class ProjectContext
    {
        private readonly JObject _config;
        private readonly IMetadataProvider _metaDataProvider;
        private readonly IObjectContext _objectContext;
        private readonly IConfigurationManager _configurationManager;
        
        public ProjectContext(IMetadataProvider metaDataProvider,
            IObjectContext objectContext, IConfigurationManager configurationManager)
        {
            _objectContext = objectContext;
            _configurationManager = configurationManager;
            _config = configurationManager.ConfigProvider.GetDatas();
            _metaDataProvider = metaDataProvider;
        }

        public object GenerateModuleProject(JToken item)
        {
            try
            {
                var project = _metaDataProvider.GetMetaData();
                var dte = _configurationManager.CurrentDte2;
                if (dte == null) return new object();
                var solution = (Solution2)dte.Solution;
                var templatePath = solution.GetProjectTemplate(_config["Project"]["ViewsModuleProjectTemplateName"].Value<string>(), "CSharp");
                var objects = (IEnumerable<object>)_objectContext.GetObjects(item);
                var modules = new List<object>();
                modules.AddRange(project.GetPropertyValueAsEnumerate("NameSpaces")
                    .Where(
                        n =>
                            n.GetPropertyValueAsEnumerate("Objects")
                                .Any(
                                    o =>
                                        o.GetPropertyValueAs<int>("ParentModule") ==
                                        n.GetPropertyValueAs<int>("ModuleId"))).Select(n => new
                                        {
                                            ProjectName = project.GetPropertyValueAsString("Name"),
                                            Name = n.GetPropertyValueAsString("Name"),
                                            DisplayName = n.GetPropertyValueAsString("DisplayName"),
                                            CapitalizedNameSpace = n.GetPropertyValueAsString("Name").Capitalize(),
                                            ModuleName = n.GetPropertyValueAsString("ModuleName"),
                                            CapitalizedName = n.GetPropertyValueAsString("Name").Capitalize(),
                                            Objects = objects.Where(o =>
                                                o.GetPropertyValueAs<int>("ParentModule") ==
                                                n.GetPropertyValueAs<int>("ModuleId"))
                                        }));
                foreach (var module in modules)
                {
                    var projectPath =
                        $"{_configurationManager.ProjectRootPath}{project.GetPropertyValueAsString("Name")}{_config["Project"]["ViewsModuleProjectPath"].Value<string>()}{module.GetPropertyValueAsString("ModuleName").Capitalize()}";
                    var projectName =
                        $"{project.GetPropertyValueAsString("Name")}{_config["Project"]["ViewsModuleProjectPath"].Value<string>()}{module.GetPropertyValueAsString("ModuleName").Capitalize()}";
                    if (!DteHelper.HasProject(solution, projectName))
                        solution.AddFromTemplate(templatePath, projectPath, projectName);
                }
                return modules;
            }
            catch (Exception e)
            {
                MessageBox.Show(string.Format(Resources.Error_Message___0__Stack_Trace___1_, e.Message, e.StackTrace));
                return new object();
            }
        }

    }
}
