using System;
using System.Collections.Generic;
using System.Linq;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Global.Helpers;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models.Contextes
{
    public class NameSpaceContext
    {
        private readonly IObjectContext _objectContext;
        private readonly IMetadataProvider _metadataProvider;
       
        public NameSpaceContext(IMetadataProvider metadataProvider, IObjectContext objectContext)
        {
            _metadataProvider = metadataProvider;
            _objectContext = objectContext;
        }

        public object GetNameSpaces(JToken item)
        {
            var namespaces = new List<object>();
            var project = _metadataProvider.GetMetaData();
            foreach (var n in project.GetPropertyValueAsEnumerate("NameSpaces"))
            {
                var objects = (List<object>)_objectContext.GetObjects(item);
                if (objects.Count > 0)
                {
                    namespaces.Add(new
                    {
                        ProjectName = project.GetPropertyValueAsString("Name"),
                        Name = n.GetPropertyValueAsString("Name"),
                        DisplayName = n.GetPropertyValueAsString("DisplayName"),
                        CapitalizedNameSpace = n.GetPropertyValueAsString("Name").Capitalize(),
                        Objects = objects.Where(x => x.GetPropertyValueAs<int>("ParentModule") == n.GetPropertyValueAs<int>("ModuleId"))
                    });
                }
            }
            return namespaces;
        }

        public object GetNameSpacesForMenu(JToken item)
        {
            var namespaces = new List<object>();
            var ns = (IEnumerable<object>)GetNameSpaces(item);
            foreach (var source in ns)
            {
                var objects = new List<object>();
                foreach (var o in source.GetPropertyValueAsEnumerate("Objects").Where(o => o.GetPropertyValueAs<bool>("MasterForm") && o.GetPropertyValueAs<bool>("VisibleForm")))
                {
                    foreach (var masterForm in o.GetPropertyValueAsEnumerate("MasterFormTypes").Where(mf => mf.GetPropertyValueAs<bool>("Value")))
                    {
                        objects.Add(new
                        {
                            DisplayName = o.GetPropertyValueAsString("DisplayName"),
                            Name = $"{o.GetPropertyValueAsString("SingularizedName")}{masterForm.GetPropertyValueAsObject("Type").GetPropertyValueAsString("Name")}"
                        });
                    }
                }
                if (objects.Count > 0)
                {
                    namespaces.Add(new
                    {
                        DisplayName = source.GetPropertyValueAsString("DisplayName"),
                        Objects = objects
                    });
                }
            }
            return new
            {
                ProjectName = _metadataProvider.GetMetaData().GetPropertyValueAsString("Name"),
                NameSpaces = namespaces
            };
        }

    }
}
