using System.Windows;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.Views
{
    /// <summary>
    /// Interaction logic for MetadataSettingModal.xaml
    /// </summary>
    public partial class GenerateSettingModal : Window
    {
        public GenerateSettingModal(IConfigurationManager configurationManager)
        {
            InitializeComponent();
            DataContext = new GenerateSettingModalViewModel(configurationManager);
        }
    }
}
