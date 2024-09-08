namespace AOC2022_day_17
{
    internal class ProgramXX
    {

        // Definicija padanja kamnov in oblik skal
        static readonly List<int[,]> shapes = new List<int[,]>
        {
            new int[,] { { 1, 1, 1, 1 } },  // Horizontalna vrstica
            new int[,] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } },  // + Plus
            new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 } },  // obrnjen L
           // new int[,] {  { 0, 0, 1 }, { 0, 0, 1 }, { 1, 1, 1 } },  // obrnjen L
            new int[,] { { 1 }, { 1 }, { 1 }, { 1 } },  // Vertikalna vrstica I
            new int[,] { { 1, 1 }, { 1, 1 } }  // Kvadrat
        };

        static void Mainx(string[] args)
        {
            // --- Day 17: Pyroclastic Flow ---

            Console.WriteLine("AOC 2022 Day 17: Pyroclastic Flow aka Tetris---");

            // C: \Users\grego\source\repos\Algoritmi_AOC2022_selection\Algoritmi_AOC2022_selection.sln
            string file = "input-d17a.txt";
            //string file = "input-d17-demo.txt";
            // right answer is  3171

            int towerWidth = 7;  // Širina stolpa
            int numberOfRocks =2022; // 2022;//  10; // 2022;  // Število kamnov, ki padejo
            long numberOfRocksB = 1000000000000; //  1.000.000.000.000; 2022;//  10; // 2022;  // Število kamnov, ki padejo

            partOne(file, towerWidth, numberOfRocks);
            /*
            total_rocks: 2573
            height_gain_in_cycle: 2729
            rocks_in_cycle: 1720
            skipped_cycles: 581395347
            total_rocks: 999999999413
            remaining_cicles: 587
            1586627906921
             * */



            long height = partTwo(file, towerWidth, numberOfRocksB);
            long newResultOfPartTwo = 1586627906921 - height;

            // pravilni je 1586627906921
            // 1552769679301
            // total hight 1552769679301
            Console.WriteLine($"Višina stolpa po {numberOfRocksB} skalah je: {height} off for {newResultOfPartTwo}");

            if (file == "input-d17-demo.txt")
            {
                Console.WriteLine($"Demo 1514285714288");
            }
            else {
                Console.WriteLine("1586627906921 OK");
                
            }
            Console.WriteLine($"{height} yours");


        }
        static long partTwo(string file, int towerWidth, long targetRocks)
        {
            Console.WriteLine("Part Two:");
            //string instructions = File.ReadAllText($"../../../{file}");
            string jets = File.ReadAllText($"../../../{file}").Trim();



            HashSet<(int, int)> occupied = new HashSet<(int, int)>();

            long totalHight = 0;


            return totalHight;
        }

        static long partTwoB(string file, int towerWidth, long targetRocks)
        {
            Console.WriteLine("Part Two:");
            //string instructions = File.ReadAllText($"../../../{file}");
            string jets = File.ReadAllText($"../../../{file}").Trim();

            

            HashSet<(int, int)> occupied = new HashSet<(int, int)>();
            for (int x = 0; x < towerWidth; x++)
            {
                occupied.Add((x, 0));
            }

            long totalHight = 0;
            long currentHeight = 0;
            // int highest = 0;
            
            var states = new Dictionary<(List<int>, int, int), (int, long)>();
            // rockCount long totalRocks = 0;
            int currentShapeIndex = 0;
            bool cycleFound = false;
            int heightGainInCycle = 0;
            long rocksInCycle = 0;
            long skippedCycles = 0;

            
            long rockCount = 0;
            char jet;
            int jetIndex = 0;
            char[] jetPattern = jets.ToCharArray();


            while (rockCount < targetRocks)
            {
                var shape = shapes[currentShapeIndex];
                

                int shapeWidth = shape.GetLength(1);
                int shapeHeight = shape.GetLength(0);
                int x = 2;
                int y = (int)totalHight + 4;

                bool isFalling = true;
                while (isFalling)
                {

                    // najdi ukaz levo ali desno
                    jet = jetPattern[jetIndex];
                    jetIndex = (jetIndex + 1) % jetPattern.Length;

                    // executedShapeInstructions += jet;
                    // executedInstructions += jet;

                    // se premakne ali ne?
                    if (jet == '<' && CanMove(shape, x - 1, y, occupied, towerWidth))
                    {
                        x--;
                    }
                    else if (jet == '>' && CanMove(shape, x + 1, y, occupied, towerWidth)) {
                        x++;
                    }
                        

                    //if (y > currentHeight || CanMove(shape, x, y - 1, occupied, towerWidth))
                    //if (CanMove(shape, x, y - 1, occupied, shapeWidth))
                    if (y > currentHeight || CanMove(shape, x, y - 1, occupied, towerWidth))
                    {
                        y--;
                    }
                    else
                    {
                        AddShapeToOccupied(shape, x, y, occupied);
                        isFalling = false;
                        totalHight = Math.Max(totalHight, y + shapeHeight - 1);
                    }
                }

                rockCount++;

                if (!cycleFound)
                {
                    var state = new List<int>();
                    for (int i = 0; i < towerWidth; i++)
                    {
                        state.Add(occupied.Where(x => x.Item1 == i).Select(x => x.Item2).DefaultIfEmpty(0).Max());
                    }

                    int lowest = state.Min();
                    state = state.Select(x => x - lowest).ToList();
                    var stateKey = (state, currentShapeIndex, jetIndex);

                    if (states.ContainsKey(stateKey))
                    {
                        var (oldHighest, oldRocks) = states[stateKey]; //
                        heightGainInCycle = (int) totalHight - oldHighest;//
                        rocksInCycle = rockCount - oldRocks;
                        skippedCycles = (targetRocks - rockCount) / rocksInCycle;
                        rockCount += skippedCycles * rocksInCycle;
                        cycleFound = true;
                    }
                    else
                    {
                        states[stateKey] = ((int)totalHight, rockCount);
                    }
                }

                //r = (r + 1) % shapes.Count;
                currentShapeIndex = (currentShapeIndex + 1) % shapes.Count;
            }

            totalHight += (skippedCycles * heightGainInCycle);
            return totalHight;

            /*
             * 
            cycleNotFound = false;
            long cycleLength = rockCount - startRockCount;
            int heightGainPerCycle = currentHeight - startHeight;

            long cyclesRemaining = (targetRocks - rockCount) / cycleLength;
            // long totalHeight = currentHeight + cyclesRemaining * heightGainPerCycle;
            // long totalHeight = cyclesRemaining * heightGainPerCycle;
            ///***totalCycleHeight = currentHeight + cyclesRemaining * heightGainPerCycle;
            long totalHeight = currentHeight + cyclesRemaining * heightGainPerCycle;

            long remainingRocks = (targetRocks - rockCount) % cycleLength;
            Console.WriteLine($"Cycle Found at rockCount {rockCount}");
            Console.WriteLine($" stateKey= {stateKey}");
            Console.WriteLine($" cycleLength  {cycleLength}");
            Console.WriteLine($" heightGainPerCycle  {heightGainPerCycle}");
            Console.WriteLine($" totalHeight  {totalCycleHeight}");
            Console.WriteLine($" cyclesRemaining  {cyclesRemaining}");
            Console.WriteLine($" remainingRocks   {remainingRocks}");
            */





        }




        static string GetTopographyKey(HashSet<(int, int)> occupied, int currentHeight, int width)
        {
            int depth = 10;  // A small depth to capture the topography
            var topKey = new char[width * depth];
            Array.Fill(topKey, '.');

            foreach (var (x, y) in occupied)
            {
                if (currentHeight - y < depth)
                {
                int row = currentHeight - y;
                int index = row * width + x;
                topKey[index] = '#';
                }
            }

            return new string(topKey);
        }

    static void partOne(string file, int towerWidth, int numberOfRocks)
    {
        Console.WriteLine("Part One:");
        //string instructions = File.ReadAllText($"../../../{file}");
        string jets = File.ReadAllText($"../../../{file}").Trim();
            //PrintJetPattern(instructions);

            // zasedeni stolpci
        HashSet<(int, int)> occupied = new HashSet<(int, int)>();

        int currentHeight = 0;
        int currentShapeIndex = 0;

        int jetIndex = 0;
        char[] jetPattern = jets.ToCharArray();

        string executedInstructions = "";

        for (int i = 0; i < numberOfRocks; i++)
        {

            var shape = shapes[currentShapeIndex];
            currentShapeIndex = (currentShapeIndex + 1) % shapes.Count;


            int x = 2; // Začetna X pozicija (dve enoti stran od levega roba)
            int y = currentHeight + 3; // Začetna Y pozicija (tri enote nad najvišjim delom stolpa)

            bool isFalling = true;
            string executedShapeInstructions = "";

            while (isFalling)
            {
                // najdi ukaz levo ali desno
                char jet = jetPattern[jetIndex];
                jetIndex = (jetIndex + 1) % jetPattern.Length;

                executedShapeInstructions += jet;
                executedInstructions += jet;

                // se premakne ali ne?
                if (jet == '<' && CanMove(shape, x - 1, y, occupied, towerWidth))
                    x--;
                else if (jet == '>' && CanMove(shape, x + 1, y, occupied, towerWidth))
                    x++;


                // Premik navzdol

                if (y > currentHeight || CanMove(shape, x, y - 1, occupied, towerWidth))
                    y--;
                else
                    isFalling = false;

            }

            // konec padanja, dodam v seznam
            // Dodajanje trenutne oblike v množico zasedenih mest
            AddShapeToOccupied(shape, x, y, occupied);

            // izpiše komande
            // PrintJetPattern(executedShapeInstructions);
            // Posodobi višino stolpa
            currentHeight = Math.Max(currentHeight, y + shape.GetLength(0));
            //PrintTower(occupied, towerWidth);

        }

        // izpiše komande
        //    PrintJetPattern(executedInstructions);

        Console.WriteLine($"Višina stolpa po {numberOfRocks} skalah je: {currentHeight}");
        //      PrintTower(occupied, towerWidth);

    }


    static bool CanMove(int[,] shape, int x, int y, HashSet<(int, int)> occupied, int towerWidth)
    {
        // Preverimo vse dele oblike y = i
        for (int i = 0; i < shape.GetLength(0); i++)
        {
            // reverimo še za x = j
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                if (shape[i, j] == 1)
                {
                    int newX = x + j;
                    int newY = y + i;

                    if (newX < 0 || newX >= towerWidth || newY < 0 || occupied.Contains((newX, newY)))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }


    static void AddShapeToOccupied(int[,] shape, int x, int y, HashSet<(int, int)> occupied)
    {
        for (int i = 0; i < shape.GetLength(0); i++)
        {
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                if (shape[i, j] == 1)
                {
                    int newX = x + j;
                    ///int newY = y + i;
                    int newY = y + i; // ???
                    occupied.Add((newX, newY));
                }
            }
        }
    }

    //  >>><
    //  <><>
    //  ><<<>><>>><<<>>><<<><<<>><>><<>>
    // Funkcija za izpis stolpa
    static void PrintTower(HashSet<(int, int)> occupied, int width)
    {
        if (occupied.Count == 0)
        {
            Console.WriteLine("Stolp je prazen.");
            return;
        }

        // Najdi najvišjo Y vrednost (vrh stolpa)
        int maxHeight = occupied.Max(pos => pos.Item2);

        for (int y = maxHeight; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                if (occupied.Contains((x, y)))
                    Console.Write("#");
                else
                    Console.Write(".");
            }
            Console.WriteLine();
        }
    }

    static void PrintJetPattern(string input)
    {
        Console.WriteLine("Vzorec premikov (jet pattern):");
        Console.WriteLine(input);
        Console.WriteLine();
    }



    static long partTwoXX(string file, int towerWidth, long targetRocks)
    {
        Console.WriteLine("Part Two:");
        string instructions = File.ReadAllText($"../../../{file}");

        // PrintJetPattern(instructions);

        // zasedeni stolpci to ostane isto
        HashSet<(int, int)> occupied = new HashSet<(int, int)>();

        Dictionary<(int shapeIndex, int jetIndex), (long rocks, int height)> seenStates = new Dictionary<(int, int), (long, int)>();
        //Dictionary<string, (long rocks, int height)> seenStates = new Dictionary<string, (long, int)>();


        int currentHeight = 0;
        int currentShapeIndex = 0;

        int jetIndex = 0;
        char[] jetPattern = instructions.ToCharArray();

        //string executedInstructions = "";

        long rockCount = 0;

        while (rockCount < targetRocks)
        {
            char jet;

            var shape = shapes[currentShapeIndex];
            currentShapeIndex = (currentShapeIndex + 1) % shapes.Count;

            int x = 2; // Začetna X pozicija (dve enoti stran od levega roba)
            int y = currentHeight + 3; // Začetna Y pozicija (tri enote nad najvišjim delom stolpa)

            bool isFalling = true;
            //string executedShapeInstructions = "";

            while (isFalling)
            {
                // najdi ukaz levo ali desno
                jet = jetPattern[jetIndex];
                jetIndex = (jetIndex + 1) % jetPattern.Length;

                // executedShapeInstructions += jet;
                // executedInstructions += jet;

                // se premakne ali ne?
                if (jet == '<' && CanMove(shape, x - 1, y, occupied, towerWidth))
                    x--;
                else if (jet == '>' && CanMove(shape, x + 1, y, occupied, towerWidth))
                    x++;


                // Premik navzdol

                if (y > currentHeight || CanMove(shape, x, y - 1, occupied, towerWidth))
                    y--;
                else
                    isFalling = false;

            }

            // konec padanja, dodam v seznam
            // Dodajanje trenutne oblike v množico zasedenih mest
            AddShapeToOccupied(shape, x, y, occupied);

            // izpiše komande
            // PrintJetPattern(executedShapeInstructions);
            // Posodobi višino stolpa
            currentHeight = Math.Max(currentHeight, y + shape.GetLength(0));
            rockCount++;
            //PrintTower(occupied, towerWidth);


            // Preveri za cikel
            var stateKey = (currentShapeIndex, jetIndex);

            if (seenStates.ContainsKey(stateKey))
            {

                // shranim cikel
                var (previousRocks, previousHeight) = seenStates[stateKey];
                long cycleLength = rockCount - previousRocks;
                int heightGainPerCycle = currentHeight - previousHeight;



                long cyclesRemaining = (targetRocks - rockCount) / cycleLength;
                // long totalHeight = currentHeight + cyclesRemaining * heightGainPerCycle;
                // currentHeigh = totalHeight;
                long totalHeight = cyclesRemaining * heightGainPerCycle;
                long remainingRocks = (targetRocks - rockCount) % cycleLength;

                for (int i = 0; i < remainingRocks; i++)
                {

                    shape = shapes[currentShapeIndex];
                    currentShapeIndex = (currentShapeIndex + 1) % shapes.Count;

                    x = 2;
                    y = currentHeight + 3;
                    isFalling = true;

                    while (isFalling)
                    {
                        jet = jetPattern[jetIndex];
                        jetIndex = (jetIndex + 1) % jetPattern.Length;

                        if (jet == '<' && CanMove(shape, x - 1, y, occupied, towerWidth))
                            x--;
                        else if (jet == '>' && CanMove(shape, x + 1, y, occupied, towerWidth))
                            x++;

                        //     if (CanMove(shape, x, y - 1, occupied, towerWidth))
                        if (y > currentHeight || CanMove(shape, x, y - 1, occupied, towerWidth))
                            y--;
                        else
                            isFalling = false;
                    }

                    AddShapeToOccupied(shape, x, y, occupied);
                    currentHeight = Math.Max(currentHeight, y + shape.GetLength(0));

                }
                totalHeight += currentHeight;
                return totalHeight;

            }
            else
            {
                seenStates[stateKey] = (rockCount, currentHeight);
            }
        }
        return currentHeight;

        // izpiše komande
        //    PrintJetPattern(executedInstructions);
        //      PrintTower(occupied, towerWidth);

     }
    }
}