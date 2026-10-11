using System;
using System.Collections.Generic;

namespace Zork
{
    internal class Program
    {
        // Each initializer is one row. NORTH decreases the row to match Step 10's sample output.
        private static readonly Room[,] Rooms =
        {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") }
        };

        // Start in the middle of the grid: West of House.
        private static (int Row, int Column) Location = (1, 1);

        private static Room CurrentRoom => Rooms[Location.Row, Location.Column];

        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        private static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");
            InitializeRoomDescriptions();

            Room? previousRoom = null;
            Commands command = Commands.UNKNOWN;

            while (command != Commands.QUIT)
            {
                Console.WriteLine(CurrentRoom);

                // Show a description at startup and whenever the room changes.
                if (previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }

                Console.Write("> ");
                string? inputString = Console.ReadLine();

                // A closed input stream should end the game instead of looping forever.
                command = inputString == null ? Commands.QUIT : ToCommand(inputString.Trim());

                switch (command)
                {
                    case Commands.LOOK:
                        Console.WriteLine(CurrentRoom.Description);
                        break;

                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if (!Move(command))
                        {
                            Console.WriteLine("The way is shut!");
                        }
                        break;

                    case Commands.QUIT:
                        Console.WriteLine("Thank you for playing!");
                        break;

                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        private static Commands ToCommand(string commandString)
        {
            // TryParse supplies case-insensitive parsing without throwing on bad input.
            if (Enum.TryParse<Commands>(commandString, true, out Commands result)
                && Enum.IsDefined(typeof(Commands), result)
                && string.Equals(commandString, result.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return result;
            }

            // Also reject numbers and comma-separated values that Enum.TryParse accepts.
            return Commands.UNKNOWN;
        }

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid direction.");

            // Preserve Step 6's invalid-argument exception even in Release builds.
            if (!IsDirection(command))
            {
                throw new ArgumentException("Invalid direction.", nameof(command));
            }

            switch (command)
            {
                case Commands.NORTH when Location.Row > 0:
                    Location.Row--;
                    break;

                case Commands.SOUTH when Location.Row < Rooms.GetLength(0) - 1:
                    Location.Row++;
                    break;

                case Commands.EAST when Location.Column < Rooms.GetLength(1) - 1:
                    Location.Column++;
                    break;

                case Commands.WEST when Location.Column > 0:
                    Location.Column--;
                    break;

                default:
                    return false;
            }

            return true;
        }

        private static void InitializeRoomDescriptions()
        {
            var roomMap = new Dictionary<string, Room>();

            foreach (Room room in Rooms)
            {
                roomMap[room.Name] = room;
            }

            roomMap["Rocky Trail"].Description = "You are on a rock-strewn trail.";
            roomMap["South of House"].Description = "You are facing the south side of a white house.  There is no door here, and all the windows are barred.";
            roomMap["Canyon View"].Description = "You are at the top of the Great Canyon on its south wall.";
            roomMap["Forest"].Description = "This is a forest, with trees in all directions around you.";
            roomMap["West of House"].Description = "This is an open field west of a white house, with a boarded front door.";
            roomMap["Behind House"].Description = "You are behind the white house.  In one corner of the house there is a small window which is slightly ajar.";
            roomMap["Dense Woods"].Description = "This is a dimly lit forest, with large trees all around.  To the east, there appears to be sunlight.";
            roomMap["North of House"].Description = "You are facing the north side of a white house.  There is no door here, and all the windows are barred.";
            roomMap["Clearing"].Description = "You are in a clearing, with a forest surrounding you on the west and south.";
        }
    }
}
