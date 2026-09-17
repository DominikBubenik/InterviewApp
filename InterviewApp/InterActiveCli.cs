using System;

namespace InterviewApp
{
    public static class InteractiveCli
    {
        public static void Run(BuildingSystemRoot system)
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
}