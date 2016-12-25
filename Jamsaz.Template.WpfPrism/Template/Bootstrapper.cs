using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using $saferootprojectname$.Core.Services;

namespace $safeprojectname$
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            Type shell = Configuration.CurrentShellAssembly.ExportedTypes.FirstOrDefault(x => x.Name.Equals(Configuration.Config.currentShell.name.ToString()));
            if (shell != null)
            {
                EventAggregatorService.SetEventAggregator(Container.TryResolve<IEventAggregator>());
                return (DependencyObject)Container.TryResolve(shell);
            }
            return null;
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog { ModulePath = Configuration.CurrentPath + Configuration.Config.currentShell.modulePath };
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mapper = base.ConfigureRegionAdapterMappings();
            foreach (var adapter in Configuration.Config.currentShell.adapters)
            {
                if (adapter.control.assemblyPath == null) continue;
                Assembly controlAssembly = Assembly.LoadFile(Configuration.CurrentPath + adapter.control.assemblyPath);
                var controlType =
                    controlAssembly.ExportedTypes.FirstOrDefault(x => x.Name.Equals(adapter.control.type.ToString()));
                var adapterType =
                    Configuration.CurrentShellAssembly.ExportedTypes.FirstOrDefault(x => x.Name.Equals(adapter.name.ToString()));
                mapper.RegisterMapping(controlType, (IRegionAdapter)Container.TryResolve(adapterType));
            }
            return mapper;
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var behaviors = base.ConfigureDefaultRegionBehaviors();
            foreach (var behavior in Configuration.Config.currentShell.behaviors)
            {
                if (behavior.assemblyPath == "")
                {
                    var behaviorType = Configuration.CurrentShellAssembly.ExportedTypes.FirstOrDefault(x => x.Name.Equals(behavior.key.ToString()));
                    if (behaviorType != null)
                    {
                        behaviors.AddIfMissing(behavior.key.ToString(), behaviorType);
                    }
                }
                else
                {
                    //TODO: Behaviors Within Path
                }
            }
            return behaviors;
        }

    }
}
