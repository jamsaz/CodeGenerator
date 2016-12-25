using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Prism.Events;
using $saferootprojectname$.Core.Services;

namespace $safeprojectname$
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CopyConfiguration();
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-ir");
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            if ((e.Exception is FileNotFoundException)) return;
            // Inform the user
            EventAggregatorService.Instance.GetEvent<PubSubEvent<object>>().Publish(new { sender, e });
            // Recover from the error
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            var bs = new Bootstrapper();
            bs.Run();
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.RequestingAssembly == null) return null;
            var filename = args.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)[0];
            var files =
                Directory.GetFiles(Configuration.CurrentPath + Configuration.Config.currentShell.dependenciesPath,
                    "*.dll");
            foreach (string file in files)
            {
                string name = file.Replace(Configuration.CurrentPath + Configuration.Config.currentShell.dependenciesPath + "\\", "").Replace(".dll", "");
                if (name == filename)
                {
                    return Assembly.LoadFile(file);
                }
            }
            return null;
        }

        private void CopyConfiguration()
        {
            string fileName = "config.json";
            string sourcePath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug", "");
            string targetPath = AppDomain.CurrentDomain.BaseDirectory;

            string sourceFile = Path.Combine(sourcePath, fileName);
            string destFile = Path.Combine(targetPath, fileName);

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            File.Copy(sourceFile, destFile, true);
        }
    }
}
