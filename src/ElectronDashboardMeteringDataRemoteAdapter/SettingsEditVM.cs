namespace ElectronDashboardMeteringDataRemoteAdapter
{
    public class SettingsEditVM
    {
        public ConfigurationManager ConfigManager { get; set; } = new ConfigurationManager();

        public SettingsEditVM()
        {
            ConfigManager.Initialize();
        }

        public void Save()
        {
            ConfigManager.Save();
        }

        public string DataHost { get { return ConfigManager.DataHost; } set { ConfigManager.DataHost = value; } }
        public string IdentityServerHost { get { return ConfigManager.IdentityServerHost; } set { ConfigManager.IdentityServerHost = value; } }
        public string ClientId { get { return ConfigManager.ClientId; } set { ConfigManager.ClientId = value; } }
        public string ClientPassword { get { return ConfigManager.ClientPassword; } set { ConfigManager.ClientPassword = value; } }
    }
}