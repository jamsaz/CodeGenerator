using System.ComponentModel;
using System.Runtime.CompilerServices;
using Jamsaz.CodeGenerator.Tool.Annotations;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels
{
    public class TemplateItem : INotifyPropertyChanged
    {
        private string _name;
        public string Name { get { return _name; } set { _name = value; OnPropertyChanged(nameof(Name)); } }

        private string _extension;
        public string Extension { get { return _extension; } set { _extension = value; OnPropertyChanged(nameof(Name)); } }

        private string _path;
        public string Path { get { return _path?.Replace(@"\", "\\"); } set { _path = value; OnPropertyChanged(nameof(Name)); } }

        private string _template;
        public string Template { get { return _template?.Replace(@"\", "\\"); ; } set { _template = value; OnPropertyChanged(nameof(Name)); } }

        private string _type;
        public string Type { get { return _type; } set { _type = value; OnPropertyChanged(nameof(Name)); } }

        private string _object;
        public string Object { get { return _object; } set { _object = value; OnPropertyChanged(nameof(Name)); } }

        private bool _createFolderForNamespace;
        public bool CreateFolderForNamespace { get { return _createFolderForNamespace; } set { _createFolderForNamespace = value; OnPropertyChanged(nameof(Name)); } }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? "NewItem" : Name;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}