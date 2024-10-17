using System.Diagnostics;

namespace Day_22
{
    internal class Program
    {
        // Define the cube layout and transitions between faces here
        // This will depend on your specific cube layout.

        /*
         * 
         * Part One 
         * is a simple greed algorith that only desices if point cam nove or not etc. 
         * Simple
         * 
         * Part Two
         * I decided on a bit more universial solution.
         * 1.   there are 11 ways to fold net of the cube and the solution should cover all possible scenarios:
         *      https://www.wolframcloud.com/objects/demonstrations/All11FoldingNetsOfTheCube-source.nb
         * 2. First we must indentify numbered Faces of the cube
         * 3. We must find edges all of the Faces (but exclude inner edges)
         * 4. Find  Edges, that connect in corners
         * 
         * TODO:

         * 
         * 5. now start search for neigbouring Faces how cube map folds
         * - from each inner corner (from 4.) move out of the corrner on and the other dirrection and identify faces 
         * - direction from moving from start to end is a direction of a vector (start-end) 90° to face 
         *   is a direction that moving cursor over new face changes
         * - continue to next edge untill Face repeats (not valid edge)
         * - at the end we'll have all connections of the faces and how moving of cursor changes direction
         *-  go to next corner and repeat the proces
         * 
         * Now we have a rule of the game and how cursor moves along Faces
         * 
         * 6. we can implement changed algorithm from part one
         * 
         * 7. We imlement function of how cursor changes directions when moves from one face to another 
         * (new direction is stored in a map prom point 5)
         * 
         * 8. We read out new position and calculate result
         * */


        static void Main(string[] args)
        {

            Console.WriteLine("--- Day 22: Monkey Map ---");


            string puzzleA = "day22_inputdemo.txt";
            string puzzleB = "day22_input.txt";

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // solution is a kind of greedy algorithm
            int rezult = part1(puzzleB);
            stopwatch.Stop();
            TimeSpan elapsed = stopwatch.Elapsed;

            Console.WriteLine($"\nPart one!\nResult: {rezult} (6032 for demo and 197160 for puzzle) in {elapsed.Seconds} sec");
            Console.WriteLine($"\nPart two!");
            Day22part2 d2 = new Day22part2(puzzleB);
            rezult = d2.FinalPassword;
            stopwatch.Stop();
            elapsed = stopwatch.Elapsed;
            Console.WriteLine($"Result: {rezult} (145065) in {elapsed.Seconds} sec");
            stopwatch.Restart();
        }




        static int part1(string puzzleFile) {

            int  password = 0;

            var input = File.ReadAllText($"../../../{puzzleFile}")
                .Replace("\r\n", "\n")// Normalize line breaks \r\n to \n
                .Split("\n\n");

            var mapLines = input[0].Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            var path = input[1].Trim();


            if (input.Length != 2)
            {
                Console.WriteLine("Napaka: Vhodni podatki niso pravilno oblikovani.");
                return 0;
            }

            // Initialize the map as a 2D array based on input dimensions
            int rows = mapLines.Length;
            int cols = mapLines.Max(line => line.Length);
            char[,] map = new char[rows, cols];

            // Fill the map array with the input data (adding spaces for empty areas)
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    map[r, c] = c < mapLines[r].Length ? mapLines[r][c] : ' ';
                }
            }

            // Movement directions: right, down, left, up
            // dr and dc are arrays that represent changes in row and column positions for each direction
            // 0 = right, 1 = down, 2 = left, 3 = up
            int[] dr = { 0, 1, 0, -1 };  // Row deltas for the four directions
            int[] dc = { 1, 0, -1, 0 };  // Column deltas for the four directions


            // Starting position: find the first open tile (.) in the first row
            int row = 0;
            int col = Array.IndexOf(mapLines[0].ToCharArray(), '.');
            int direction = 0;  // 0 = desno, 1 = down, 2 = left, 3 = up

