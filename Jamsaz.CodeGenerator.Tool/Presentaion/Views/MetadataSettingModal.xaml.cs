using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Jamsaz.CodeGenerator.Tool.Engine;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Global.Helpers;
using Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels;
using Newtonsoft.Json.Linq;
using Telerik.Windows.Controls;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.Views
{
    public partial class MetadataSettingModal : Window
    {
        public MetadataSettingModal(ICodeManager codeManager, IConfigurationManager configurationManager)
        {
            InitializeComponent();
            DataContext = new MetadataSettingViewModel(codeManager, configurationManager);
        }
    }
}
