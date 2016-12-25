using Jamsaz.CodeGenerator.Tool.Engine;
using Jamsaz.CodeGenerator.Tool.Engine.Generator;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.Views
{
    /// <summary>
    /// Interaction logic for CodeGeneratorControl.xaml
    /// </summary>
    public partial class CodeGeneratorControl : System.Windows.Controls.UserControl
    {
        public CodeGeneratorControl(ICodeManager codeManager, IConfigurationManager configurationManager, ITemplateGenerator generator)
        {
            InitializeComponent();
            DataContext = new ProjectViewModel(codeManager, configurationManager, generator);
        }

        
    }
}
