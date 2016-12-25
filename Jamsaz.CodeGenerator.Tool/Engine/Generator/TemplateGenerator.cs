using System;
using System.Collections.Generic;
using System.IO;
using Mustache;
using Newtonsoft.Json.Linq;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Models.Contextes;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Engine.TemplateEngines.Mustache.Tags;
using Jamsaz.CodeGenerator.Tool.Global.Helpers;


namespace Jamsaz.CodeGenerator.Tool.Engine.Generator
{
    public class TemplateGenerator : ITemplateGenerator
    {
        private readonly JObject _config;
        private readonly IMetadataProvider _metaDataProvider;
        private readonly IObjectContext _objectContext;
        private readonly IConfigurationManager _configurationManager;
        private readonly NameSpaceContext _nameSpaceContext;
        private readonly ProjectContext _projectContext;
        private FormatCompiler _compiler;

        public TemplateGenerator(IMetadataProvider metaDataProvider, IObjectContext objectContext,
            IConfigurationManager configurationManager)
        {
            _objectContext = objectContext;
            _configurationManager = configurationManager;
            _config = configurationManager.ConfigProvider.GetDatas();
            _metaDataProvider = metaDataProvider;
            _nameSpaceContext = new NameSpaceContext(_metaDataProvider, _objectContext);
            _projectContext = new ProjectContext(_metaDataProvider, _objectContext, _configurationManager);
        }

        public void Generate()
        {
            _metaDataProvider.UpdateSource();
            foreach (var item in _config["Project"].Value<JArray>("Templates"))
            {
                var template =
                    $"{_config["Project"].Value<string>("TemplatePath")}{item["Template"].Value<string>()}";
                if (item["Path"].Value<string>().Contains("@"))
                {
                    var obj = (List<object>)SelectObjectMethod(item["Object"].Value<string>(), item);
                    GenerateTemplateWithDynamicPath(item, _configurationManager.ProjectRootPath, template, obj);
                }
                else
                {
                    var realProjectPath =
                         $"{_configurationManager.ProjectRootPath}{item["Path"].Value<string>()}";
                    if (!Directory.Exists(realProjectPath))
                        Directory.CreateDirectory(realProjectPath);
                    GenerateTemplateWithFixedPath(item, realProjectPath, template);
                }
            }
        }

        private void GenerateTemplateWithFixedPath(JToken item, string currentProjectPath, string templatePath)
        {
            if (item["Type"].Value<string>() == "Multiple")
            {
                var obj = (List<object>)SelectObjectMethod(item["Object"].Value<string>(), item);
                foreach (var o in obj)
                {
                    if (item["CreateFolderForNamespace"].Value<bool>())
                    {
                        var p = $"{currentProjectPath}{o.GetPropertyValueAsString("CapitalizedNameSpace")}";
                        if (!Directory.Exists(p))
                            Directory.CreateDirectory(p);
                        if (item["Name"].Value<string>().Contains("@"))
                        {
                            var fileName = item["Name"].Value<string>()
                                .Replace("@", o.GetPropertyValueAsString("Name").Capitalize());
                            File.WriteAllText(
                                $"{p}\\{fileName}.{item["Extension"].Value<string>()}",
                                GenerateTemplate(templatePath, o));
                        }
                        else
                        {
                            File.WriteAllText(
                                $"{p}\\{o.GetPropertyValueAsString("Name").Capitalize()}.{item["Extension"].Value<string>()}",
                                GenerateTemplate(templatePath, o));
                        }
                    }
                    else
                    {
                        File.WriteAllText(
                            $"{currentProjectPath}\\{o.GetPropertyValueAsString("Name")}.{item["Extension"].Value<string>()}",
                            GenerateTemplate(templatePath, o));
                    }
                }
            }
            else
            {
                var o = string.IsNullOrEmpty(item["Object"].Value<string>())
                    ? _metaDataProvider.GetMetaData()
                    : SelectObjectMethod(item["Object"].Value<string>(), item);
                File.WriteAllText(
                    $"{currentProjectPath}\\{item["Name"].Value<string>()}.{item["Extension"].Value<string>()}",
                    GenerateTemplate(templatePath, o));
            }
        }
        private void GenerateTemplateWithDynamicPath(JToken item, string currentProjectPath, string templatePath, object currentObject)
        {
            if (item["Type"].Value<string>() == "Multiple")
            {
                var obj = (List<object>)currentObject;
                foreach (var o in obj)
                {
                    var p =
                        $"{currentProjectPath}{item["Path"].Value<string>().Replace("@", o.GetPropertyValueAsString("CapitalizedNameSpace").Capitalize())}";
                    if (!Directory.Exists(p))
                        Directory.CreateDirectory(p);
                    if (item["Name"].Value<string>().Contains("@"))
                    {
                        var fileName = item["Name"].Value<string>()
                            .Replace("@", o.GetPropertyValueAsString("Name").Capitalize());
                        File.WriteAllText(
                            $"{p}\\{fileName}.{item["Extension"].Value<string>()}",
                            GenerateTemplate(templatePath, o));
                    }
                    else
                    {
                        File.WriteAllText(
                            $"{p}\\{o.GetPropertyValueAsString("Name").Capitalize()}.{item["Extension"].Value<string>()}",
                            GenerateTemplate(templatePath, o));
                    }
                }
            }
            else
            {
                var o = string.IsNullOrEmpty(item["Object"].Value<string>())
                    ? _metaDataProvider.GetMetaData()
                    : SelectObjectMethod(item["Object"].Value<string>(), item);
                var p =
                    $"{currentProjectPath}{item["Path"].Value<string>().Replace("@", o.GetPropertyValueAsString("Name").Capitalize())}";
                if (!Directory.Exists(p))
                    Directory.CreateDirectory(p);

                File.WriteAllText(
                    $"{p}\\{item["Name"].Value<string>()}.{item["Extension"].Value<string>()}",
                    GenerateTemplate(templatePath, o));
            }
        }
        private object SelectObjectMethod(string op, JToken item)
        {
            switch (op)
            {
                case "GetNameSpaces":
                    return _nameSpaceContext.GetNameSpaces(item);
                case "GetNameSpacesForMenu":
                    return _nameSpaceContext.GetNameSpacesForMenu(item);
                case "GetObjects":
                    return _objectContext.GetObjects(item);
                case "GetAllObjects":
                    return _objectContext.GetAllObjects(item);
                case "GetDataContextObjects":
                    return _objectContext.GetDataContextObjects(item);
                case "GenerateModuleProject":
                    return _projectContext.GenerateModuleProject(item);
                default:
                    var runtimeObjectGenerator = new RuntimeObjectGenerator(_configurationManager);
                    return runtimeObjectGenerator.Generate(op);
            }
        }
        private string GenerateTemplate(string template, dynamic obj, bool useDefaultTag = true)
        {
            if (useDefaultTag)
            {
                _compiler = new FormatCompiler();
                _compiler.RegisterTag(new CallTagDefinition(), true);
            }
            else
            {
                _compiler = new FormatCompiler()
                {
                    OpenTagDelimeter = "<%",
                    CloseTagDelimeter = "%>"
                };
            }

            var generator = _compiler.Compile(File.ReadAllText(template));
            string code = generator.Render(obj);
            return code;
        }
    }
}