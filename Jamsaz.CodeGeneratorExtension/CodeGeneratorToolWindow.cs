//------------------------------------------------------------------------------
// <copyright file="CodeGeneratorToolWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Jamsaz.CodeGenerator.Tool.Engine;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Models.Contextes;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Concrete;
using Jamsaz.CodeGenerator.Tool.Engine.Generator;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Global.Helpers;
using Jamsaz.CodeGenerator.Tool.Presentaion.Views;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGeneratorExtension
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("582d730a-cffe-41e5-9eff-21a5725565f6")]
    public class CodeGeneratorToolWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeGeneratorToolWindow"/> class.
        /// </summary>
        public CodeGeneratorToolWindow() : base(null)
        {
            //Caption = "JamsazCodeGenerator";
            //Content = new CodeGeneratorControl();
            ConfigurationContainer();
        }

        private void ConfigurationContainer()
        {
            var container = new UnityContainer();
            var instances = Config(container);
            var metaDataDataReader =
                new JsonDataProvider<JArray>(instances.Item1.ConfigurationFilePath + Constants.Metadata);
            var metaDataWriter = new JsonDataProvider(instances.Item1.ConfigurationFilePath + Constants.Metadata);
            container.RegisterType<IConfigProvider<JObject>, JsonConfigProvider<JObject>>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(instances.Item1.ConfigurationFilePath + Constants.GeneratorConfig));
            container.RegisterType<IMetadataGenerator, MetadataGenerator>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new JsonDataProvider(instances.Item1.ConfigurationFilePath + Constants.Metadata), instances.Item1));
            container.RegisterType<ICodeManager, CodeManager>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(instances.Item1, metaDataDataReader, container.Resolve<IMetadataGenerator>(),
                    metaDataWriter));
            container.RegisterType<IConfigurationManager, ConfigurationManager>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMetadataProvider, MetadataProvider>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(metaDataDataReader, metaDataWriter));
            var metaDataProvider = container.Resolve<IMetadataProvider>();
            var objectContext = new ObjectContext(metaDataProvider);
            container.RegisterType<ITemplateGenerator, TemplateGenerator>(new ContainerControlledLifetimeManager(),
                new InjectionConstructor(metaDataProvider, objectContext, instances.Item1));

            Caption = "JamsazCodeGenerator";
            Content = container.Resolve<CodeGeneratorControl>(new ParameterOverride("configurationManager", instances.Item1));
        }

        private static Tuple<IConfigurationManager, IConfigProvider<JObject>> Config(UnityContainer container)
        {
            var dte = DteHelper.GetCurrent();
            var configurationManager = new ConfigurationManager(container)
            {
                CurrentDte = dte,
                CurrentDte2 = DteHelper.GetCurrent2(),
                ConfigurationFilePath = ""
            };
            configurationManager.GetConfigurationFilePath();
            var configFileProvider =
                new JsonConfigProvider<JObject>(configurationManager.ConfigurationFilePath +
                                                Constants.GeneratorConfig);
            return new Tuple<IConfigurationManager, IConfigProvider<JObject>>(configurationManager,
                configFileProvider);
        }
    }
}
