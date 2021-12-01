namespace Airports_CLI
{
    public interface ICommand
    {
        string GetDescription();

        void Start();
    }
}
