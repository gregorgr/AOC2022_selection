namespace Day_09
{
    // Rope Bridge problem

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Rope Bridge problem AOC 2022 day9!");

            try
            {

                // Vhodni podatki

                // demo data
                //string fileA = "input-d9a-demo.txt";

                string fileA = "input-d9a.txt";
                // right answer is  6087
                // partOne(fileA);



                // ---Part Two-- -
                string fileB = "input-d9b.txt";
                // string fileB = "input-d9b-demo.txt";
                partTwo(fileB);

            }

            catch (Exception e)
            {

                throw;
            }



        }
        static void partTwo(string file)
        {
            // Vhodni podatki
            string[] moves = System.IO.File.ReadAllLines($"../../../{file}");

            int initialStateX = 15, initialStateY = 10;
            // Dolžina vrvi
            int ropeLength = 10;

            // Inicializacija vrvi (vsi deli na začetku pri (0,0))

            List<(int, int)> rope = new List<(int, int)>();
            for (int i = 0; i < ropeLength; i++)
            {
                rope.Add((initialStateX, initialStateY));
            }

            // Skup množice, da spremljamo unikatne pozicije
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            visited.Add(rope[ropeLength - 1]);


            // Premikanje po vhodnih podatkih
            foreach (var move in moves)
            {
                char direction = move[0];
                int steps = int.Parse(move.Substring(1));

                // PrintGrid(90, 35,  rope);
                // Console.WriteLine($"\n{move}");
                for (int i = 0; i < steps; i++)
                {
                    // Premik glave vrvi je podoben kot pri prvem delu
                    switch (direction)
                    {
                        case 'U': rope[0] = (rope[0].Item1, rope[0].Item2 + 1); break;
                        case 'D': rope[0] = (rope[0].Item1, rope[0].Item2 - 1); break;
                        case 'L': rope[0] = (rope[0].Item1 - 1, rope[0].Item2); break;
                        case 'R': rope[0] = (rope[0].Item1 + 1, rope[0].Item2); break;
                    }

                    for (int j = 1; j < ropeLength; j++)
                    {
                        // pozicija predhodnega dela
                        int hx = rope[j - 1].Item1, hy = rope[j - 1].Item2;

                        // bool touching = Math.Abs(x1 - x2) <= 1 & Math.Abs(y1 - y2) <= 1;
                        if (!isTouching(hx, hy, rope[j].Item1, rope[j].Item2))
                        {
                            int sign_x = (hx == rope[j].Item1) ? 0 : Math.Sign(hx - rope[j].Item1);
                            int sign_y = (hy == rope[j].Item2) ? 0 : Math.Sign(hy - rope[j].Item2);

                            rope[j] = (rope[j].Item1 + sign_x, rope[j].Item2 + sign_y);

                            // Dodajanje nove pozicije 9 na seznam obiskanih
                            if (j == 9)
                            {
                                visited.Add((rope[j].Item1, rope[j].Item2));
                            }
                        }
                    }
                    //PrintGrid(60, 25, rope);
                    //Console.WriteLine($"\n{move} - STEP {i+1}");
                }
            }

            // Izhod rezultata
            Console.WriteLine($"partTwo: Število unikatnih pozicij, ki jih je zadnji del vrvi obiskal: {visited.Count}");

        }

        static void PrintGrid(int sizeX, int sizeY, List<(int, int)> rope)
        {
            // Ustvari prazno mrežo
            char[,] grid = new char[sizeX, sizeY];

            try
            {

                for (int i = 0; i < sizeX; i++)
                {
                    for (int j = 0; j < sizeY; j++)
                    {
                        grid[i, j] = '.'; // Prazno mesto
                    }
                }

                // Nastavi pozicije vrvi
                for (int i = rope.Count - 1; i >= 0; i--)
                {
                    int x = rope[i].Item1;
                    int y = rope[i].Item2;

                    if (x >= 0 && x < sizeX && y >= 0 && y < sizeY)
                    {
                        grid[y, x] = i == 0 ? 'H' : (char)('0' + i); // 'H' za glavo, i za rep
                    }
                }

                // Izpis mreže
                for (int i = sizeX - 1; i >= 0; i--) // Obrnemo vrstni red vrstic za pravilen prikaz Y osi
                {
                    for (int j = 0; j < sizeY; j++)
                    {
                        Console.Write(grid[i, j]);
                    }
                    Console.WriteLine();
                }

            }

            catch (Exception e)
            {

                throw new Exception(e.ToString());
            }
        }



        static void partOne(string file)
        {


            string[] moves = System.IO.File.ReadAllLines($"../../../{file}");

            // Pozicija vozlišča
            int xH = 0, yH = 0, xT = 0, yT = 0;

            // Skup množice, da spremljamo unikatne pozicije
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            visited.Add((xT, xT));

            // Premikanje po vhodnih podatkih
            foreach (var move in moves)
            {
                char direction = move[0];
                int steps = int.Parse(move.Substring(1));

                for (int i = 0; i < steps; i++)
                {
                    switch (direction)
                    {
                        case 'U': yH++; break;
                        case 'D': yH--; break;
                        case 'L': xH--; break;
                        case 'R': xH++; break;
                    }

                    if (!isTouching(xH, yH, xT, yT))
                    {

                        if (Math.Abs(xH - xT) > 1)
                        {
                            xT = (xH - xT) > 0 ? xT + 1 : xT - 1;
                            yT = yH;
                        }
                        else if (Math.Abs(yH - yT) > 1)
                        {
                            yT = (yH - yT) > 0 ? yT + 1 : yT - 1;
                            xT = xH;
                        }


                        visited.Add((xT, yT));
                    }
                }
            }
            /*
            foreach (var obiskan in visited)
            {

                Console.WriteLine($" {obiskan.Item1}, {obiskan.Item2}");
            }
            */

            // Izhod rezultata
            Console.WriteLine($"partOne: Število unikatnih pozicij, ki so jih obiskali: {visited.Count}");

        }

        static Boolean isTouching(int x1, int y1, int x2, int y2)
        {
            bool touching = Math.Abs(x1 - x2) <= 1 & Math.Abs(y1 - y2) <= 1;
            // Console.WriteLine($"x={x} y={y} touching={touching}");
            return touching;
        }

    }
}