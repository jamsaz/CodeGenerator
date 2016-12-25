using System.IO;
using System.Windows;
using Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.Views
{
    /// <summary>
    /// Interaction logic for CodeEditorDialog.xaml
    /// </summary>
    public partial class CodeEditorDialog : Window
    {
        public CodeEditorDialog(string filePath)
        {
            InitializeComponent();
            DataContext = new CodeEditorViewModel(filePath);
        }
    }
}
