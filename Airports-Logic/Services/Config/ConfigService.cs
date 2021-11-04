namespace Airports_Logic.Services
{
    using Airports_Logic.Model;
    using Microsoft.Extensions.Configuration;

    // https://pradeeploganathan.com/dotnet/configuration-in-a-net-core-console-application/
    public class ConfigService : IConfig
    {
        private InputPathsConfiguration config;
        private string filePath;

        public ConfigService(string filePath)
        {
            this.filePath = filePath;
        }

        public InputPathsConfiguration Config
        {
            get
            {
                if (this.config == null)
                {
                    this.LoadConfig();
                }

                return this.config;
            }
        }

        private void LoadConfig()
        {
            IConfiguration configurationRoot = new ConfigurationBuilder()
                .AddJsonFile(this.filePath, optional: false, reloadOnChange: true)
                .Build();

            this.config = configurationRoot.GetSection("InputPaths").Get<InputPathsConfiguration>();
        }
    }
}
