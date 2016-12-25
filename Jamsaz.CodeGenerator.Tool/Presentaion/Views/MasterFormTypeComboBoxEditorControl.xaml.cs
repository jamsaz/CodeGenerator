using System.IO;
using System.Reflection;
using System.Windows;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels;
using Newtonsoft.Json;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.Views
{
    /// <summary>
    /// Interaction logic for GenerateTypeComboBoxEditorControl.xaml
    /// </summary>
    public partial class MasterFormTypeComboBoxEditorControl : System.Windows.Controls.UserControl
    {
        public MasterFormTypeComboBoxEditorControl()
        {
            InitializeComponent();
            DataContext = this;
            LoadComboBoxItems();
        }

        private void LoadComboBoxItems()
        {
            //var directoryName = ConfigurationManager.ConfigurationFilePath;
            //if (directoryName != null)
            //{
            //    dynamic json =
            //        JsonConvert.DeserializeObject(
            //            File.OpenText(directoryName + Constants.GeneratorConfig).ReadToEnd());
            //    if (json.Project.MasterFormTemplateGenerateType != null)
            //    {
            //        foreach (var t in json.Project.MasterFormTemplateGenerateType)
            //        {
            //            ComboBox.Items.Add(new MasterFormTypes(t.Name.ToString()));
            //        }
            //    }
            //}
            return;

        }

        public object Type { get; set; }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem", typeof(object), typeof(MasterFormTypeComboBoxEditorControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((MasterFormTypeComboBoxEditorControl) sender).SelectedItem = (e.NewValue);
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
    }
}
