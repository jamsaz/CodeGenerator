using Microsoft.Win32;
using Telerik.Windows.Controls;

namespace $safeprojectname$.GridView
{
    public static class Export
    {
        public static void ExportPdf(object gridView)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = "pdf",
                Filter = string.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "pdf", "Pdf"),
                FilterIndex = 1
            };

            if (dialog.ShowDialog() != true) return;
            using (var stream = dialog.OpenFile())
            {
                var radGridview = (RadGridView) gridView;
                radGridview.ExportToPdf(stream);
            }
        }
        public static void ExportXlsx(object gridView)
        {
            var dialog = new SaveFileDialog
            {
                DefaultExt = "xlsx",
                Filter = string.Format("Workbooks (*.{0})|*.{0}|All files (*.*)|*.*", "xlsx"),
                FilterIndex = 1
            };

            if (dialog.ShowDialog() != true) return;
            using (var stream = dialog.OpenFile())
            {
                var radGridview = (RadGridView)gridView;
                radGridview.ExportToXlsx(stream);
            }
        }
    }
}
