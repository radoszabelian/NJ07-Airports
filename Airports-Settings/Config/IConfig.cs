namespace Airports_Settings.Services
{
    public interface IConfig
    {
        public T GetConfigSection<T>(string sectionName);
    }
}
