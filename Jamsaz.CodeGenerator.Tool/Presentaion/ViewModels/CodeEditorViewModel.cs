using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ICSharpCode.AvalonEdit;
using Telerik.Windows.Controls;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels
{
    public class CodeEditorViewModel
    {
        private readonly string _filePath;
        public CodeEditorViewModel(string filePath)
        {
            LoadCommand = new DelegateCommand(LoadCommand_Execute);
            SaveCommand = new DelegateCommand(SaveCommand_Execute);
            _filePath = filePath;
        }

        public ICommand LoadCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public string CodeText { get; set; }

        private void SaveCommand_Execute(object o)
        {
            var window = (Window)o;
            var textEditor = (Grid) window.Content;
            CodeText = ((TextEditor) textEditor.Children[0]).Text;
            File.WriteAllText(_filePath, CodeText);
            window.Close();
        }

        private void LoadCommand_Execute(object o)
        {
            var textEditor = (TextEditor) o;
            CodeText = File.ReadAllText(_filePath);
            textEditor.Text = CodeText;
        }
    }
}