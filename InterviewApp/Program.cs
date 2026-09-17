using System;

namespace InterviewApp;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 1. Initialize system and wire events
        var system = BuildingSystemInitializer.Initialize();

        // 2. Run the automated simulation sequence
        SimulationRunner.Run(system);

        // 3. Hand over control to the interactive CLI loop
        InteractiveCli.Run(system);
    }
}