using IntruderAlert;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntruderAlertStarter;

public class Simulation
{
    public void Start()
    {

        var room = new Room("gallery");
        var r = new Random();
        int limit = 100;
        int counter = 0;

        room.TakeMeasurements(
            m =>
            {
                Console.WriteLine(room.Debounce);
                Console.WriteLine(room.Average);
                Console.WriteLine();
                counter++;
                return counter < limit;
            });

        counter = 0;
        room.TakeMeasurements(
            m =>
            {
                Console.WriteLine(room.Debounce);
                Console.WriteLine(room.Average);
                room.Intruders += (room.Intruders, r.Next(5)) switch
                {
                    ( > 0, 0) => -1,
                    ( < 3, 1) => 1,
                    _ => 0
                };

                Console.WriteLine($"Current intruders: {room.Intruders}");
                Console.WriteLine($"Calculated intruder risk: {room.RiskStatus}");
                Console.WriteLine();
                counter++;
                return counter < limit;
            });

    }
}


