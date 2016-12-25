using System.ComponentModel;
using System.Runtime.CompilerServices;
using Jamsaz.CodeGenerator.Tool.Annotations;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels
{
    public class MasterFormTypes : INotifyPropertyChanged
    {
        public MasterFormTypes() { }
        public MasterFormTypes(string name)
        {
            Name = name;
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        #region Overrides

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }

}
