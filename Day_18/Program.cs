using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_18
{


    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AOC 2022 Day 18: 3D Boiling Boulders---");

            string file = "input-d18.txt";

            string demoPuzzle = "2,2,2\r\n1,2,2\r\n3,2,2\r\n2,1,2\r\n2,3,2\r\n2,2,1\r\n2,2,3\r\n2,2,4\r\n2,2,6\r\n1,2,5\r\n3,2,5\r\n2,1,5\r\n2,3,5";

            string[] myPuzzle = File.ReadAllLines($"../../../{file}");

            // Part One: demo
            int resultPartOne = CubeSurface(demoPuzzle.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            Console.WriteLine($"Part one!\nDEMO Result: {resultPartOne}");   // 62         

            resultPartOne = CubeSurface(myPuzzle);
            Console.WriteLine($"My result: {resultPartOne}"); // correct 4322

            // Part 2: Handling Air Pockets Inside the Lava Droplet
            // demo data
            int resultPartTwo = ExternalSurface(demoPuzzle.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));
            Console.WriteLine($"Part Two!\nDEMO Result: {resultPartTwo}"); // 58

            resultPartTwo = ExternalSurface(myPuzzle);
            Console.WriteLine($"My result: {resultPartTwo}"); // 2516


        }


        static int ExternalSurface(string[] puzzle)
        {

            List<(int x, int y, int z)> cubes = puzzle.Select(line => ParseCoordinates(line))
                                              .ToList();
            return CalculateExternalSurfaceArea(cubes);

        }

        static int CalculateExternalSurfaceArea(List<(int x, int y, int z)> cubes)
        {
            // all cubes
            HashSet<(int, int, int)> cubeSet = new HashSet<(int, int, int)>(cubes);
            // visited: A set that tracks which points in the 3D space (air) have been explored
            HashSet<(int, int, int)> visited = new HashSet<(int, int, int)>();
            int totalExposedFaces = 0;

            // BFS to Identify External Air
            // While exploring, every time the BFS touches a cube (a part of the lava droplet), it counts that as an exposed surface
            // https://mfmfazrin.medium.com/breadth-first-search-with-examples-using-c-378d336fd2be

            // Define the boundary for flood fill (adjust these based on the input ranges)
            int minX = cubes.Min(c => c.x) - 1;
            int maxX = cubes.Max(c => c.x) + 1;
            int minY = cubes.Min(c => c.y) - 1;
            int maxY = cubes.Max(c => c.y) + 1;
            int minZ = cubes.Min(c => c.z) - 1;
            int maxZ = cubes.Max(c => c.z) + 1;

            // a queue for BFS (flood fill starting outside the droplet)
            // https://www.geeksforgeeks.org/breadth-first-search-or-bfs-for-a-graph/
            Queue<(int, int, int)> queue = new Queue<(int, int, int)>();
            queue.Enqueue((minX, minY, minZ));

            // adjacency matrix -> where we can move in 3D grid
            var directions = new List<(int, int, int)>()
            {
                (1, 0, 0),
                (-1, 0, 0),
                (0, 1, 0),
                (0, -1, 0),
                (0, 0, 1),
                (0, 0, -1)
            };

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                // skip visited
                if (visited.Contains(current) || cubeSet.Contains(current))
                {
                    continue;
                }

                visited.Add(current);

                foreach (var (dx, dy, dz) in directions)
                {
                    var neighbor = (current.Item1 + dx, current.Item2 + dy, current.Item3 + dz);

                    //Checking Exposed Faces
                    if (neighbor.Item1 >= minX &&
                        neighbor.Item1 <= maxX &&
                        neighbor.Item2 >= minY &&
                        neighbor.Item2 <= maxY &&
                        neighbor.Item3 >= minZ &&
                        neighbor.Item3 <= maxZ)
                    {
                        if (cubeSet.Contains(neighbor))
                        {
                            // If the neighboring space is an occupied cube, we we add one face as exposed
                            totalExposedFaces++;
                        }
                        else if (!visited.Contains(neighbor))
                        {
                            // It's an air space, add it to the queue to explore
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }

            return totalExposedFaces;
        }

        static int CubeSurface(string[] puzzle)
        {

            List<(int x, int y, int z)> cubes = puzzle.Select(line => ParseCoordinates(line))
                                              .ToList();
            return getExposedSurfaceArea(cubes);
        }




        static int getExposedSurfaceArea(List<(int x, int y, int z)> cubes)
        {

            HashSet<(int, int, int)> cubeSet = new HashSet<(int, int, int)>();

            int result = 0;
            foreach (var cube in cubes)
            {
                // Console.WriteLine($"Cube: ({cube.x}, {cube.y}, {cube.z})");

                result += 6;
                // find all sides of a cube 1x1x1
                var cubeSides = new List<(int, int, int)>()
                {
                    (cube.x + 1, cube.y, cube.z),  // Right
                    (cube.x - 1, cube.y, cube.z),  // Left
                    (cube.x, cube.y + 1, cube.z),  // Up
                    (cube.x, cube.y - 1, cube.z),  // Down
                    (cube.x, cube.y, cube.z + 1),  // Forward
                    (cube.x, cube.y, cube.z - 1)   // Backward
                };


                foreach (var side in cubeSides)
                {
                    if (cubeSet.Contains(side))
                    {
                        // if side is in 3D space, we subtract it by 2          
                        result -= 2;
                    }
                }
                // add cube to a collection HashSet
                cubeSet.Add(cube);
            }

            return result;
        }

        // get tuple (x, y, z)
        static (int x, int y, int z) ParseCoordinates(string line)
        {
            var parts = line.Split(',').Select(int.Parse).ToArray();
            return (parts[0], parts[1], parts[2]);
        }

    }
}