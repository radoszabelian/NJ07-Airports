namespace Airports_Settings.Services
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Json;

    // https://pradeeploganathan.com/dotnet/configuration-in-a-net-core-console-application/
    public class ConfigService : IConfig
    {
        private string filePath;
        private IConfiguration configurationRoot;

        public ConfigService(string filePath)
        {
            this.filePath = filePath;
            this.LoadConfig();
        }

        public T GetConfigSection<T>(string sectionName)
        {
            return this.configurationRoot.GetSection(sectionName).Get<T>();
        }

        private void LoadConfig()
        {
            this.configurationRoot = new ConfigurationBuilder()
                .AddJsonFile(this.filePath, optional: false, reloadOnChange: true)
                .Build();
        }
    }
}
