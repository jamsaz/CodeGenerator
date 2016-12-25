using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Jamsaz.CodeGenerator.Tool.Annotations;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Presentaion.Views;
using Newtonsoft.Json.Linq;
using Telerik.Windows.Controls;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels
{
    public class GenerateSettingModalViewModel : INotifyPropertyChanged
    {
        protected IConfigurationManager ConfigurationManager;

        public GenerateSettingModalViewModel(IConfigurationManager configurationManager)
        {
            ConfigurationManager = configurationManager;
            //---------------------------------------------------------
            BrowseCommand = new DelegateCommand(BrowseCommand_Execute);
            SaveCommand = new DelegateCommand(SaveCommand_Execute);
            LoadCommand = new DelegateCommand(LoadCommand_Execute);
            AddTemplateCommand = new DelegateCommand(AddTemplateCommand_Execute);
            RemoveTemplateCommand = new DelegateCommand(RemoveTemplateCommand_Execute);
            EditorCommand = new DelegateCommand(EditorCommand_Execute);
        }

        public bool IsBusy { get; set; }

        private string _pathTextBox;

        public string PathTextBox
        {
            get { return _pathTextBox; }
            set
            {
                _pathTextBox = value;
                OnPropertyChanged(nameof(PathTextBox));
            }
        }

        private ObservableCollection<TemplateItem> _items;

        public ObservableCollection<TemplateItem> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        private int _selectedIndex;

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }

        private TemplateItem _selectedItem;

        public TemplateItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand BrowseCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand LoadCommand { get; set; }
        public ICommand AddTemplateCommand { get; set; }
        public ICommand RemoveTemplateCommand { get; set; }
        public ICommand EditorCommand { get; set; }

        private void EditorCommand_Execute(object o)
        {
            if (SelectedItem != null && !string.IsNullOrEmpty(PathTextBox))
            {
                var editorDialog = new CodeEditorDialog(PathTextBox + SelectedItem.Template);
                editorDialog.ShowDialog();
            }
            else
            {
                System.Windows.MessageBox.Show("Please set template path to see code", "Warning", MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void RemoveTemplateCommand_Execute(object o)
        {
            if (
                System.Windows.MessageBox.Show("Are you sure to eliminate selected template?", "Info",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Items.RemoveAt(SelectedIndex);
            }
        }

        private void AddTemplateCommand_Execute(object o)
        {
            Items.Add(new TemplateItem());
        }

        private void LoadCommand_Execute(object o)
        {
            var configFile = ConfigurationManager.ConfigProvider.GetDatas();
            PathTextBox = configFile["Project"]["TemplatePath"].Value<string>();
            var templates = new List<TemplateItem>();
            foreach (var result in (JArray)configFile["Project"]["Templates"])
            {
                templates.Add(result.ToObject<TemplateItem>());
            }
            Items = new ObservableCollection<TemplateItem>(templates);
        }

        private void SaveCommand_Execute(object o)
        {
            var window = (Window)o;
            var config = ConfigurationManager.ConfigProvider;
            var configFile = config.GetDatas();
            configFile["Project"]["TemplatePath"] = JToken.FromObject(PathTextBox);
            configFile["Project"]["Templates"] = JToken.FromObject(Items);
            config.SaveData(configFile);
            window.DialogResult = true;
            window.Close();
        }

        private void BrowseCommand_Execute(object o)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                PathTextBox = dialog.SelectedPath;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}