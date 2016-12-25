using Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels;
using Jamsaz.CodeGenerator.Tool.Presentaion.Views;
using Telerik.Windows.Controls.Data.PropertyGrid;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class MasterFormType
    {
        [Editor(typeof(MasterFormTypeComboBoxEditorControl), "SelectedItem")]
        public MasterFormTypes Type { get; set; }
        public bool Value { get; set; }

        #region Overrides

        public override string ToString()
        {
            if (Type != null)
                return Type.Name;
            return "NewMasterFormTemplate";
        }

        #endregion
    }
}
