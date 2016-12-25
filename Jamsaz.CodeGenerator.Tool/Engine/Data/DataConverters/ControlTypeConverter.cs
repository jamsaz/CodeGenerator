using Jamsaz.CodeGenerator.Tool.Engine.Data.Models;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.DataConverters
{
    public static class ControlTypeConverter
    {
        public static ControlType GetControlTypeFromDataType(string dataType, bool hasParent)
        {
            var controlType = ControlType.Text;
            if (hasParent)
            {
                controlType = ControlType.Search;
            }
            else
            {
                switch (dataType.ToLower())
                {
                    case "int":
                        controlType = ControlType.Number;
                        break;
                    case "long":
                        controlType = ControlType.Number;
                        break;
                    case "decimal":
                        controlType = ControlType.Amount;
                        break;
                    case "bool":
                        controlType = ControlType.Check;
                        break;
                    case "bit":
                        controlType = ControlType.Check;
                        break;
                    case "double":
                        controlType = ControlType.Number;
                        break;
                    case "float":
                        controlType = ControlType.Number;
                        break;
                    case "string":
                        controlType = ControlType.Text;
                        break;
                    case "datetimeoffset":
                        controlType = ControlType.Time;
                        break;
                    case "datetime":
                        controlType = ControlType.Date;
                        break;
                }
            }
            return controlType;
        }

    }
}
