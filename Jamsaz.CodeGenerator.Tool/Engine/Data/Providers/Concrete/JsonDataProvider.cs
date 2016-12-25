using System.IO;
using Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Abstracts;
using Newtonsoft.Json;

namespace Jamsaz.CodeGenerator.Tool.Engine.Data.Providers.Concrete
{
    public class JsonDataProvider<TResult> : IReadableDataProvider<TResult>, IDataProvider
    {
        private TResult _currentJObject;
        private readonly string _filePath;
        public JsonDataProvider(string filePath)
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

        private void LoadData()
        {
            if (string.IsNullOrEmpty(_filePath))
            {
                _currentJObject = default(TResult);
            }
            else if (File.Exists(_filePath))
            {
                var contents = File.ReadAllText(_filePath);
                _currentJObject = string.IsNullOrEmpty(contents)
                    ? default(TResult)
                    : JsonConvert.DeserializeObject<TResult>(contents);
            }
            else
            {
                _currentJObject = default(TResult);
            }
        }

    }

    public class JsonDataProvider : IWriteableDataProvider, IDataProvider
    {
        public string FilePath { get; set; }

        public JsonDataProvider()
        {
            FilePath = "";
        }
        public JsonDataProvider(string filePath)
        {
            FilePath = filePath;
        }

        public void SaveData(object data)
        {
            var strJson = JsonConvert.SerializeObject(data);
            File.WriteAllText(FilePath, strJson);
        }
    }
}