using System.Windows;
using $saferootprojectname$.Core.Controls.Menues;

namespace $safeprojectname$.Views
{
    /// <summary>
    /// Interaction logic for PanelBar.xaml
    /// </summary>
    public partial class PanelBar : Menu
    {
        public PanelBar()
        {
            InitializeComponent();
            SetResourceReference(StyleProperty, typeof(Menu));
            ItemTemplate = (DataTemplate)Resources["ItemTemplate"];
        }
    }
}
