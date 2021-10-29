using NJ07_Airports.Commands;
using NJ07_Airports.Commands.GeoLocation;
using NJ07_Airports.Logging;
using NJ07_Airports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NJ07_Airports
{
    public class Menu
    {
        List<ICommand> commands;

        public Menu(ExerciseResultsUtility resultUtility, GeoLocation geoLocationUtility)
        {
            commands = new List<ICommand>()
            {
                resultUtility,
                geoLocationUtility
            };
        }

        public void Start()
        {
            int selectedCommandId = -99;

            while (selectedCommandId != 0)
            {
                ShowCommands();
                selectedCommandId = GetPrompt();

                if (selectedCommandId >= 0 && commands.Count() > selectedCommandId)
                {
                    commands.ElementAt(selectedCommandId).Start();
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
            foreach (var item in commands)
            {
                Console.WriteLine($"{i} - {item.GetDescription()}");
                i++;
            }
        }
    }
}
