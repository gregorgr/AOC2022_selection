namespace Day_17
{
    internal class Program
    {

        // Definicija padanja kamnov in oblik skal
        static readonly List<int[,]> rocks = new List<int[,]>
        {
            new int[,] { { 1, 1, 1, 1 } },  // Horizontalna vrstica
            new int[,] { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } },  // + Plus
            new int[,] { { 1, 1, 1 }, { 0, 0, 1 }, { 0, 0, 1 } },  // obrnjen L
           // new int[,] {  { 0, 0, 1 }, { 0, 0, 1 }, { 1, 1, 1 } },  // obrnjen L
            new int[,] { { 1 }, { 1 }, { 1 }, { 1 } },  // Vertikalna vrstica I
            new int[,] { { 1, 1 }, { 1, 1 } }  // Kvadrat
        };

        static void Main(string[] args)
        {
            // --- Day 17: Pyroclastic Flow ---

            Console.WriteLine("AOC 2022 Day 17: Pyroclastic Flow aka Tetris---");

            // C: \Users\grego\source\repos\Algoritmi_AOC2022_selection\Algoritmi_AOC2022_selection.sln
            string file = "input-d17a.txt";
            // string file = "input-d17-demo.txt";
            // right answer is  3171

            int towerWidth = 7;  // Širina stolpa
            int numberOfRocks = 2022; // 2022;//  10; // 2022;  // Število kamnov, ki padejo
            long numberOfRocksB = 1000000000000; //  1.000.000.000.000; 2022;//  10; // 2022;  // Število kamnov, ki padejo

            partOne(file, towerWidth, numberOfRocks);
            /*
            total_rocks: 2572
            state: (1, 2, 1, 0, 1, 1, 3, 2, 5063)
            height_gain_in_cycle: 2729
            rocks_in_cycle: 1720
            skipped_cycles: 581395347
            total_rocks: 999999999413
            remaining_cicles: 588
            1586627906921
             * */



            long height = partTwo(file, towerWidth, numberOfRocksB);
            long correctResult = 1586627906921;

            // pravilni je 1586627906921
            // 1552769679301
            // total hight 1552769679301
            Console.WriteLine($"Višina stolpa po {numberOfRocksB} skalah je: {height}");

            if (file == "input-d17-demo.txt")
            {
                Console.WriteLine($"Demo 1514285714288");
            }
            else
            {
                if (correctResult - height != 0)
                {
                    Console.WriteLine($"Result off for {correctResult - height}");
                    Console.WriteLine("1586627906921 OK");
                    Console.WriteLine($"{height} yours");

                }
                else
                {
                    Console.WriteLine("BRAVO!!!");
                }

            }


        }


        static int moveLeftOrRight(char jet, int[,] currentRock, int x, int y, HashSet<(int, int)> occupied, int maxWidth)
        {
            int move = 0;
            if (jet == '<' && CanMove(currentRock, x - 1, y, occupied, maxWidth))
            {
                move--;
            }
            else if (jet == '>' && CanMove(currentRock, x + 1, y, occupied, maxWidth))
            {
                move++;
            }
            return move;
        }




        static long partTwo(string file, int towerWidth, long targetRocks)
        {
            Console.WriteLine("Part Two:");
            //string instructions = File.ReadAllText($"../../../{file}");
            string jets = File.ReadAllText($"../../../{file}").Trim();
            char jet;
            int jetIndex = -1;
            char[] jetPattern = jets.ToCharArray();


            HashSet<(int, int)> occupied = new HashSet<(int, int)>();
            
            var seenStates = new Dictionary<(string topography, int rockIndex, int jetIndex), (long rocks, int height)>();
            

            long skippedCiclesHeight = 0;
            int currentHeight = 0;
            bool patternDetected = false;

            int currentRockIndex = 0;



            long rockCount = 0;

            while (rockCount < targetRocks)
            {
                var currentRock = rocks[currentRockIndex];
                currentRockIndex = (currentRockIndex + 1) % rocks.Count;

                int x = 2; // Začetna X pozicija (dve enoti stran od levega roba)
                int y = currentHeight + 3; // Začetna Y pozicija (tri enote nad najvišjim delom stolpa)

                bool isFalling = true;

                while (isFalling)
                {

                    jetIndex = (jetIndex + 1) % jetPattern.Length;
                    jet = jetPattern[jetIndex];

                    // se premakne levo desno ali ne?
                    x += moveLeftOrRight(jet, currentRock, x, y, occupied, towerWidth);

                    // Premik navzdol
                    if (y > currentHeight || CanMove(currentRock, x, y - 1, occupied, towerWidth))
                    {
                        y--;
                    }
                    else
                    {
                        isFalling = false;
                    }
                }
                // Dodajanje trenutne oblike v množico zasedenih mest
                AddCurrentRockToOccupied(currentRock, x, y, occupied);
                currentHeight = Math.Max(currentHeight, y + currentRock.GetLength(0));

                // moram ugotoviti ponavljajoče vzorce in s tem najdem cikle
                ////// var stateKey = (currentRockIndex, jetIndex);
                // Generate a state key for current state using a tuple
                string topography = GetTopographyKey(occupied, currentHeight, towerWidth);

                var stateKey = (topography, currentRockIndex, jetIndex);


                if (!patternDetected && seenStates.ContainsKey(stateKey))
                { 
                    var (previousRocks, previousHeight) = seenStates[stateKey];
                    int tempRock = (int)rockCount;

                    patternDetected = true;
               

                    if (patternDetected)
                    {
                        long cycleLength = rockCount - previousRocks;
                        int heightGainPerCycle = currentHeight - previousHeight;
                        long rocksInCycle = rockCount - previousRocks;
                        long cyclesRemaining = (targetRocks - rockCount) / cycleLength;
                        // long totalHeight = currentHeight + cyclesRemaining * heightGainPerCycle;
                        // currentHeigh = totalHeight;

                        skippedCiclesHeight = cyclesRemaining * heightGainPerCycle;
                        long remainingRocks = (targetRocks - rockCount) % cycleLength;
                        targetRocks = rockCount + remainingRocks;


                        Console.WriteLine($"Cycle Found at rockCount {rockCount}");
                        Console.WriteLine($" stateKey= {stateKey}");
                        Console.WriteLine($" cycleLength  {cycleLength}");
                        Console.WriteLine($" heightGainPerCycle  {heightGainPerCycle}");
                        Console.WriteLine($" rocksInCycle  {rocksInCycle}");

                        Console.WriteLine($" totalHeight  {skippedCiclesHeight}");
                        Console.WriteLine($" cyclesRemaining  {cyclesRemaining}");
                        Console.WriteLine($" remainingRocks   {remainingRocks}");
                    }
                }
                if (!patternDetected)
                {
                    seenStates[stateKey] = (rockCount, currentHeight);
                }



                rockCount++;
            }

            return skippedCiclesHeight + currentHeight;
        }




        static string GetTopographyKey(HashSet<(int, int)> occupied, int currentHeight, int width)
        {
            int depth = 20;  // A small depth to capture the topography
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
            int currentRockIndex = 0;

            int jetIndex = 0;
            char[] jetPattern = jets.ToCharArray();

            string executedInstructions = "";

            for (int i = 0; i < numberOfRocks; i++)
            {

                var currentRock = rocks[currentRockIndex];
                currentRockIndex = (currentRockIndex + 1) % rocks.Count;


                int x = 2; // Začetna X pozicija (dve enoti stran od levega roba)
                int y = currentHeight + 3; // Začetna Y pozicija (tri enote nad najvišjim delom stolpa)

                bool isFalling = true;
                string executedcurrentRockInstructions = "";

                while (isFalling)
                {
                    // najdi ukaz levo ali desno
                    char jet = jetPattern[jetIndex];
                    jetIndex = (jetIndex + 1) % jetPattern.Length;

                    executedcurrentRockInstructions += jet;
                    executedInstructions += jet;

                    // se premakne ali ne?
                    if (jet == '<' && CanMove(currentRock, x - 1, y, occupied, towerWidth))
                        x--;
                    else if (jet == '>' && CanMove(currentRock, x + 1, y, occupied, towerWidth))
                        x++;

                    // Premik navzdol

                    if (y > currentHeight || CanMove(currentRock, x, y - 1, occupied, towerWidth))
                        y--;
                    else
                        isFalling = false;

                }

                // konec padanja, dodam v seznam
                // Dodajanje trenutne oblike v množico zasedenih mest
                AddCurrentRockToOccupied(currentRock, x, y, occupied);

                // izpiše komande
                // PrintJetPattern(executedcurrentRockInstructions);
                // Posodobi višino stolpa
                currentHeight = Math.Max(currentHeight, y + currentRock.GetLength(0));
                //PrintTower(occupied, towerWidth);

            }

            // izpiše komande
            //    PrintJetPattern(executedInstructions);

            Console.WriteLine($"Višina stolpa po {numberOfRocks} skalah je: {currentHeight}");
            //      PrintTower(occupied, towerWidth);

        }


        static bool CanMove(int[,] currentRock, int x, int y, HashSet<(int, int)> occupied, int towerWidth)
        {
            // Preverimo vse dele oblike y = i
            for (int i = 0; i < currentRock.GetLength(0); i++)
            {
                // reverimo še za x = j
                for (int j = 0; j < currentRock.GetLength(1); j++)
                {
                    if (currentRock[i, j] == 1)
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


        static void AddCurrentRockToOccupied(int[,] currentRock, int x, int y, HashSet<(int, int)> occupied)
        {
            for (int i = 0; i < currentRock.GetLength(0); i++)
            {
                for (int j = 0; j < currentRock.GetLength(1); j++)
                {
                    if (currentRock[i, j] == 1)
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

            Dictionary<(int rockIndex, int jetIndex), (long rocks, int height)> seenStates = new Dictionary<(int, int), (long, int)>();
            //Dictionary<string, (long rocks, int height)> seenStates = new Dictionary<string, (long, int)>();


            int currentHeight = 0;
            int currentRockIndex = 0;

            int jetIndex = 0;
            char[] jetPattern = instructions.ToCharArray();

            //string executedInstructions = "";

            long rockCount = 0;

            while (rockCount < targetRocks)
            {
                char jet;

                var currentRock = rocks[currentRockIndex];
                currentRockIndex = (currentRockIndex + 1) % rocks.Count;

                int x = 2; // Začetna X pozicija (dve enoti stran od levega roba)
                int y = currentHeight + 3; // Začetna Y pozicija (tri enote nad najvišjim delom stolpa)

                bool isFalling = true;
                //string executedRockInstructions = "";

                while (isFalling)
                {
                    // najdi ukaz levo ali desno
                    jet = jetPattern[jetIndex];
                    jetIndex = (jetIndex + 1) % jetPattern.Length;

                    // executedcurrentRockInstructions += jet;
                    // executedInstructions += jet;

                    // se premakne ali ne?
                    if (jet == '<' && CanMove(currentRock, x - 1, y, occupied, towerWidth))
                        x--;
                    else if (jet == '>' && CanMove(currentRock, x + 1, y, occupied, towerWidth))
                        x++;


                    // Premik navzdol

                    if (y > currentHeight || CanMove(currentRock, x, y - 1, occupied, towerWidth))
                        y--;
                    else
                        isFalling = false;

                }

                // konec padanja, dodam v seznam
                // Dodajanje trenutne oblike v množico zasedenih mest
                AddCurrentRockToOccupied(currentRock, x, y, occupied);

                // izpiše komande
                // PrintJetPattern(executedcurrentRockInstructions);
                // Posodobi višino stolpa
                currentHeight = Math.Max(currentHeight, y + currentRock.GetLength(0));
                rockCount++;
                //PrintTower(occupied, towerWidth);


                // Preveri za cikel
                var stateKey = (currentRockIndex, jetIndex);

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

                        currentRock = rocks[currentRockIndex];
                        currentRockIndex = (currentRockIndex + 1) % rocks.Count;

                        x = 2;
                        y = currentHeight + 3;
                        isFalling = true;

                        while (isFalling)
                        {
                            jet = jetPattern[jetIndex];
                            jetIndex = (jetIndex + 1) % jetPattern.Length;

                            if (jet == '<' && CanMove(currentRock, x - 1, y, occupied, towerWidth))
                                x--;
                            else if (jet == '>' && CanMove(currentRock, x + 1, y, occupied, towerWidth))
                                x++;

                            //     if (CanMove(currentRock, x, y - 1, occupied, towerWidth))
                            if (y > currentHeight || CanMove(currentRock, x, y - 1, occupied, towerWidth))
                                y--;
                            else
                                isFalling = false;
                        }

                        AddCurrentRockToOccupied(currentRock, x, y, occupied);
                        currentHeight = Math.Max(currentHeight, y + currentRock.GetLength(0));

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