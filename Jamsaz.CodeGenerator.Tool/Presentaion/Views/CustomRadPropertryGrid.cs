using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Models;
using Newtonsoft.Json.Linq;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.PropertyGrid;

namespace Jamsaz.CodeGenerator.Tool.Presentaion.Views
{
    public class CustomRadPropertryGrid : RadPropertyGrid
    {
        public static readonly DependencyProperty DefinitionCollectionProperty = DependencyProperty.Register(
            "DefinitionCollection", typeof(PropertyDefinitionCollection), typeof(CustomRadPropertryGrid),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public PropertyDefinitionCollection DefinitionCollection
        {
            get { return (PropertyDefinitionCollection)GetValue(DefinitionCollectionProperty); }
            set { SetValue(DefinitionCollectionProperty, value); }
        }

        private static void DefinitionCollection_PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var owner = (CustomRadPropertryGrid)sender;
            owner.LoadPropertyDefinitaionFromPattern(owner);
        }

        public static readonly DependencyProperty PatternsProperty = DependencyProperty.Register(
            "Patterns", typeof(ObservableCollection<Pattern>), typeof(CustomRadPropertryGrid), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, DefinitionCollection_PropertyChangedCallback));

        public ObservableCollection<Pattern> Patterns
        {
            get { return (ObservableCollection<Pattern>)GetValue(PatternsProperty); }
            set { SetValue(PatternsProperty, value); }
        }

        private void LoadPropertyDefinitaionFromPattern(RadPropertyGrid owner)
        {
            owner.PropertyDefinitions.Clear();
            var type = ((JObject)owner.Item)["NameSpaces"] == null
                    ? (((JObject)owner.Item)["Objects"] == null
                        ? (((JObject)owner.Item)["Properties"] == null ? "" : "Objects")
                        : "NameSpaces")
                    : "Project";
            foreach (var pattern in (JArray)FindPattern(type))
            {
                owner.PropertyDefinitions.Add(new LookupPropertyDefinition
                {
                    Binding = new Binding(pattern["Key"].Value<string>()) { Source = Item },
                    DisplayName = pattern["Key"].Value<string>(),
                    Description = pattern["Describtion"].Value<string>(),
                    GroupName = pattern["GroupName"].Value<string>()
                });
            }
        }

        private object FindPattern(string name, PatternDefinition parent = null)
        {
            switch (name)
            {
                case "Project":
                    return Patterns.First().Definitaion.First().Value;
                case "NameSpaces":
                    var nameSpacesFirstOrDefault = ((JArray)Patterns.First().Definitaion.First().Value).FirstOrDefault(x => x["Key"].Value<string>() == "NameSpaces");
                    return nameSpacesFirstOrDefault?["Value"];
                case "Objects":
                    var objectsFirstOrDefault = ((JArray)Patterns.First().Definitaion.First().Value).FirstOrDefault(x => x["Key"].Value<string>() == "NameSpaces");
                    if (objectsFirstOrDefault == null) return null;
                    var firstOrDefault = ((JArray)objectsFirstOrDefault["Value"]).FirstOrDefault(x => x["Key"].Value<string>() == "Objects");
                    return firstOrDefault?["Value"];
                default:
                    return null;
            }
        }
    }
}
