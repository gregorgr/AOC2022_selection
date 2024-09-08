namespace Day_10
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AOC 2022 Day 10: Cathode-Ray Tube");

            try
            {

                // Vhodni podatki

                // demo data
                // string file = "input-d10-demo.txt";

                string file = "input-d10a.txt";
                // right answer is  6087
                partOne(file);




            }
            catch (Exception e)
            {

                throw;
            }
        }



        static void partOne(string file)
        {
            Console.WriteLine("Part One:");
            string[] instructions = File.ReadAllLines($"../../../{file}");

            // Part 1 - Simulate CPU Instructions
            int cycle = 0;
            int registerX = 1;  // Initial value of register X
            List<int> signalStrengths = new List<int>();

            // Part 2: Print the screen = FBURHZCH
            int screenWidth = 40;
            int screenHeight = 6;
            char[,] screen = new char[screenHeight, screenWidth];

            foreach (string instruction in instructions)
            {
                if (instruction.StartsWith("noop"))
                {
                    // No operation: Increase cycle by 1
                    DrawPixel(cycle, registerX, screen, screenWidth);
                    cycle++;
                    SignalStrength(cycle, registerX, signalStrengths);

                }
                else if (instruction.StartsWith("addx"))
                {
                    // Add value to register X after two cycles
                    int value = int.Parse(instruction.Split(' ')[1]);

                    // First cycle
                    DrawPixel(cycle, registerX, screen, screenWidth);
                    cycle++;
                    SignalStrength(cycle, registerX, signalStrengths);


                    // Second cycle
                    DrawPixel(cycle, registerX, screen, screenWidth);
                    cycle++;
                    SignalStrength(cycle, registerX, signalStrengths);


                    registerX += value;
                }
            }

            // Part 1: Output the sum of the signal strengths
            int resultPart1 = signalStrengths.Sum();
            Console.WriteLine($"Part 1: Sum of the signal strengths is {resultPart1}");

            Console.WriteLine("\nPart Two:");

            // Part 2: Print the screen = FBURHZCH
            Console.WriteLine("Part 2: CRT Screen Output:");
            for (int y = 0; y < screenHeight; y++)
            {
                for (int x = 0; x < screenWidth; x++)
                {
                    Console.Write(screen[y, x]);
                }
                Console.WriteLine();
            }

        }

        static void SignalStrength(int cycle, int x, List<int> signalStrengths)
        {
            // Capture signal strengths at specific cycles (20th, 60th, 100th, etc.)
            if ((cycle - 20) % 40 == 0)
            {
                int signalStrength = cycle * x;
                signalStrengths.Add(signalStrength);
            }
        }


        static void DrawPixel(int cycle, int registerX, char[,] screen, int screenWidth)
        {
            // Calculate the current row and column for the screen display
            int row = cycle / screenWidth;
            int col = cycle % screenWidth;

            // Draw '#' if within 1 pixel of registerX, otherwise draw '.'
            if (Math.Abs(registerX - col) <= 1)
            {
                screen[row, col] = '#';
            }
            else
            {
                screen[row, col] = '.';
            }
        }
    }
}