using System;
using System.Windows.Input;

namespace $safeprojectname$.FormFields.SelectForm
{
    public class SelectFormButtonCommand :ICommand
    {
        #region Ctor

        public SelectFormButtonCommand(Action<object> action)
        {
            excuteAction = action;
        }

        #endregion

        #region Private Fields

        private Action<object> excuteAction; 

        #endregion

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            excuteAction.Invoke(parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
