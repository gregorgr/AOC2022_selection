using System.Diagnostics;

namespace Day_14
{
    internal class Program
    {
        private static HashSet<(int, int)> obstacles = new HashSet<(int, int)>();
        private static int maxY = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("AOC 2022 Day 14: Regolith Reservoirm");

            string puzzleA = "input_day14demo.txt";
            string puzzleB = "input_day14.txt";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int rezult = part1(puzzleB);

            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;

            Console.WriteLine($"\nPart one!\nNumber of sand particles that come to rest: {rezult} in {elapsed.Seconds} seconds and {elapsed.Milliseconds} milliseconds");

            stopwatch.Restart();

            rezult = part2(puzzleB);

            Console.WriteLine($"\nPart two!\nNumber of sand particles that come to rest: {rezult} ) in {elapsed.Seconds} seconds and {elapsed.Milliseconds} milliseconds");
            stopwatch.Stop();
            elapsed = stopwatch.Elapsed;
        }


        static int part2(string file)
        {
            obstacles = new HashSet<(int, int)>();

            InputCreateMap(file);

            // Add the floor two units below the lowest obstacle
            maxY += 2;

            // Simulate falling sand
            int restingSand = SimulateFallingSandPartTwo();

            return restingSand;
        }

         static int part1(string file) {

            InputCreateMap(file);
            // Simulate falling sand
            int restingSand = SimulateFallingSandPartOne();
            return restingSand;
        }


        static void InputCreateMap(string file) {



            var input = File.ReadAllLines($"../../../{file}");

            // parsing input & create map of obsicles
            foreach (var line in input)
            {
                var points = line.Split(" -> ");
                for (int i = 0; i < points.Length - 1; i++)
                {
                    var start = ParsePoint(points[i]);
                    var end = ParsePoint(points[i + 1]);
                    AddObstacleLine(start, end);
                }
            }
        }

        // Simulate the falling of sand until the source is blocked
        private static int SimulateFallingSandPartTwo()
        {
            int sandCount = 0;
            bool isSourceBlocked = false;

            while (!isSourceBlocked)
            {
                int x = 500; // Sand starts falling from x=500
                int y = 0;

                // Simulate one sand particle
                while (true)
                {
                    // Check if the sand can move down, down-left, or down-right
                    if (y + 1 == maxY)
                    {
                        // The sand hits the floor
                        obstacles.Add((x, y));
                        sandCount++;
                        break;
                    }
                    else if (!obstacles.Contains((x, y + 1)))
                    {
                        // Move down
                        y++;
                    }
                    else if (!obstacles.Contains((x - 1, y + 1)))
                    {
                        // Move down-left
                        x--;
                        y++;
                    }
                    else if (!obstacles.Contains((x + 1, y + 1)))
                    {
                        // Move down-right
                        x++;
                        y++;
                    }
                    else
                    {
                        // Sand comes to rest
                        obstacles.Add((x, y));
                        sandCount++;

                        // Check if the source is blocked
                        if (x == 500 && y == 0)
                        {
                            isSourceBlocked = true;
                        }

                        break;
                    }
                }
            }
            return sandCount;
        }


        // Simulate the falling of sand
        private static int SimulateFallingSandPartOne()
        {
            int sandCount = 0;
            bool isSandFalling = true;

            while (isSandFalling)
            {
                int x = 500; // Sand starts falling from x=500
                int y = 0;

                // Simulate one sand particle
                while (true)
                {
                    if (y > maxY)
                    {
                        // Sand falls into the abyss (out of bounds)
                        isSandFalling = false;
                        break;
                    }

                    // Check if the sand can move down, down-left, or down-right
                    if (!obstacles.Contains((x, y + 1)))
                    {
                        // Move down
                        y++;
                    }
                    else if (!obstacles.Contains((x - 1, y + 1)))
                    {
                        // Move down-left
                        x--;
                        y++;
                    }
                    else if (!obstacles.Contains((x + 1, y + 1)))
                    {
                        // Move down-right
                        x++;
                        y++;
                    }
                    else
                    {
                        // Sand comes to rest
                        obstacles.Add((x, y));
                        sandCount++;
                        break;
                    }
                }
            }

            return sandCount;
        }

        // Add obstacles along a line between two points
        private static void AddObstacleLine((int x, int y) start, (int x, int y) end)
        {
            if (start.x == end.x)
            {
                // Vertical line
                for (int y = Math.Min(start.y, end.y); y <= Math.Max(start.y, end.y); y++)
                {
                    obstacles.Add((start.x, y));
                    maxY = Math.Max(maxY, y);
                }
            }
            else if (start.y == end.y)
            {
                // Horizontal line
                for (int x = Math.Min(start.x, end.x); x <= Math.Max(start.x, end.x); x++)
                {
                    obstacles.Add((x, start.y));
                    maxY = Math.Max(maxY, start.y);
                }
            }
        }


        // Parse a point in the form "x,y"
        private static (int x, int y) ParsePoint(string point)
        {
            var parts = point.Split(',');
            return (int.Parse(parts[0]), int.Parse(parts[1]));
        }


    }
}