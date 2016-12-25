namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class EnumItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        #region Overrides

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            return "NewItem";
        }

        #endregion
    }
}
