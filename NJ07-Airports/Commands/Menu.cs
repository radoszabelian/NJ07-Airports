namespace NJ07_Airports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using NJ07_Airports.Commands;
    using NJ07_Airports.Commands.GeoLocation;
    using NJ07_Airports.Logging;
    using NJ07_Airports.Model;

    /// <summary>
    /// This class shows a menu with options, and allows the user to choose what the program do next.
    /// </summary>
    public class Menu
    {
        private List<ICommand> commands;

        public Menu(ExerciseResultsUtility resultUtility, GeoLocation geoLocationUtility)
        {
            this.commands = new List<ICommand>()
            {
                resultUtility,
                geoLocationUtility,
            };
        }

        /// <summary>
        /// Starts the menu loop. Showing menu - User chooses something - Doing stuff - Asking again.
        /// </summary>
        public void Start()
        {
            int selectedCommandId = -99;

            while (selectedCommandId != 0)
            {
                this.ShowCommands();
                selectedCommandId = this.GetPrompt();

                if (selectedCommandId >= 0 && this.commands.Count() > selectedCommandId)
                {
                    this.commands.ElementAt(selectedCommandId).Start();
                }
                else
                {
                    Console.WriteLine("Command does not exists!");
                }
            }
        }

        private int GetPrompt()
        {
            Console.WriteLine();
            Console.Write("Which command to execute?");

            try
            {
                int result = int.Parse(Console.ReadLine());
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        private void ShowCommands()
        {
            var i = 0;
            foreach (var item in this.commands)
            {
                Console.WriteLine($"{i} - {item.GetDescription()}");
                i++;
            }
        }
    }
}
