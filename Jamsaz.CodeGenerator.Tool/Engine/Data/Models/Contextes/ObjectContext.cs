using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Global.Helpers;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models.Contextes
{
    public class ObjectContext : IObjectContext
    {
        private readonly IMetadataProvider _metaDataProvider;
        
        public ObjectContext(IMetadataProvider metaDataProvider)
        {
            _metaDataProvider = metaDataProvider;
        }

        public List<object> GetCompositeObjects(string id)
        {
            var jObject = _metaDataProvider.GetMetaData();
            if (jObject == null) return new List<object>();
            var @select = jObject["NameSpaces"].Select(x => x["Objects"].Select(o => o["Relations"]).ToList());
            var result = from nameSpace in @select from item in nameSpace from relation in item select relation;
            return
                result.ToList()
                    .Where(r => r["RefrenceObjectId"].ToString() == id)
                    .Select(x => new
                    {
                        CapitalizedNameSpace = x["ParentObjectSchemaName"].ToString().Capitalize(),
                        SingularizedName = x["ParentObjectName"].ToString().Singularize(),
                        PluralizedName = x["ParentObjectName"].ToString().Pluralize()
                    }).Cast<object>().ToList();
        }
        public object GetObjects(JToken item)
        {
            var listOfObjects = new List<object>();
            var project = _metaDataProvider.GetMetaData();
            foreach (var n in project.GetPropertyValueAsEnumerate("NameSpaces"))
            {
                foreach (
                    var o in
                        n.GetPropertyValueAsEnumerate("Objects"))
                {
                    var compositeObjects = GetCompositeObjects(o.GetPropertyValueAsString("Id"));
                    listOfObjects.Add(new
                    {
                        ProjectName = project.GetPropertyValueAsString("Name"),
                        Prefix = item["Name"].Value<string>().Replace("@", ""),
                        Name = o.GetPropertyValueAsString("Name"),
                        DisplayName = o.GetPropertyValueAsString("DisplayName"),
                        CapitalizedNameSpace = n.GetPropertyValueAsString("Name").Capitalize(),
                        SchemaName = n.GetPropertyValueAsString("SchemaName"),
                        SingularizedName = o.GetPropertyValueAsString("Name").Singularize(),
                        SingularizedLowerName = o.GetPropertyValueAsString("Name").Singularize().ToLower(),
                        CapitalizedName = o.GetPropertyValueAsString("Name").Capitalize(),
                        UnCapitalizedName = o.GetPropertyValueAsString("Name").UnCapitalize(),
                        PluralizedName = o.GetPropertyValueAsString("Name").Pluralize(),
                        PluralizedLowerName = o.GetPropertyValueAsString("Name").Pluralize().ToLower(),
                        ParentModule = o.GetPropertyValueAs<int>("ParentModule"),
                        Properties = o.GetPropertyValueAsEnumerate("Properties").Select(p => new
                        {
                            DisplayName = p.GetPropertyValueAsString("DisplayName"),
                            SingularizedName = p.GetPropertyValueAsString("Name").Singularize(),
                            LowerSingularizedName = p.GetPropertyValueAsString("Name").Singularize().LowercaseFirst(),
                            GridColumn =
                                (p.GetPropertyValueAs<ControlType>("ControlType") == ControlType.Search)
                                    ? "GridViewColumnTypes.ComboBoxColumn"
                                    : (p.GetPropertyValueAsEnumerate("EnumItems").Any()
                                        ? "GridViewColumnTypes.ComboBoxColumn"
                                        : "GridViewColumnTypes.DataColumn"),
                            HasDisplayName = !string.IsNullOrEmpty(p.GetPropertyValueAsString("DisplayName")),
                            IsComputed = p.GetPropertyValueAs<bool>("IsComputed"),
                            IsRequired =
                                (p.GetPropertyValueAs<string>("Name") != "id" && !p.GetPropertyValueAs<bool>("ReadOnly") &&
                                 !p.GetPropertyValueAs<bool>("Optional")),
                            IsString =
                                (p.GetPropertyValueAs<string>("DataType") == "string" &&
                                 p.GetPropertyValueAs<int>("MaxLength") > 0),
                            MaxLength = p.GetPropertyValueAs<int>("MaxLength"),
                            IsObject = p.GetPropertyValueAs<ControlType>("ControlType") == ControlType.Search,
                            SQLDataType = p.GetPropertyValueAs<string>("SQLDataType"),
                            Nullable = p.GetPropertyValueAs<bool>("Nullable"),
                            DataType = p.GetPropertyValueAs<string>("DataType"),
                            Name = p.GetPropertyValueAs<string>("Name"),
                            UnCapitalizedName = p.GetPropertyValueAs<string>("Name").UnCapitalize(),
                            ParentUnCapitalizedName = o.GetPropertyValueAsString("Name").UnCapitalize(),
                            ParentObjectSingularizedName =
                                p.GetPropertyValueAs<string>("ParentObjectName").Singularize(),
                            ParentObjectPluralizedName = p.GetPropertyValueAs<string>("ParentObjectName").Pluralize(),
                            ParentObjectLowerPluralizedName =
                                p.GetPropertyValueAs<string>("ParentObjectName").Pluralize().LowercaseFirst(),
                            ParentObjectCapitalizedName = p.GetPropertyValueAs<string>("ParentObjectName").Capitalize(),
                            ParentObjectUnCapitalizedName =
                                p.GetPropertyValueAs<string>("ParentObjectName").UnCapitalize(),
                            CapitalizeNameSpace = n.GetPropertyValueAsString("Name").Capitalize(),
                            InGrid = p.GetPropertyValueAs<bool>("InGrid"),
                            InLookup = p.GetPropertyValueAs<bool>("InLookup"),
                            ControlType = p.GetPropertyValueAs<ControlType>("ControlType"),
                            Attributes = p.GetPropertyValueAsString("Attributes"),
                            ParentObjectPrimaryKeyName = p.GetPropertyValueAsString("ParentObjectPrimaryKeyName"),
                            Visible = p.GetPropertyValueAs<bool>("Visible"),
                            ViewVisible = p.GetPropertyValueAs<bool>("Visible") ? "Visible" : "Hidden",
                            ReadOnly = p.GetPropertyValueAsString("ReadOnly").ToString().ToLower(),
                            EnumItems = p.GetPropertyValueAsEnumerate("EnumItems"),
                            FieldType = GetFieldType(p),
                            WpfField = GetWpfField(p),
                            ParentObjectProperties = GetParentObjectProperties(p, n),
                            ParentObjectName = p.GetPropertyValueAsString("ParentObjectName"),
                            ParentObjectNameSpace = p.GetPropertyValueAsString("ParentObjectNameSpace"),
                            SelectForm = p.GetPropertyValueAs<ControlType>("ControlType") == ControlType.Search,
                            Optional = p.GetPropertyValueAs<bool>("Optional"),
                            ColumnIndex = p.GetPropertyValueAs<int>("ColumnIndex"),
                            RowIndex = p.GetPropertyValueAs<int>("RowIndex"),
                        }),
                        CompositeObjects = compositeObjects,
                        IsComposite = compositeObjects.Any(),
                        HasOperationRow =
                            (o.GetPropertyValueAs<bool>("Add") || o.GetPropertyValueAs<bool>("Edit") ||
                             o.GetPropertyValueAs<bool>("Delete")),
                        FormRowGrid = GetFormRowGrid(o),
                        FormColumnGrid = GetFormColumnGrid(o),
                        FormRowCount = (o.GetPropertyValueAs<int>("RowOfForm") + 1).ToString(),
                        ParentObjectRepository = GetParentObjectRepository(o, n),
                        UnSingularizedName = o.GetPropertyValueAsString("Name").Singularize().LowercaseFirst(),
                        PrimaryKey = o.GetPropertyValueAsString("PrimaryKey"),
                        Add = o.GetPropertyValueAs<bool>("Add"),
                        Edit = o.GetPropertyValueAs<bool>("Edit"),
                        Delete = o.GetPropertyValueAs<bool>("Delete"),
                        AddButtonText = o.GetPropertyValueAsString("AddButtonText"),
                        EditButtonText = o.GetPropertyValueAsString("EditButtonText"),
                        DeleteButtonText = o.GetPropertyValueAsString("DeleteButtonText"),
                        SaveButtonText = o.GetPropertyValueAsString("SaveButtonText"),
                        CancelButtonText = o.GetPropertyValueAsString("CancelButtonText"),
                        SaveMessageText = o.GetPropertyValueAsString("SaveMessageText"),
                        SaveMessageTitle = o.GetPropertyValueAsString("SaveMessageTitle"),
                        DeleteMessageText = o.GetPropertyValueAsString("DeleteMessageText"),
                        DeleteMessageTitle = o.GetPropertyValueAsString("DeleteMessageTitle"),
                        GridRowNotSelectedMessage = o.GetPropertyValueAsString("GridRowNotSelectedMessage"),
                        GridRowDeletedMessage = o.GetPropertyValueAsString("GridRowDeletedMessage"),
                        IsGrid = item["Name"].Value<string>().ToLower().Contains("gridview"),
                        IsForm = (item["Name"].Value<string>().ToLower().Contains("addviewform") || item["Name"].Value<string>().ToLower().Contains("editviewform")),
                        IsDialog = item["Name"].Value<string>().ToLower().Contains("addviewdialog") || item["Name"].Value<string>().ToLower().Contains("editviewdialog"),
                        IsFormOrDialog =
                            item["Name"].Value<string>().ToLower().Contains("addviewdialog") || item["Name"].Value<string>().ToLower().Contains("editviewdialog") ||
                            item["Name"].Value<string>().ToLower().Contains("addviewform") || item["Name"].Value<string>().ToLower().Contains("editviewform"),
                        IsEdit = item["Name"].Value<string>().ToLower().Contains("edit"),
                        MasterForm = o.GetPropertyValueAs<bool>("MasterForm"),
                        VisibleForm = o.GetPropertyValueAs<bool>("VisibleForm"),
                        MasterFormTypes = o.GetPropertyValueAsEnumerate("MasterFormTypes")
                    });
                }
            }
            return listOfObjects;
        }
        public object GetAllObjects(JToken item)
        {
            var objects = (IList)GetObjects(item);
            return new
            {
                ProjectName = _metaDataProvider.GetMetaData().GetPropertyValueAsString("Name"),
                Objects = objects
            };
        }
        public object GetDataContextObjects(JToken item)
        {
            var objects = (List<object>)GetObjects(item);
            var compositObjects = new List<object>();

            compositObjects.AddRange(objects.Where(
                x =>
                    x.GetPropertyValueAs<bool>("IsComposite"))
                .Select(x => new
                {
                    CapitalizedNameSpace = x.GetPropertyValueAsString("CapitalizedNameSpace"),
                    SingularizedName = x.GetPropertyValueAsString("SingularizedName"),
                }));
            return new
            {
                CapitalizedName = _metaDataProvider.GetMetaData().GetPropertyValueAsString("Name"),
                Objects = objects,
                CompositeObjects = compositObjects
            };
        }

        private static string GetFieldType(object o)
        {
            switch (o.GetPropertyValueAs<ControlType>("ControlType"))
            {
                case ControlType.Text:
                    return "TextBox";
                case ControlType.Date:
                    return "DateTimebox";
                case ControlType.Time:
                    return "DateTimebox";
                case ControlType.Check:
                    return "Checkbox";
                case ControlType.EnumCombo:
                    return "Combobox";
                case ControlType.EnumRadio:
                    return "RadioBoxList";
                case ControlType.EnumCheck:
                    return "CheckBoxList";
                default:
                    return "TextBox";
            }
        }
        private static string GetWpfField(object o)
        {
            var rowspan = string.IsNullOrEmpty(o.GetPropertyValueAsString("RowSpan")) || o.GetPropertyValueAs<int>("RowSpan") == 0 ? "" : $"Grid.RowSpan=\"{o.GetPropertyValueAsString("RowSpan")}\"";
            var colspan = string.IsNullOrEmpty(o.GetPropertyValueAsString("ColumnSpan")) || o.GetPropertyValueAs<int>("ColumnSpan") == 0 ? "" : $"Grid.ColumnSpan=\"{o.GetPropertyValueAsString("ColumnSpan")}\"";
            var rowIndex = o.GetPropertyValueAsString("RowIndex");
            var columnIndex = o.GetPropertyValueAsString("ColumnIndex");
            var displayName = o.GetPropertyValueAsString("DisplayName");
            var visible = o.GetPropertyValueAs<bool>("Visible");
            var singularizedName = o.GetPropertyValueAsString("Name").Singularize();
            var name = o.GetPropertyValueAsString("Name");
            switch (o.GetPropertyValueAs<ControlType>("ControlType"))
            {
                case ControlType.Text:
                    return
                        $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" FieldType=\"TextBox\"  Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                case ControlType.Date:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" FieldType=\"DateTimebox\"  Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                case ControlType.Time:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" FieldType=\"DateTimebox\"  Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                case ControlType.Check:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" FieldType=\"Checkbox\"  Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                case ControlType.EnumCombo:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" ItemSource=\"{{Binding {singularizedName}EnumItems}}\" DisplayMemberPath=\"Content\" SelectedValuePath=\"Value\" FieldType=\"Combobox\"  Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                case ControlType.EnumRadio:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" SelectedValuePath=\"Value\" ItemSource=\"{{Binding {singularizedName}EnumItems}}\" SelectionMode=\"Single\" FieldType=\"RadioBoxList\"  Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                case ControlType.EnumCheck:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" ItemSource=\"{{Binding {singularizedName}EnumItems}}\" SelectionMode=\"Multiple\" FieldType=\"CheckBoxList\"  Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                case ControlType.Number:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" FieldType=\"TextBox\" Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                case ControlType.Amount:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" FieldType=\"TextBox\" Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                case ControlType.NationalCode:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" FieldType=\"TextBox\" Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
                default:
                    return $"<formFields:Field Grid.Row=\"{rowIndex}\" Grid.Column=\"{columnIndex}\" {rowspan} {colspan} LabelText=\"{displayName}\" Value=\"{{Binding {name},Mode=TwoWay}}\" FieldType=\"TextBox\" Visibility=\"{(visible ? "Visible" : "Hidden")}\"/>";
            }
        }
        private static string GetFormColumnGrid(object o)
        {
            var output = "\n";
            for (var i = 0; i < o.GetPropertyValueAs<int>("ColumnOfForm"); i++)
            {
                output += "<ColumnDefinition/>\n";
            }
            return output;
        }
        private static string GetFormRowGrid(object o)
        {
            var output = "\n";
            for (var i = 0; i < o.GetPropertyValueAs<int>("RowOfForm"); i++)
            {
                output += "<RowDefinition/>\n";
            }
            return output;
        }
        private IEnumerable<object> GetParentObjectProperties(object obj, object ns)
        {
            var jObject = _metaDataProvider.GetMetaData();
            if (jObject == null) return new List<object>();
            var @select = jObject.GetPropertyValueAsEnumerate("NameSpaces").Where(
                x => x.GetPropertyValueAs<string>("SchemaName").Capitalize().Equals(ns.GetPropertyValueAsString("Name").Capitalize()))
                .Select(
                    x =>
                        x.GetPropertyValueAsEnumerate("Objects").Where(o => o.GetPropertyValueAs<string>("Name").Equals(obj.GetPropertyValueAsString("ParentObjectName")))
                            .Select(o => o.GetPropertyValueAsEnumerate("Properties"))
                            .ToList());
            var result = from nameSpace in @select from item in nameSpace from relation in item select relation;
            return result.ToList().Select(x => new
            {
                Name = x.GetPropertyValueAsString("Name"),
                CapitalizeNameSpace = x.GetPropertyValueAs<string>("CapitalizeNameSpace"),
                ParentObjectName = x.GetPropertyValueAs<string>("ParentObjectName"),
                ParentObjectSingularizedName = x.GetPropertyValueAs<string>("ParentObjectName").Singularize(),
                ParentObjectCapitalizedName = x.GetPropertyValueAs<string>("ParentObjectName").Capitalize(),
                ParentObjectUnCapitalizedName = x.GetPropertyValueAs<string>("ParentObjectName").UnCapitalize(),
                HasDisplayName = !string.IsNullOrEmpty(x.GetPropertyValueAs<string>("DisplayName")),
                DisplayName = x.GetPropertyValueAs<string>("DisplayName"),
                SingularizedName = x.GetPropertyValueAs<string>("Name").Singularize(),
                GridColumn = (x.GetPropertyValueAs<ControlType>("ControlType") == ControlType.Search)
                                    ? "GridViewColumnTypes.ComboBoxColumn"
                                    : (x.GetPropertyValueAsEnumerate("EnumItems").Any()
                                        ? "GridViewColumnTypes.ComboBoxColumn"
                                        : "GridViewColumnTypes.DataColumn"),
                HasParent = x.GetPropertyValueAs<ControlType>("ControlType") == ControlType.Search,
                InLookup = x.GetPropertyValueAs<bool>("InLookup"),
                Attributes = x.GetPropertyValueAs<string>("Attributes"),
                ParentObjectPrimaryKeyName = x.GetPropertyValueAs<string>("ParentObjectPrimaryKeyName"),
                ParentObjectPluralizedName = x.GetPropertyValueAs<string>("ParentObjectName").Pluralize(),
                ParentObjectLowerPluralizedName = x.GetPropertyValueAs<string>("ParentObjectName").Pluralize().LowercaseFirst(),
                ParentObjectVisible = x.GetPropertyValueAs<bool>("Visible").ToString().ToLower()
            });
        }
        private string GetParentObjectRepository(object o, object ns)
        {
            var output = ", ";
            foreach (var propertiese in o.GetPropertyValueAsEnumerate("Properties"))
            {
                if (!string.IsNullOrEmpty(propertiese.GetPropertyValueAsString("ParentObjectName")))
                {
                    output += $"Repositories.{propertiese.GetPropertyValueAsString("ParentObjectNameSpace")}.I{propertiese.GetPropertyValueAsString("ParentObjectName").Singularize()}Repository {propertiese.GetPropertyValueAsString("ParentObjectName").UnCapitalize()}Repository, ";
                }
                output =
                    GetParentObjectProperties(propertiese, ns)
                        .Where(
                            parentObjectProperty =>
                                !string.IsNullOrEmpty(
                                    parentObjectProperty.GetType()
                                        .GetProperty("ParentObjectName")
                                        .GetValue(parentObjectProperty, null)
                                        .ToString()))
                        .Aggregate(output,
                            (current, parentObjectProperty) =>
                                current +
                                $"Repositories.{parentObjectProperty.GetPropertyValueAsString("CapitalizeNameSpace")}.I{parentObjectProperty.GetPropertyValueAsString("ParentObjectName")}Repository {parentObjectProperty.GetPropertyValueAsString("ParentObjectName").UnCapitalize()}Repository, ");
            }
            output = output.Substring(0, output.Length - 2);
            return output.Equals(",") ? "" : output;
        }

    }
}
