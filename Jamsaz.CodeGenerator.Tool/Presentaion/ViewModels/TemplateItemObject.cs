namespace Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels
{
    public class TemplateItemObject
    {
        public string Name { get; set; }
        public string CSharpFileCode { get; set; }

        public override string ToString()
        {
            return string.IsNullOrEmpty(Name) ? "NewItem" : Name;
        }
    }
}