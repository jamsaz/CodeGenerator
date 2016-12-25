using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Jamsaz.CodeGenerator.Tool.Engine.Data.DataConverters;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Models;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Concrete;
using Jamsaz.CodeGenerator.Tool.Global;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Generator
{
    public class MetadataGenerator : IMetadataGenerator
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IWriteableDataProvider _generateToWriteableDataProvider;
        private readonly MasterFormTypeTemplate _formTypeTemplate;

        public MetadataGenerator(IWriteableDataProvider generateToWriteableDataProvider, IConfigurationManager configurationManager)
        {
            _generateToWriteableDataProvider = generateToWriteableDataProvider;
            _configurationManager = configurationManager;
            _formTypeTemplate = new MasterFormTypeTemplate(configurationManager);
        }

        public void Generate()
        {
            var dataProvider = new DataProvider(_configurationManager);
            var metatDataProvider = dataProvider.GetDataProvider();
            var metadata = metatDataProvider as IReadableDataProvider<IQueryable<Data.Models.Data>>;
            if (metadata == null) return;
            var query = metadata.GetDatas();
            var projectSchemaes =
                query.GroupBy(x => x.SchemaId)
                    .Select(x => new { Schema = x.FirstOrDefault(), Objects = x })
                    .Select(x => new NameSpace
                    {
                        SchemaName = x.Schema.SchemaName,
                        Name = x.Schema.SchemaName,
                        DisplayName = x.Schema.SchemaName,
                        ModuleName = x.Schema.SchemaName,
                        ModuleId = x.Schema.SchemaId,
                        Objects =
                            x.Objects.GroupBy(o => o.TableId)
                                .Select(o => new { Object = o.FirstOrDefault(), Properties = o })
                                .Select(o => new Object
                                {
                                    PrimaryKey = o.Object.PrimaryKeyName,
                                    Id = o.Object.TableId,
                                    Name = o.Object.TableName,
                                    DisplayName = o.Object.TableName,
                                    NameSpace = o.Object.SchemaName,
                                    Layout = LayoutTypes.None,
                                    Properties =
                                        o.Properties.GroupBy(p => p.ColumnId)
                                            .Select(p => new { Property = p.FirstOrDefault() })
                                            .Select(p => new Property
                                            {
                                                EnumItems = new ObservableCollection<EnumItem>(),
                                                CapitalizeNameSpace = o.Object.SchemaName.Capitalize(),
                                                Name = p.Property.ColumnName,
                                                SQLDataType = p.Property.DataType,
                                                DataType =
                                                    p.Property.IsForeignKey
                                                        ? $"{p.Property.ForeignKeyRefrenceSchema.Capitalize()}.{p.Property.ForeignKeyRefrenceTable.Singularize()}"
                                                        : p.Property.DataType,
                                                ControlType =
                                                    ControlTypeConverter.GetControlTypeFromDataType(p.Property.DataType, p.Property.IsForeignKey),
                                                Nullable =
                                                    p.Property.DataType != "string" &&
                                                    p.Property.Nullable,
                                                Default = p.Property.Default,
                                                IsComputed = p.Property.IsComputed,
                                                DisplayName = p.Property.ColumnName,
                                                Visible = true,
                                                SQLName = p.Property.ColumnName,
                                                Optional = false,
                                                ColumnIndex = 0,
                                                RowIndex = (p.Property.ColumnId - 1),
                                                Priority = p.Property.ColumnId,
                                                Attributes = "",
                                                Check = "",
                                                MaxLength = p.Property.MaxLength,
                                                ParentObjectName =
                                                    p.Property.IsForeignKey
                                                        ? p.Property.ForeignKeyRefrenceTable
                                                        : "",
                                                ParentObjectPrimaryKeyName = p.Property.ForeignKeyRefrenceColumn,
                                                ParentObjectNameSpace = p.Property.ForeignKeyRefrenceSchema.Capitalize(),
                                                InGrid = true,
                                                InLookup = true
                                            }),
                                    Relations =
                                        o.Properties.GroupBy(p => p.ColumnId)
                                            .Select(p => new { Property = p.FirstOrDefault() })
                                            .Where(p => p.Property.IsForeignKey)
                                            .Select(f => new
                                            {
                                                Name = f.Property.ForeignKeyName,
                                                RefrenceObjectName = f.Property.ForeignKeyRefrenceTable,
                                                RefrenceObjectId = f.Property.ForeignKeyRefrenceTableId,
                                                RefrenceObjectSchemaName = f.Property.ForeignKeyRefrenceSchema,
                                                RefrencePropertyName = f.Property.ForeignKeyRefrenceColumn,
                                                ParentObjectName = f.Property.TableName,
                                                ParentObjectSchemaName = f.Property.ForeignKeyParentSchema,
                                                ParentColumnName = f.Property.ColumnName
                                            }),
                                    VisibleForm = true,
                                    Visible = true,
                                    Path = "",
                                    Add = true,
                                    Delete = true,
                                    Edit = true,
                                    AddButtonText = "اضافه کردن",
                                    EditButtonText = "ویرایش",
                                    DeleteButtonText = "حذف",
                                    SaveButtonText = "ذخیره سازی",
                                    CancelButtonText = "انصراف",
                                    SaveMessageText = "آیا مایل به ذخیره سازی اطلاعات هستید؟",
                                    SaveMessageTitle = "سوال",
                                    DeleteMessageText = "آیا مایل به حذف اطلاعات انتخاب شده هستید؟",
                                    DeleteMessageTitle = "سوال",
                                    GridRowNotSelectedMessage = "لطفا یک سطر را انتخاب کنید",
                                    GridRowDeletedMessage = "اطلاعات انتخاب شده حذف شد",
                                    InlineEditing = true,
                                    MasterForm = false,
                                    MasterFormTypes = _formTypeTemplate.CreateDefaultMasterFormTypesTemplateList(),
                                    ParentModule = x.Schema.SchemaId,
                                    ColumnOfForm = 1,
                                    RowOfForm =
                                        o.Properties.GroupBy(p => p.ColumnId)
                                            .Select(p => new { Property = p.FirstOrDefault() })
                                            .Count(),
                                    Description = "",
                                    Title = ""
                                })
                    }).ToList();
            var config = _configurationManager.ConfigProvider.GetDatas();
            _generateToWriteableDataProvider.SaveData(new List<object>
            {
                new Project {Name = config["Project"].Value<string>("Name"), NameSpaces = projectSchemaes}
            });
        }
    }
}
