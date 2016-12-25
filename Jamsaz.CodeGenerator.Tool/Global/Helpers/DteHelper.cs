using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using EnvDTE;
using EnvDTE80;
using Process = System.Diagnostics.Process;

namespace Jamsaz.CodeGenerator.Tool.Global.Helpers
{
    public class DteHelper
    {
        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);
        [DllImport("ole32.dll")]
        private static extern void GetRunningObjectTable(int reserved,
                                                         out IRunningObjectTable prot);
        public static DTE GetCurrent()
        {
            string rotEntry = $"!VisualStudio.DTE.14.0:{Process.GetCurrentProcess().Id}";
            IRunningObjectTable rot;
            GetRunningObjectTable(0, out rot);
            IEnumMoniker enumMoniker;
            rot.EnumRunning(out enumMoniker);
            enumMoniker.Reset();
            IntPtr fetched = IntPtr.Zero;
            IMoniker[] moniker = new IMoniker[1];
            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                CreateBindCtx(0, out bindCtx);
                string displayName;
                moniker[0].GetDisplayName(bindCtx, null, out displayName);
                if (displayName == rotEntry)
                {
                    object comObject;
                    rot.GetObject(moniker[0], out comObject);
                    return (DTE)comObject;
                }
            }
            return null;
        }

        public static DTE2 GetCurrent2()
        {
            string rotEntry = $"!VisualStudio.DTE.14.0:{Process.GetCurrentProcess().Id}";
            IRunningObjectTable rot;
            GetRunningObjectTable(0, out rot);
            IEnumMoniker enumMoniker;
            rot.EnumRunning(out enumMoniker);
            enumMoniker.Reset();
            IntPtr fetched = IntPtr.Zero;
            IMoniker[] moniker = new IMoniker[1];
            while (enumMoniker.Next(1, moniker, fetched) == 0)
            {
                IBindCtx bindCtx;
                CreateBindCtx(0, out bindCtx);
                string displayName;
                moniker[0].GetDisplayName(bindCtx, null, out displayName);
                if (displayName == rotEntry)
                {
                    object comObject;
                    rot.GetObject(moniker[0], out comObject);
                    return (DTE2)comObject;
                }
            }
            return null;
        }

        public static bool HasProject(Solution2 solution, string projectName)
        {
            foreach (Project project in solution.Projects)
            {
                if (project.Name.Equals(projectName))
                    return true;
            }
            return false;
        }

        private void IterateProjects()
        {
            Projects projects = GetCurrent().Solution.Projects;
            if (projects.Count > 0)
            {
                List<Project> list = new List<Project>(projects.Count);
                foreach (Project p in projects)
                {
                    if (HasProperty(p.Properties, ("FullPath")))
                        //ignore installer projects
                        list.Add(p);
                }
            }
        }

        private bool HasProperty(EnvDTE.Properties properties, string propertyName)
        {
            if (properties != null)
            {
                foreach (Property item in properties)
                {
                    if (item != null && item.Name == propertyName)
                        return true;
                }
            }
            return false;
        }

        public static string GetRootNameSpace(Project project)
        {
            try
            {
                return project.Properties.Item("RootNamespace").Value.ToString();
            }
            catch
            {
                return "";
            }
        }

        public static string GetDirectory(Project project)
        {
            return project.Properties.Item("FullPath").Value.ToString();
        }
    }
}
