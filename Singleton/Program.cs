using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Singleton
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public sealed class ConfigManager
    {
        private static ConfigManager _instance;
        private static readonly object _instanceLock = new object();
        private Dictionary<string, object> _settings;
        private Dictionary<string, object> _json;

        private ConfigManager()
        {
            LoadFromJson();
        }

        public static ConfigManager? Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null) return new ConfigManager();
                    return _instance;
                }
            }

        }
        
        private void LoadFromJson()
        {
            try
            {
                using StreamReader reader = new("./Assets/settings.json");

                string text = reader.ReadToEnd();

                _json = JsonSerializer.Deserialize<Dictionary<string, object>>(text);

                FlattenDictionary(_json);
            }catch(FileNotFoundException e)
            {
                Console.WriteLine("File Not Found.");
                return;
            }
            catch (JsonException)
            {
                Console.WriteLine("Json Exception");
            }
        }

        private void SaveToJson()
        {
            lock (_instanceLock) 
            {
                string json = JsonSerializer.Serialize(_settings);

                File.WriteAllText("./Assets/settings.json", json);
            }
        }

        private void FlattenDictionary(Dictionary<string, object> source, string prefix = "")
        {
            foreach (var kvp in source)
            {
                string key = prefix == "" ? kvp.Key : $"{prefix}:{kvp.Key}";

                if (kvp.Value is Dictionary<string, object> nested)
                {
                    FlattenDictionary(nested, key);
                }
                else
                    _settings[key] = kvp.Value?.ToString() ?? "";
            }
        }

        public string GetString(string key)
        {
            lock (_instanceLock)
            {
                if (_settings.ContainsKey(key))
                    return _settings[key].ToString();

                return "Key Not Found.";
            }
        }

        public void SetString(string key, string value)
        {
            lock (_instanceLock)
            {
                _settings[key] = value;
                SaveToJson();
            }
        }

        public bool TryGetString (string key, out string value)
        {
            value = "";

            if (!_settings.ContainsKey(key))
                return false;
            return true;
        }
    }

}
