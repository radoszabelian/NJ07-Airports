namespace Airports_Logic.Services
{
    public interface ICommand
    {
        string GetDescription();

        void Start();
    }
}
