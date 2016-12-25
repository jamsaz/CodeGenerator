using System;
using System.Windows.Input;

namespace $safeprojectname$.GridView
{
    public class ButtonCommand :ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var p = (GridViewButtonCommandParameter)parameter;
            switch (p.Action)
            {
                case GridViewButtonTypes.ExportGridViewButton:
                    var value = int.Parse(p.Parameter.ToString());
                    switch (value)
                    {
                        case (int)GridViewExportTypes.Excel:
                            Export.ExportXlsx(p.Parent);
                            break;
                        case (int)GridViewExportTypes.Pdf:
                            Export.ExportPdf(p.Parent);
                            break;
                    }
                    break;
                case GridViewButtonTypes.AddNewRecordButton:

                    break;
                case GridViewButtonTypes.DeleteRecordButton:

                    break;
                case GridViewButtonTypes.EditRecordButton:

                    break;
                case GridViewButtonTypes.PrintGridViewButton:

                    break;
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
