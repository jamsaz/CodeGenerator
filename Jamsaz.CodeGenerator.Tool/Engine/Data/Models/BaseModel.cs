namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class BaseModel
    {
        public override string ToString()
        {
            var name = GetType().GetProperty("Name");
            object value;
            if (name == null)
            {
                if (GetType().GetProperty("Type").GetValue(this) == null) return "NewItem";
                name = GetType().GetProperty("Type").GetValue(this).GetType().GetProperty("Name");
                value = name.GetValue(GetType().GetProperty("Type").GetValue(this));
            }
            else
            {
                value = name.GetValue(this);
            }
            return value?.ToString() ?? "NewItem";
        }
    }
}
