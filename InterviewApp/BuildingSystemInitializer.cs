using System;
using InterviewApp.Devices;

namespace InterviewApp
{
    public static class BuildingSystemInitializer
    {
        public static BuildingSystemRoot Initialize()
        {
            var system = new BuildingSystemRoot();

            // Wire up events to handle automatic printing to stdout
            system.StructureChanged += () => HandleStructureChanged(system);

            system.DevicePropertyChanged += HandleDevicePropertyChanged;

            Console.WriteLine("Initializing Building Management System...");

            // Create and add groups (3 floors)
            string[] floors = { "floor_0", "floor_1", "floor_2" };
            foreach (var floor in floors)
            {
                system.AddGroupNode(new GroupNode(floor));
            }

            // Add ~3 devices to each group
            // --- Floor 0 Devices ---
            system.AddDeviceToGroup("floor_0", new Door("d_0_1", "Main Entrance Door", DoorState.Locked));
            system.AddDeviceToGroup("floor_0", new CardReader("cr_0_1", "Reception Card Reader", "A01234DE7FFF"));
            system.AddDeviceToGroup("floor_0", new Speaker("s_0_1", "Lobby Speaker", SpeakerSound.Music, 0.4));

            // --- Floor 1 Devices ---
            system.AddDeviceToGroup("floor_1", new Door("d_1_1", "Office 101 Door", DoorState.None));
            system.AddDeviceToGroup("floor_1", new Speaker("s_1_1", "Hallway Speaker", SpeakerSound.None, 0.2));
            system.AddDeviceToGroup("floor_1", new CardReader("cr_1_2", "Server Room Reader", "B123456789ABCDEF"));

            // --- Floor 2 Devices ---
            system.AddDeviceToGroup("floor_2", new Door("d_2_1", "Roof Access Door", DoorState.Locked | DoorState.OpenForTooLong));
            system.AddDeviceToGroup("floor_2", new Speaker("s_2_1", "Roof Speaker", SpeakerSound.Alarm, 0.8));
            system.AddDeviceToGroup("floor_2", new CardReader("cr_2_1", "Roof Card Reader", "1234567890123456"));

            return system;
        }
        
        private static void HandleStructureChanged(BuildingSystemRoot system)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[EVENT: Tree Structure Changed]");
            Console.ResetColor();
            Console.WriteLine(system.GetTreeRepresentation());
        }
        
        private static void HandleDevicePropertyChanged(BaseDeviceNode device, string propertyName)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[EVENT: Device Property Changed] -> Device: {device.Id} ({device.Name}), Property: '{propertyName}', State: {device.GetCurrentState()}");
            Console.ResetColor();
        }
    }
}