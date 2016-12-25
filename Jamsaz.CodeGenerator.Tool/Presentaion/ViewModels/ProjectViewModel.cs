using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Jamsaz.CodeGenerator.Tool.Annotations;
using Jamsaz.CodeGenerator.Tool.Engine;
using Jamsaz.CodeGenerator.Tool.Engine.Generator;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Presentaion.Views;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using Telerik.Windows.Controls;
using MyConstants = Jamsaz.CodeGenerator.Tool.Global.Constants;


namespace Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        #region Ctor

        public ProjectViewModel(ICodeManager codeManager, IConfigurationManager configurationManager,
            ITemplateGenerator generator)
        {
            metaData = new ObservableCollection<object>();
            LoadMetaData = new DelegateCommand(LoadMetaData_Execute);
            UpdateMetaData = new DelegateCommand(UpdateMetaData_Execute);
            SelectCommand = new DelegateCommand(SelectCommand_Execute);
            SaveMetaDataChanges = new DelegateCommand(SaveMetaDataChanges_Execute);
            NewProject = new DelegateCommand(NewProject_Execute);
            Close = new DelegateCommand(Close_Execute);
            GenerateCode = new DelegateCommand(GenerateCode_Execute);
            OpenProject = new DelegateCommand(OpenProject_Execute);
            Setting = new DelegateCommand(Setting_Excute);
            _isBusy = false;
            //-------------------------------------------------------------------------
            _sycornizationContext = SynchronizationContext.Current;
            CodeManager = codeManager;
            ConfigurationManager = configurationManager;
            Generator = generator;
        }

        #endregion

        #region Privates

        private readonly SynchronizationContext _sycornizationContext;

        #endregion

        #region Injection

        protected ICodeManager CodeManager;
        protected IConfigurationManager ConfigurationManager;
        protected ITemplateGenerator Generator;

        #endregion

        #region Properties

        private ObservableCollection<object> metaData;
        public ObservableCollection<object> MetaData
        {
            get
            {
                return metaData;
            }
            set
            {
                if (metaData == value)
                    return;
                metaData = value;
                OnPropertyChanged(nameof(MetaData));
            }
        }

        private object patterns;
        public object Patterns
        {
            get
            {
                return patterns;
            }
            set
            {
                if (patterns == value)
                    return;
                patterns = value;
                OnPropertyChanged(nameof(Patterns));
            }
        }

        private Visibility closeVisible;
        public Visibility CloseVisible
        {
            get { return closeVisible; }
            set
            {
                if (closeVisible == value)
                    return;
                closeVisible = value;
                OnPropertyChanged(nameof(CloseVisible));
            }
        }

        private object selectedItem;
        public object SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (selectedItem == value)
                    return;
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
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

        #endregion

        #region Commands

        public ICommand LoadMetaData { get; set; }

        public ICommand UpdateMetaData { get; set; }

        public ICommand SelectCommand { get; set; }

        public ICommand SaveMetaDataChanges { get; set; }

        public ICommand NewProject { get; set; }

        public ICommand Close { get; set; }

        public ICommand GenerateCode { get; set; }

        public ICommand OpenProject { get; set; }

        public ICommand Setting { get; set; }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NewProject_Execute(object o)
        {
            CodeManager.InitializeProject();
            MetaData = new ObservableCollection<object> { CodeManager.CodeObjects };
        }
        private void SaveMetaDataChanges_Execute(object o)
        {
            CodeManager.SaveProjectSetting(MetaData);
        }
        private void SelectCommand_Execute(object o)
        {
            var owner = o as RadTreeView;
            if (owner != null) SelectedItem = owner.SelectedItem;
        }
        private void UpdateMetaData_Execute(object o)
        {
            try
            {
                if (ConfigurationManager.ProjectWasSetting)
                {
                    IsBusy = true;
                    Task.Run(() =>
                    {
                        var msg = MessageBox.Show("Do you want to change project setting for a new connection?", "Question", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        switch (msg)
                        {
                            case MessageBoxResult.Yes:
                                GetDispatcher(LoadProjectSettingDialog);
                                break;
                            case MessageBoxResult.No:
                                CodeManager.RealoadProject();
                                break;
                            default:
                                return;
                        }
                        LoadMetaData.Execute("Reload");
                    }).ContinueWith(result =>
                    {
                        if (result.IsCompleted)
                        {
                            _sycornizationContext.Send(e => { IsBusy = false; }, null);
                        }
                    });
                }
                else
                {
                    LoadProjectSettingDialog();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"[Error]:{e.Message}[Stack Trace]:{e.StackTrace}");
            }
        }
        private void LoadMetaData_Execute(object o)
        {
            if (o?.ToString() == "Reload") MetaData = null;
            if (!ConfigurationManager.SolutionIsOpen)
            {
                MessageBox.Show("Please open a solution first!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                MetaData = null;
                return;
            }
            if (MetaData != null && MetaData.Count != 0)
            {
                IsBusy = true;
                Task.Run(() =>
                {
                    SaveMetaDataChanges?.Execute(null);
                }).ContinueWith((result) =>
                {
                    if (result.IsCompleted)
                    {
                        _sycornizationContext.Send((e) => { IsBusy = false; }, null);
                    }
                });
                return;
            }
            if (ConfigurationManager.CurrentDte != null)
                CloseVisible = Visibility.Hidden;
            if (!ConfigurationManager.ProjectWasSetting)
            {
                LoadProjectSettingDialog();
            }
            CodeManager.InitializeProject();
            MetaData = new ObservableCollection<object> { CodeManager.CodeObjects };
        }
        private void Close_Execute(object o)
        {
            Application.Current.MainWindow.Close();
        }
        private void GenerateCode_Execute(object o)
        {
            var configFile = ConfigurationManager.ConfigProvider.GetDatas();
            if (string.IsNullOrEmpty(configFile["Project"].Value<JObject>()["TemplatePath"].Value<string>()))
            {
                MessageBox.Show("Please set generate setting first!", "Info", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                IsBusy = true;
                Task.Run(() =>
                {
                    CodeManager.SaveProjectSetting(MetaData);
                    Generator.Generate();
                }).ContinueWith((result) =>
                {
                    if (result.IsCompleted)
                    {
                        _sycornizationContext.Send((e) => { IsBusy = false; }, null);
                    }
                });
            }
        }
        private void OpenProject_Execute(object o)
        {
            var openFileDialog = new OpenFileDialog { Filter = "Json files (*.json)|*.json|All files (*.*)|*.*" };
            if (openFileDialog.ShowDialog() != true) return;
            var strMetadata = File.ReadAllText(openFileDialog.FileName);
            var configurationPath = ConfigurationManager.ConfigurationFilePath;
            File.WriteAllText(configurationPath + MyConstants.Metadata, strMetadata);
            CodeManager.RealoadProject();
        }
        private void Setting_Excute(object o)
        {
            var settingDialog = new GenerateSettingModal(ConfigurationManager);
            settingDialog.ShowDialog();
        }

        #endregion

        #region Methods

        private void LoadProjectSettingDialog()
        {
            var settingModal = new MetadataSettingModal(CodeManager, ConfigurationManager);
            var result = settingModal.ShowDialog();
            if (!result.HasValue) return;
            var metadataSettingViewModel = settingModal.DataContext as MetadataSettingViewModel;
            CodeManager.SaveProjectSetting(metadataSettingViewModel?.Metadata);
            CodeManager.RealoadProject();
        }

        private void GetDispatcher(Action call)
        {
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (Application.Current != null)
            {
                dispatcher = Application.Current.Dispatcher;
            }
            dispatcher?.Invoke(call);
        }

        #endregion
    }
}