            // Helper function to check if a tile is open and walkable
            // bool IsOpenTile(int r, int c)
            // {
            //    return r >= 0 && r < rows && c >= 0 && c < cols && map[r, c] == '.';
            // }



            // Variable to keep track of the current number of steps to move (handle multi-digit numbers)
            int currentNum = 0;

            // Loop over each character in the path (instructions for movement)
            for (int i = 0; i < path.Length; i++)
            {
                char command = path[i];

                // If the character is a digit, accumulate it to form the number of steps
                if (char.IsDigit(command))
                {
                    currentNum = currentNum * 10 + (command - '0');  // 
                }
                else
                {
                    // Once we encounter a letter (L or R), move the required number of steps
                    Move(ref row, ref col, direction, currentNum, dr, dc, map, rows, cols);
                    currentNum = 0;  // Ponastavimo število premikov

                    // Update the direction based on the command (L = turn left, R = turn right)
                    if (command == 'L')
                    {
                        direction = (direction + 3) % 4;  // Turn left (counter-clockwise)
                    }
                    else if (command == 'R')
                    {
                        direction = (direction + 1) % 4;  // Turn right (clockwise)
                    }
                }
            }

            // Perform the last move if there's a remaining number after the loop
            if (currentNum > 0)
            {
                Move(ref row, ref col, direction, currentNum, dr, dc, map, rows, cols);
            }

            // Compute the final password based on the final position and direction
            password = 1000 * (row + 1) + 4 * (col + 1) + direction;

            Console.WriteLine($"Solution od Part one\nrow: {row}; col: {col}; direction: {direction}");
          

            return password;
        }



        // Function to handle moving across the map with wrapping around edges
        static void Move(ref int row, ref int col, int direction, int steps, int[] dr, int[] dc, char[,] map, int rows, int cols)
        {
            for (int i = 0; i < steps; i++)
            {
                // Calculate the next row and column based on the direction
                int newRow = row + dr[direction];
                int newCol = col + dc[direction];

                // If the move goes out of bounds or into an empty space, wrap around
                if (newRow < 0 || newRow >= rows || newCol < 0 || newCol >= cols || map[newRow, newCol] == ' ')
                {
                    (newRow, newCol) = WrapAround(row, col, direction, map, rows, cols);
                }

                // If we encounter a wall ('#'), stop moving
                if (map[newRow, newCol] == '#')
                {
                    break;
                }

                // Update the current position to the new row and column
                row = newRow;
                col = newCol;
            }
        }



        // Function to detect if we're leaving the current cube face (adjust as needed)
        static bool IsLeavingFace(int row, int col, int currentFace, int rows, int cols)
        {
            // Define face boundaries and return true if we're leaving the face
            return row < 0 || row >= rows || col < 0 || col >= cols;
        }

 


        // Function to handle wrapping around the map when moving off the edge
        static (int, int) WrapAround(int row, int col, int direction, char[,] map, int rows, int cols)
        {
            // If moving right, wrap to the beginning of the row
            if (direction == 0)  // right
            {
                for (int c = 0; c < cols; c++)
                {
                    if (map[row, c] != ' ')  // Find the first non-empty tile in the row
                        return (row, c);
                }
            }
            // If moving down, wrap to the top of the column
            else if (direction == 1)  // down - dol
            {
                for (int r = 0; r < rows; r++)
                {
                    if (map[r, col] != ' ') // Find the first non-empty tile in the column
                        return (r, col);
                }
            }
            // If moving left, wrap to the end of the row
            else if (direction == 2)  // left
            {
                for (int c = cols - 1; c >= 0; c--)
                {
                    if (map[row, c] != ' ')
                        return (row, c);
                }
            }
            // If moving up, wrap to the bottom of the column
            else if (direction == 3)  // up
            {
                for (int r = rows - 1; r >= 0; r--)
                {
                    if (map[r, col] != ' ')
                        return (r, col);
                }
            }
            // Return the original position if no wrapping is needed (fallback)
            return (row, col);  
        }
    }
}