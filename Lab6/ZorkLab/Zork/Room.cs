namespace Zork
{
    public class Room
    {
        public string Name { get; }
        public string Description { get; set; }

        public Room(string name, string description = "")
        {
            Name = name;
            Description = description;
        }

        // Console.WriteLine(room) will display its name instead of "Zork.Room".
        public override string ToString() => Name;
    }
}
