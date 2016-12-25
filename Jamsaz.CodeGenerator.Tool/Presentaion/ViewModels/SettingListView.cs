using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Jamsaz.CodeGenerator.Tool.Annotations;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels
{
    public class SettingListView : INotifyPropertyChanged
    {
        public string Name { get; set; }

        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public int Id { get; set; }
        public ObservableCollection<SettingListView> Childs { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
