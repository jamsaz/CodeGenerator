using System.Windows.Input;

namespace $safeprojectname$.Dialog
{
    public class DialogButton
    {
        public string Content { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
    }
}
