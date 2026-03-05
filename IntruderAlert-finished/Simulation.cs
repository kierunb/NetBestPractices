using IntruderAlert;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntruderAlertOptimized;

public class Simulation
{
    public void Start()
    {
        var room = new Room("gallery");
        var r = new Random();
        int limit = 100;
        int counter = 0;

        room.TakeMeasurements(
            () =>
            {
                ref readonly DebounceMeasurement debounce = ref room.Debounce;
                Console.WriteLine(debounce.ToString());
                ref readonly AverageMeasurement average = ref room.Average;
                Console.WriteLine(average.ToString());
                Console.WriteLine();
                counter++;
                return counter < limit;
            });

        counter = 0;
        room.TakeMeasurements(
            () =>
            {
                ref readonly DebounceMeasurement debounce = ref room.Debounce;
                Console.WriteLine(debounce.ToString());
                ref readonly AverageMeasurement average = ref room.Average;
                Console.WriteLine(average.ToString());
                room.Intruders += (room.Intruders, r.Next(5)) switch
                {
                    ( > 0, 0) => -1,
                    ( < 3, 1) => 1,
                    _ => 0
                };

                Console.WriteLine(room.ToString());
                Console.WriteLine();
                counter++;
                return counter < limit;
            });
    }
}
