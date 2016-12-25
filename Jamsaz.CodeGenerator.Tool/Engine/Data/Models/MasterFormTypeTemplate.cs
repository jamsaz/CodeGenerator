using System.Collections.Generic;
using System.Collections.ObjectModel;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Jamsaz.CodeGenerator.Tool.Global;
using Jamsaz.CodeGenerator.Tool.Presentaion.ViewModels;
using Newtonsoft.Json.Linq;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Models
{
    public class MasterFormTypeTemplate
    {
        private readonly IConfigurationManager _configurationManager;

        public MasterFormTypeTemplate(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public ObservableCollection<MasterFormType> CreateDefaultMasterFormTypesTemplateList()
        {
            var json = _configurationManager.ConfigProvider.GetDatas();
            var result = new ObservableCollection<MasterFormType>();
            var masterFormTypes = json["Project"].Value<JArray>("MasterFormTemplateGenerateType");
            if (masterFormTypes != null)
            {
                foreach (var t in masterFormTypes)
                {
                    result.Add(new MasterFormType
                    {
                        Type = new MasterFormTypes(t["Name"].ToString()),
                        Value = t["Name"].ToString().Equals("GridViewForm")
                    });
                }
                return result;
            }
            return new ObservableCollection<MasterFormType>(new List<MasterFormType>());
        }
    }
}
