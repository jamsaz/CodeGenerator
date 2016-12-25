using System.Linq;
using System.Windows;
using Prism.Regions;
using Telerik.Windows.Controls.Docking;

namespace $safeprojectname$
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IRegionManager regionManager;
        public MainWindow(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            InitializeComponent();
        }

        private void RadDocking_OnPreviewClose(object sender, StateChangeEventArgs e)
        {
            var currentPane = e.Panes.FirstOrDefault(x => x.IsActive);
            var region = regionManager.Regions["FormRegion"];
            var currentView = region.Views.Cast<dynamic>().FirstOrDefault(x => currentPane != null && x.Name == currentPane.Name);
            if (currentView != null)
                region.Remove(currentView);
        }
    }
}
