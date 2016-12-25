using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Jamsaz.CodeGenerator.Tool.Annotations;
using Jamsaz.CodeGenerator.Tool.Engine;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Global.Helpers;
using Newtonsoft.Json.Linq;
using Telerik.Windows.Controls;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels
{
    public class MetadataSettingViewModel : INotifyPropertyChanged
    {
        #region Definations

        protected ICodeManager CodeManager;
        protected IConfigurationManager ConfigurationManager;
        private ObservableCollection<object> _metaData;
        private readonly SynchronizationContext _synchronizationContext;
        private bool _isCancel;

        public object Metadata;

        #endregion

        #region Ctor

        public MetadataSettingViewModel(ICodeManager codeManager, IConfigurationManager configurationManager)
        {
            CodeManager = codeManager;
            ConfigurationManager = configurationManager;
            _metaData = new ObservableCollection<object>();
            _synchronizationContext = SynchronizationContext.Current;
            _isCancel = false;
            //---------------------------------------------------------
            LoadCommand = new DelegateCommand(LoadCommand_Execute);
            SchemaChangeCommand = new DelegateCommand(SchemaChangeCommand_Execute);
            SaveCommand = new DelegateCommand(SaveCommand_Execute);
            CancelCommand = new DelegateCommand(CancelCommand_Execute);
            ClosedCommand = new DelegateCommand(CloseCommand_Execute);
        }

        #endregion

        #region Properties

        private string _serverText;
        public string ServerText
        {
            get { return _serverText; }
            set
            {
                _serverText = value;
                OnPropertyChanged(nameof(ServerText));
            }
        }

        private string _dataSourceName;
        public string DataSourceName
        {
            get { return _dataSourceName; }
            set
            {
                _dataSourceName = value;
                OnPropertyChanged(nameof(DataSourceName));
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private object _dataTypeSelectedItem;
        public object DataTypeSelectedItem
        {
            get { return _dataTypeSelectedItem; }
            set
            {
                _dataTypeSelectedItem = value;
                OnPropertyChanged(nameof(DataTypeSelectedItem));
            }
        }

        private object _projectTypeSelectedItem;
        public object ProjectTypeSelectedItem
        {
            get { return _projectTypeSelectedItem; }
            set
            {
                _projectTypeSelectedItem = value;
                OnPropertyChanged(nameof(ProjectTypeSelectedItem));
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy == value)
                    return;
                _isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        private ObservableCollection<SettingListView> _schemaList;
        public ObservableCollection<SettingListView> SchemaList
        {
            get { return _schemaList; }
            set
            {
                _schemaList = value;
                OnPropertyChanged(nameof(SchemaList));
            }
        }

        private ObservableCollection<SettingListView> _objectList;
        public ObservableCollection<SettingListView> ObjectList
        {
            get { return _objectList; }
            set
            {
                _objectList = value;
                OnPropertyChanged(nameof(ObjectList));
            }
        }

        #endregion

        #region Events

        private void CancelCommand_Execute(object o)
        {
            if (MessageBox.Show(
                "Are you soure to cancel setting operation? if you cancel that metatdata load as default!",
                "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No) return;
            _isCancel = true;
            var window = o as Window;
            window?.Close();
        }

        private void SaveCommand_Execute(object o)
        {
            SaveCurrentSetting(() =>
            {
                if (SchemaList != null)
                {
                    foreach (var item in SchemaList)
                    {
                        if (!item.IsSelected)
                        {
                            _metaData.First()
                                .GetPropertyValueAs<IList>("NameSpaces")
                                .Remove(
                                    _metaData.First()
                                        .GetPropertyValueAsEnumerate("NameSpaces")
                                        .Single(x => x.GetPropertyValueAsString("Name") == item.Name));
                        }
                        else if (item.IsSelected && item.Childs.Any(x => x.IsSelected))
                        {
                            foreach (var child in item.Childs.Where(x => !x.IsSelected))
                            {
                                _metaData.First().GetPropertyValueAsEnumerate("NameSpaces")
                                    .Single(x => x.GetPropertyValueAsString("Name") == item.Name)
                                    .GetPropertyValueAs<IList>("Objects")
                                    .Remove(_metaData.First().GetPropertyValueAsEnumerate("NameSpaces")
                                        .Single(x => x.GetPropertyValueAsString("Name") == item.Name)
                                        .GetPropertyValueAsEnumerate("Objects")
                                        .Single(x => x.GetPropertyValueAs<int>("Id") == child.Id));
                            }
                        }
                    }
                }
                var window = o as Window;
                if (window == null) return;
                window.DialogResult = true;
                Metadata = _metaData;
                window.Close();
            });
        }

        private void SchemaChangeCommand_Execute(object sender)
        {
            var listBox = sender as RadListBox;
            var selectedItem = listBox?.SelectedItem as SettingListView;
            if (selectedItem != null) ObjectList = selectedItem.Childs;
        }

        private void LoadCommand_Execute(object p)
        {
            if (string.IsNullOrEmpty(ServerText) || string.IsNullOrEmpty(DataSourceName))
            {
                MessageBox.Show("Please Enter ServerName and DataSource!");
                return;
            }
            try
            {
                SaveCurrentSetting(() =>
                {
                    IsBusy = true;
                    Task.Run(() =>
                    {
                        CodeManager.RealoadProject();
                        _metaData = new ObservableCollection<object> { CodeManager.CodeObjects };
                    }).ContinueWith(r =>
                    {
                        if (r.IsCompleted)
                        {
                            _synchronizationContext.Send(ev =>
                            {
                                IsBusy = false;
                                SchemaList =
                                    new ObservableCollection<SettingListView>(_metaData.First()
                                        .GetPropertyValueAsEnumerate("NameSpaces").Select((ns, i) => new SettingListView
                                        {
                                            Name = ns.GetPropertyValueAsString("Name"),
                                            Id = i,
                                            IsSelected = false,
                                            Childs =
                                                new ObservableCollection<SettingListView>(
                                                    ns.GetPropertyValueAsEnumerate("Objects")
                                                        .Select(o => new SettingListView
                                                        {
                                                            Name = o.GetPropertyValueAsString("Name"),
                                                            Id = o.GetPropertyValueAs<int>("Id"),
                                                            IsSelected = false,
                                                            Childs = null
                                                        }).ToList())
                                        }).ToList());
                            }, null);
                        }
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseCommand_Execute(object o)
        {
            if (!_isCancel) return;
            var config = ConfigurationManager.ConfigProvider.GetDatas();
            config["Project"]["Type"] = JToken.Parse("0");
            ConfigurationManager.ConfigProvider.SaveData(config);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Methods

        private void SaveCurrentSetting(Action complete)
        {
            if ((string.IsNullOrEmpty(ServerText) || string.IsNullOrEmpty(DataSourceName)) && ((RadComboBoxItem)DataTypeSelectedItem).Tag.ToString() != "-1")
            {
                MessageBox.Show("Please Enter ServerName and DataSource!");
                return;
            }
            try
            {
                var config = ConfigurationManager.ConfigProvider.GetDatas();
                config["Project"]["Type"] = JToken.FromObject(((RadComboBoxItem)ProjectTypeSelectedItem).Tag);
                config["Project"]["Data"]["Type"] = JToken.FromObject(((RadComboBoxItem)DataTypeSelectedItem).Tag);
                config["Project"]["Data"]["Path"] = JToken.FromObject(ServerText);
                config["Project"]["Data"]["Name"] = JToken.FromObject(DataSourceName);
                config["Project"]["Data"]["UserName"] = JToken.FromObject(string.IsNullOrEmpty(UserName) ? "" : UserName);
                config["Project"]["Data"]["Password"] = JToken.FromObject(string.IsNullOrEmpty(Password) ? "" : Password);
                IsBusy = true;
                Task.Run(() =>
                {
                    ConfigurationManager.ConfigProvider.SaveData(config);
                }).ContinueWith(r =>
                {
                    if (r.IsCompleted)
                    {
                        _synchronizationContext.Send(ev =>
                        {
                            IsBusy = false;
                            _isCancel = false;
                            complete();
                        }, null);
                    }
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Commands

        public ICommand LoadCommand { get; set; }
        public ICommand SchemaChangeCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ClosedCommand { get; set; }

        #endregion

    }
}