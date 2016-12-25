using System.IO;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Newtonsoft.Json;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Concrete
{
    public class JsonConfigProvider<TResult> : IConfigProvider<TResult>
    {
        private TResult _currentJObject;
        private readonly string _filePath;

        public JsonConfigProvider(string filePath)
        {
            _filePath = filePath;
            LoadData();
        }

        public bool HasCash { get; set; }

        public TResult GetDatas()
        {
            if (_currentJObject == null || HasCash == false)
                LoadData();
            return _currentJObject;
        }

        public void SaveData(object data)
        {
            var strJson = JsonConvert.SerializeObject(data);
            File.WriteAllText(_filePath, strJson);
        }

        private void LoadData()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                _currentJObject = default(TResult);
                return;
            }
            var contents = File.ReadAllText(_filePath);
            _currentJObject = string.IsNullOrEmpty(contents)
                ? default(TResult)
                : JsonConvert.DeserializeObject<TResult>(contents);
        }
    }
}
