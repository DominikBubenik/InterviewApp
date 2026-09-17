using System;
using System.Threading;
using InterviewApp.Devices;

namespace InterviewApp
{
    public static class SimulationRunner
    {
        public static void Run(BuildingSystemRoot system)
        {
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
            
            system.RemoveDeviceFromGroup("floor_0", "s_1_1");
            Thread.Sleep(10000);
            
        }
    }
}