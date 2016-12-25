using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using $safeprojectname$.Views;

namespace $safeprojectname$
{
    public class NavigationModule : IModule
    {
        private readonly IRegionManager regionManager;
        private readonly IUnityContainer unityContainer;
        public NavigationModule(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            this.regionManager = regionManager;
            this.unityContainer = unityContainer;
        }

        public void Initialize()
        {
            regionManager.RegisterViewWithRegion("NavigationMenuRegion", () => unityContainer.Resolve<PanelBar>());
        }
    }
}
