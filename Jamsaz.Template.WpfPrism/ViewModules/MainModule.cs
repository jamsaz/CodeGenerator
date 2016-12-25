using Microsoft.Practices.Unity;
using Prism.Modularity;
using $safeprojectname$.Views;

namespace $safeprojectname$
{
    public class MainModule : IModule
    {
        private readonly IUnityContainer unityContainer;

        public MainModule(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
           
        }
    }
}
