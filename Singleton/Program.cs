using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace Singleton
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Object> _settings;
           

           


        }
    }

    public sealed class ConfigManager()
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
                lock (_instance)
                {
                    return _instance;
                }
            }

        }
        
        private void LoadFromJson()
        {
            using StreamReader reader = new("./Assets/settings.json");

            string text = reader.ReadToEnd();

            _json = JsonSerializer.Deserialize<Dictionary<string, object>>(text);

            FlattenDictionary(_json);
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

        public string 
    }

}
