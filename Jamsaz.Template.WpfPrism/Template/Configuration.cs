using System.IO;
using System.Reflection;
using System.Windows;
using Newtonsoft.Json;

namespace $safeprojectname$
{
    public static class Configuration
    {
        public static readonly string CurrentPath;
        public static readonly dynamic Config;
        public static readonly Assembly CurrentShellAssembly;

        static Configuration()
        {
            CurrentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!File.Exists(CurrentPath + @"\config.json")) MessageBox.Show("config file missing");
            Config = JsonConvert.DeserializeObject(File.ReadAllText(CurrentPath + @"\config.json"));
            CurrentShellAssembly = Assembly.LoadFile(CurrentPath + Config.currentShell.path);
        }
    }
}
