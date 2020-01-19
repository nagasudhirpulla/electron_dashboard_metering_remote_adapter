using Newtonsoft.Json;
using System;
using System.IO;

namespace ElectronDashboardMeteringDataRemoteAdapter
{
    public class ConfigurationManager
    {
        public string DataHost { get; set; } = "host.address.com";
        public string IdentityServerHost { get; set; } = "idserver.address.com";
        public string ClientId { get; set; } = "ClientId";
        public string ClientPassword { get; set; } = "ClientPassword";
        public string configFilename = "meterAdapterConfig.json";

        public string GetAppDataPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public void Initialize()
        {
            string path = GetAppDataPath();
            string jsonPath = path + "\\" + configFilename;

            if (File.Exists(jsonPath))
            {
                ConfigurationManager configObj = JsonConvert.DeserializeObject<ConfigurationManager>(File.ReadAllText(jsonPath));
                CloneFromObject(configObj);
            }
            else
            {
                Save();
            }
        }

        public void Save()
        {
            string ConfigJSONStr = JsonConvert.SerializeObject(this, Formatting.Indented);

            string path = GetAppDataPath();
            string jsonPath = path + "\\" + configFilename;

            File.WriteAllText(jsonPath, ConfigJSONStr);
        }

        public void CloneFromObject(ConfigurationManager configuration)
        {
            IdentityServerHost = configuration.IdentityServerHost;
            DataHost = configuration.DataHost;
            ClientId = configuration.ClientId;
            ClientPassword = configuration.ClientPassword;
        }
    }
}