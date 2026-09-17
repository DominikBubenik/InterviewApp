using System;
using System.Threading;
using InterviewApp;
using InterviewApp.Devices;

namespace InterviewApp;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        var system = new BuildingSystemRoot();

        // 1. Wire up events to handle automatic printing to stdout
        system.StructureChanged += () =>
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n[EVENT: Tree Structure Changed]");
            Console.ResetColor();
            Console.WriteLine(system.GetTreeRepresentation());
        };

        system.DevicePropertyChanged += (device, propertyName) =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[EVENT: Device Property Changed] -> Device: {device.Id} ({device.Name}), Property: '{propertyName}', State: {device.GetCurrentState()}");
            Console.ResetColor();
        };

        Console.WriteLine("Initializing Building Management System...");

        // 2. Create and add groups (3 floors)
        string[] floors = { "floor_0", "floor_1", "floor_2" };
        foreach (var floor in floors)
        {
            system.AddGroupNode(new GroupNode(floor));
        }

        // 3. Add ~3 devices to each group
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

        // 4. Automated Simulation Sequence (Testing changes & movements)
        Console.WriteLine("\n--- Starting Automated Simulation ---");
        Thread.Sleep(10000);

        // Change property on a door
        var door = system.FindDeviceById("d_0_1") as Door;
        if (door != null)
        {
            door.Locked = false;
            Thread.Sleep(8000);
            door.Open = true;
            Thread.Sleep(8000);
        }

        // Change property on a speaker
        var speaker = system.FindDeviceById("s_0_1") as Speaker;
        if (speaker != null)
        {
            speaker.Volume = 0.75;
            Thread.Sleep(8000);
        }

        // Move a device from floor_1 to floor_0
        Console.WriteLine("\n> Moving device 's_1_1' from 'floor_1' to 'floor_0'...");
        system.MoveDevice("floor_1", "floor_0", "s_1_1");
        Thread.Sleep(10000);

        // 5. Interactive Mode Loop
        RunInteractiveMode(system);
    }

    static void RunInteractiveMode(BuildingSystemRoot system)
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n--- INTERACTIVE COMMANDS ---");
            Console.WriteLine("  print");
            Console.WriteLine("  addgroup <groupName>");
            Console.WriteLine("  rmgroup <groupName>");
            Console.WriteLine("  move <fromGroup> <toGroup> <deviceId>");
            Console.WriteLine("  exit");
            Console.Write("\nEnter command: ");

            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) continue;

            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string cmd = parts[0].ToLower();

            try
            {
                switch (cmd)
                {
                    case "print":
                        Console.WriteLine(system.GetTreeRepresentation());
                        break;
                    case "addgroup":
                        system.AddGroupNode(new GroupNode(parts[1]));
                        break;
                    case "rmgroup":
                        system.RemoveGroupNode(parts[1]);
                        break;
                    case "move":
                        system.MoveDevice(parts[1], parts[2], parts[3]);
                        break;
                    case "exit":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error]: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}