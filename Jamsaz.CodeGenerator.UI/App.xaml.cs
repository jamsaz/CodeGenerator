using System.IO;
using System.Reflection;
using System.Windows;
using Jamsaz.CodeGenerator.Tool.Global;
using Microsoft.Practices.Unity;

namespace Jamsaz.CodeGenerator.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName != null)
            {
                //ConfigurationManager.ConfigurationFilePath = directoryName.Replace(@"\bin\Debug", @"\");
                var container = new UnityContainer();
                //container.RegisterType<IMetadataGenerator, Metadata>();
                //container.RegisterType<IGenerator, MvvmPrismGenerator>();
                Current.MainWindow = container.Resolve<MainWindow>();
            }
            else
            {
                Current.MainWindow.Close();
                return;
            }
            base.OnStartup(e);
        }
    }
}
