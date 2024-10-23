using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_22
{

    // Class representing a cube face
    public class CubeFace
    {
        public int Id { get; set; }  // Face ID for tracking
        public int TopLeftRow { get; set; }
        public int TopLeftCol { get; set; }
        public int Size { get; set; }

        public CubeFace(int id, int topLeftRow, int topLeftCol, int size)
        {
            Id = id;
            TopLeftRow = topLeftRow;
            TopLeftCol = topLeftCol;
            Size = size;
        }
    }

    public class EdgePair {

        public Edge EdgeA { get; set; }
        public Edge EdgeB { get; set; }

        public EdgePair(Edge eA, Edge eBd) {
            EdgeA = eA;
            EdgeB = eBd;
        }
    }
    public enum Direction
    {
        Right = 0,
        Down = 1,
        Left = 2,
        Up = 3
    }

    public class Edge
    {
        public (int Row, int Col) Start { get; set; } // direction of connection
        public (int Row, int Col) End { get; set; }
        public List<(int Row, int Col)> Coordinates { get; set; }
        public int FaceId { get; set; }
        public string EdgeType { get; set; }

        public bool IsVertical {  get; set; }

        public Edge(int faceId, string edgeType, (int Row, int Col) start, (int Row, int Col) end)
        {
            FaceId = faceId;
            EdgeType = edgeType;
            Start = start;
            End = end;
            IsVertical = start.Col == end.Col;
            Coordinates = new List<(int Row, int Col)>();
        }

        // Check if two edges are adjacent and aligned
        public bool IsAdjacentAndAligned(Edge other)
        {
            // Check for Different Faces
            if (this.FaceId==other.FaceId) { 
                return false; 
            }
      
            // Check if this edge is aligned and adjacent to the other edge (perpendicular)
            if (this.EdgeType == "right" && other.EdgeType == "left" && this.Start.Row == other.Start.Row && this.Start.Col + 1 == other.Start.Col)
            {
                return true;
            }
            if (this.EdgeType == "bottom" && other.EdgeType == "top" && this.Start.Row+ 1 == other.Start.Row && this.Start.Col == other.Start.Col)
            {
                return true;
            }
            return false;
        }

        public bool Equals(Edge other) {
            // Check for null and reference equality
            if (other == null) return false;

            // edge is the same
            if (this.FaceId == other.FaceId && this.IsVertical == other.IsVertical && this.EdgeType == other.EdgeType)
            {
                return true;
            }
            return false;
        }

        public void FlippPoints() {
            (int Row, int Col) oldStart = this.Start;
            this.Start = this.End;
            this.End = oldStart;
        }
        public bool IsConnectedTo(Edge other) {

            // edge is the same
            if(this.Equals( other)) {
                return false;
            }
            // edge is the same
            if (this.FaceId == other.FaceId && this.IsVertical == other.IsVertical && this.EdgeType == other.EdgeType)
            {
                return true;
            }
            if ((this.End == other.Start))
            {

                return true;
            }

            if ((this.End == other.End))
            {
                other.FlippPoints();
                return true;
            }
            if ((this.End.Row + 1 == other.Start.Row && this.End.Col == other.Start.Col) || (this.End.Row == other.Start.Row && this.End.Col+1 == other.Start.Col))
            {
                return true;
            }
            if ((this.End.Row + 1 == other.End.Row && this.End.Col == other.End.Col) || (this.End.Row  == other.End.Row && this.End.Col+1 == other.End.Col))
            {
                other.FlippPoints();
                return true;
            }
            if ((this.End.Row-1 == other.End.Row && this.End.Col == other.End.Col))
            {
                other.FlippPoints();
                return true;
            }
            if ((this.End.Row  == other.End.Row-1 && this.End.Col == other.End.Col))
            {
                other.FlippPoints();
                return true;
            }

            return false;
        }
        public bool IsValidCorner(Edge other)
        {
            // Check for Different Faces
            // string s1 = this.EdgeType;
            // string s2 = other.EdgeType;
            if (this.FaceId == other.FaceId || this.IsVertical == other.IsVertical)
            {
                return false;
            }

            //if(!this.IsVertical && this.Start.Row==0 ) {    return false;     }

            if (!this.IsVertical)
            {
                if (this.EdgeType == "top")
                {

                    // TODO:


                }
                else
                {
                    // bottom
                    if ((this.Start.Col - 1 == other.Start.Col && this.Start.Row + 1 == other.Start.Row)) {

                        return true;
                    }else if (this.End.Col - 1 == other.Start.Col && this.End.Row + 1 == other.Start.Row) {

                        // flip start of this
                        this.FlippPoints();
                        return true;
                    }
                    else if (this.Start.Col - 1 == other.End.Col && this.Start.Row + 1 == other.End.Row) {

                        // flip start of other
                        other.FlippPoints();
                        return true;
                    }
                    else if (this.End.Col - 1 == other.End.Col && this.End.Row + 1 == other.End.Row)
                    {
                        // flip start of this
                        this.FlippPoints();
                        // flip start of other
                        other.FlippPoints();
                        return true;
                    }

                }
            }
            else {
                // horizontal
                if (this.EdgeType == "left")
                {
                    if ((this.Start.Col - 1 == other.Start.Col && this.Start.Row + 1 == other.Start.Row))
                    {

                        return true;
                    }
                    else if (this.End.Col - 1 == other.Start.Col && this.End.Row + 1 == other.Start.Row)
                    {

                        // flip start of this
                        this.FlippPoints();
                        return true;
                    }
                    else if (this.Start.Col - 1 == other.End.Col && this.Start.Row + 1 == other.End.Row)
                    {

                        // flip start of other
                        other.FlippPoints();
                        return true;
                    }
                    else if (this.End.Col - 1 == other.End.Col && this.End.Row + 1 == other.End.Row)
                    {
                        // flip start of this
                        this.FlippPoints();
                        // flip start of other
                        other.FlippPoints();
                        return true;
                    }
                }
                else
                {
                    // bottom
                    // TODO: 
                    // check if code behaves OK
                    if ((this.Start.Col - 1 == other.Start.Col && this.Start.Row + 1 == other.Start.Row))
                    {

                        return true;
                    }
                    else if (this.End.Col - 1 == other.Start.Col && this.End.Row + 1 == other.Start.Row)
                    {

                        // flip start of this
                        this.FlippPoints();
                        return true;
                    }
                    else if (this.Start.Col - 1 == other.End.Col && this.Start.Row + 1 == other.End.Row)
                    {

                        // flip start of other
                        other.FlippPoints();
                        return true;
                    }
                    else if (this.End.Col - 1 == other.End.Col && this.End.Row + 1 == other.End.Row)
                    {
                        // flip start of this
                        this.FlippPoints();
                        // flip start of other
                        other.FlippPoints();
                        return true;
                    }
                }
            }


            // Check if this edge is aligned and adjacent to the other edge (perpendicular)
            if (this.EdgeType == "right" && other.EdgeType == "left" && this.Start.Row == other.Start.Row && this.Start.Col + 1 == other.Start.Col)
            {
                return true;
            }
            if (this.EdgeType == "bottom" && other.EdgeType == "top" && this.Start.Row + 1 == other.Start.Row && this.Start.Col == other.Start.Col)
            {
                return true;
            }
            return false;
        }
        public Edge Clone()
        {
            // Create a new Edge object with the same properties
            var clonedEdge = new Edge(this.FaceId, this.EdgeType, this.Start, this.End);

            // Copy the coordinates list (deep copy)
            foreach (var coordinate in this.Coordinates)
            {
                clonedEdge.Coordinates.Add((coordinate.Row, coordinate.Col));
            }

            return clonedEdge;
        }
    }


    public class Day22part2
    {
        /*
         * 
        // we would use similar code if we would store mapping in to cubeTransitions
        static Dictionary<(int, int), (int, int)> cubeTransitions = new Dictionary<(int, int), (int, int)>()
        {
            // Assuming a cube with 6 faces, indexed 1-6, with transitions between faces

            // Transitions for face 1
            { (1, 0), (2, 0) }, // Moving right from face 1 leads to face 2, same direction
            { (1, 1), (4, 1) }, // Moving down from face 1 leads to face 4, same direction
            { (1, 2), (3, 2) }, // Moving left from face 1 leads to face 3, same direction
            { (1, 3), (6, 1) }, // Moving up from face 1 wraps to face 6, changes to down

            // Transitions for face 2
            { (2, 0), (5, 2) }, // Moving right from face 2 wraps to face 5, changes to left
            { (2, 1), (4, 2) }, // Moving down from face 2 wraps to face 4, changes to left
            { (2, 2), (1, 2) }, // Moving left from face 2 leads back to face 1, same direction
            { (2, 3), (6, 0) }, // Moving up from face 2 wraps to face 6, changes to right

            // Define similar transitions for faces 3, 4, 5, 6
        };
        */
        // Dictionary to store cube transitions (current position and direction -> new position and direction)
        // public Dictionary<(int Row, int Col, string Direction), (int newRow, int newCol, string newDirection)> cubeTransitions;
        // Dictionary to store cube transitions (current last position -> new position and direction)
        public Dictionary<(int Row, int Col), (int newRow, int newCol, Direction newDir)> cubeTransitions;


        public int FinalPassword { get; set; }

        private string[] mapLines;

        private List<CubeFace> Faces;

        // Directions as vectors for movement
        private (int Row, int Col)[] DirectionVectors = new (int Row, int Col)[]
        {
            (0, 1),   // Right
            (1, 0),   // Down
            (0, -1),  // Left
            (-1, 0)   // Up
        };

        public Day22part2(string puzzleFile) {


            var input = File.ReadAllText($"../../../{puzzleFile}")
                .Replace("\r\n", "\n")// Normalize line breaks \r\n to \n
                .Split("\n\n");

            var _mapLines = input[0].Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            mapLines = _mapLines;

            var pathInstructions = input[1].Trim();

            // Determine the size of the map
            int rows = mapLines.Length;
            int cols = mapLines.Max(line => line.Length);

            // Convert the map into a 2D array
            char[,] map = GetMap(rows, cols); // new char[rows, cols];

            try
            {

                // Find the size of one face dynamically
                int faceSize = FindFaceSize(map, rows, cols);
                Console.WriteLine($"Detected face size: {faceSize}");


                // Find all faces on the map
                Faces = FindAllFaces(map, faceSize, rows, cols);
                // Output all faces with their coordinates and sizes
                foreach (var face in Faces)
                {
                    Console.WriteLine($"Face  {face.Id}  at ({face.TopLeftRow}, {face.TopLeftCol}) with size {face.Size}");

                }

                
                // Identify the outer edges for each face
                // var edges = GetAllOuterEdges(faces, map);
                int i = 1;
                var edges = GetAllOuterEdges(Faces);
                Console.WriteLine("OuterEdges:");
                foreach (var edge in edges)
                {
                    Console.WriteLine($"{i} Edge of Face {edge.FaceId} ({edge.EdgeType}) starts at {edge.Start} and ends at {edge.End}");
                    i++;
                }


                Console.WriteLine("");
                // Find corners where edges intersect

                var corners = FindInnerCorners(edges);

                //var corners = FindCorners(edges);

                foreach (EdgePair pair in corners)
                {
                    Console.WriteLine($"Corner between Face {pair.EdgeA.FaceId} {pair.EdgeA.EdgeType} and position {pair.EdgeB.FaceId} {pair.EdgeB.EdgeType}");
                }
                Console.WriteLine("");

                List<EdgePair> edgePairs = FindOtherPairs(corners, edges);
                foreach (EdgePair pair in edgePairs)
                {
                    Console.WriteLine($"face {pair.EdgeA.FaceId}  {pair.EdgeA.EdgeType} --> {pair.EdgeB.FaceId} {pair.EdgeB.EdgeType} ");
                    Console.WriteLine($"{pair.EdgeA.FaceId}={pair.EdgeA.Start}:{pair.EdgeA.End} --> {pair.EdgeB.FaceId}={pair.EdgeB.Start}:{pair.EdgeB.End}\n");
                }
                Console.WriteLine("");

                // create a cube transitions map
                // cubeTransitions = new Dictionary<(int Row, int Col, Direction Dir), (int newRow, int newCol, Direction newDir)>();
                cubeTransitions = new Dictionary<(int Row, int Col), (int newRow, int newCol, Direction newDir)>();

                //var solver =   
                GenerateCubeTransitions(edgePairs);
                // Output the transitions
                foreach (var transition in cubeTransitions)
                {
                 //   Console.WriteLine($"({transition.Key.Row}, {transition.Key.Col}) -> " +
                 //                     $"({transition.Value.newRow}, {transition.Value.newCol}, {transition.Value.newDir})");
                }
                Console.WriteLine("");

                // Now execute the path instructions:
                ExecutePathInstructions(pathInstructions);
  

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message.ToString());
            }



            }

        private void ExecutePathInstructions(string pathInstructions)
        {
            // Start position and direction
            int row = 0;
            int col = Array.IndexOf(mapLines[0].ToCharArray(), '.'); // Starting at the leftmost open tile
            Direction currentDirection = Direction.Right;

            // Process the path instructions
            for (int i = 0; i < pathInstructions.Length;)
            {
                if (char.IsDigit(pathInstructions[i]))
                {
                    // Parse the number of steps
                    int steps = 0;
                    while (i < pathInstructions.Length && char.IsDigit(pathInstructions[i]))
                    {
                        steps = steps * 10 + (pathInstructions[i] - '0');
                        i++;
                    }

                    // Move forward by the parsed number of steps
                    MoveForward(ref row, ref col, ref currentDirection, steps);
                }
                else if (pathInstructions[i] == 'R' || pathInstructions[i] == 'L')
                {
                    // Rotate direction
                    currentDirection = Turn(currentDirection, pathInstructions[i]);
                    i++; // Move past the 'R' or 'L'
                }
            }

            // Output the final result
            Console.WriteLine($"Final position: ({row + 1}, {col + 1}), Facing: {currentDirection}");
            FinalPassword = (1000 * (row + 1)) + (4 * (col + 1)) + (int)currentDirection;
           // Console.WriteLine($"Final password: {finalPassword}");
        }


        private void MoveForward(ref int row, ref int col, ref Direction direction, int steps)
        {
            Direction newDirection = direction;

            for (int step = 0; step < steps; step++)
            {
                // Calculate the next row and column based on current direction
                int nextRow = row + DirectionVectors[(int)direction].Row;
                int nextCol = col + DirectionVectors[(int)direction].Col;

                // Check if the next position is outside the current face
                if (!IsInsideMap(nextRow, nextCol) || mapLines[nextRow][nextCol] == ' ')
                {
                    // Use the cube transition map to move to the next face
                    (nextRow, nextCol, newDirection) = cubeTransitions[(row, col)];
                }

                // If the new position is a wall, stop moving
                if (mapLines[nextRow][nextCol] == '#') break;

                if (newDirection!=direction) { direction = newDirection; }

                // Update the current position
                row = nextRow;
                col = nextCol;
            }


        }


        private Direction Turn(Direction currentDirection, char turn)
        {
            // Rotate 90 degrees: 'R' = clockwise, 'L' = counterclockwise
            if (turn == 'R')
                return (Direction)(((int)currentDirection + 1) % 4);
            else
                return (Direction)(((int)currentDirection + 3) % 4); // Equivalent to turning left
        }

        private bool IsInsideMap(int row, int col)
        {
            return row >= 0 && row < mapLines.Length && col >= 0 && col < mapLines[row].Length;
        }


        // Method to generate transitions from edge pairs
        public void GenerateCubeTransitions(List<EdgePair> edgePairs)
        {
            foreach (var pair in edgePairs)
            {
                Edge edgeA = pair.EdgeA;
                Edge edgeB = pair.EdgeB;

                // Both edges should have the same number of coordinates
                int edgeLength = edgeA.Coordinates.Count;

                // Get new direction fron mode type for  edge B to edge A
                Direction directionA = GetDirectionFromEdgeType(edgeA);
                Direction directionB = GetDirectionFromEdgeType(edgeB);

                // the edge of square face edge is a vector pointing in one direction 
                // if start point of vector correspondes to edge coordinates  array edgeA.Coordinates[0]
                //      then we start at index 0 and increase index ++
                //      else we start at inde= edgeLength - 1 (49) and decreasse index --
                int indexA = edgeA.Start == edgeA.Coordinates[0] ? 0 : edgeLength - 1;
                int indexB = edgeB.Start == edgeB.Coordinates[0] ? 0:  edgeLength - 1;

                bool isForwardArrayA = indexA == 0;
                bool isForwardArrayB = indexB == 0;

                // Loop until we've processed all elements in the arrays
                int steps = 0; // Keep track of steps (to compare with edgeLength)
                while (steps < edgeLength)
                {
                    var coordA = edgeA.Coordinates[indexA];
                    var coordB = edgeB.Coordinates[indexB];  // vector direction 


                    // Add transition from edge A to edge B
                    cubeTransitions[(coordA.Row, coordA.Col)] = (coordB.Row, coordB.Col, directionB);

                    cubeTransitions[(coordB.Row, coordB.Col)] = (coordA.Row, coordA.Col, directionA);

                    // Move isForwardArrayA based on direction of an edge vector
                    if (isForwardArrayA)
                        indexA++;  // Move forward in edge A
                    else
                        indexA--;  // Move backward in edge A

                    // Move isForwardArrayB based on direction of an edge vector
                    if (isForwardArrayB)
                        indexB++;  // Move backward
                    else
                        indexB--;  // Move forward

                    // Increment the step counter
                    steps++;

                }
     
            }
        }

        
       
        // Helper method to get the direction based on the edge type
        private Direction GetDirectionFromEdgeType(Edge edge)
        {

            switch (edge.EdgeType) {

                case "right":
                    return Direction.Left;
                case "left": 
                    return Direction.Right;
                case "top":
                    return Direction.Down;
                case "bottom":
                    return Direction.Up;
                default:
                    throw new Exception($"Unknown direction type: {edge.EdgeType}");

            }
        }

        public static List<EdgePair> FindOtherPairs(List<EdgePair> pairs, List<Edge> edges)
        {
            // clone list of edges
            List<Edge> edgeClones = new List<Edge>();
            foreach (Edge edge in edges)
            {
                Edge clonedEdge = edge.Clone(); // Use the Clone method to create a deep copy
                edgeClones.Add(clonedEdge);     // Add the cloned edge to the edgeClones list
            }

            // remove paired edges 
            foreach (EdgePair pair in pairs) {

                
                for (int i = edgeClones.Count - 1; i >= 0; i--) { 
                    if (pair.EdgeA.Equals(edgeClones [i]) || pair.EdgeB.Equals(edgeClones[i])) {

                        // remove  (edges[i] from list
                        edgeClones.RemoveAt(i);
                    }
                }
            }


            List<EdgePair> newPairs = new List<EdgePair>();
            foreach (EdgePair pair in pairs)
            {
                newPairs = findNextPair(newPairs, pair, edgeClones);
                newPairs.Add(pair);

            }

            // check if some edges are not paired
            if (edgeClones.Count == 2)
            {
                
                EdgePair newpair = new EdgePair(edgeClones[1], edgeClones[0]);
                newPairs.Add(newpair);
            }
            else if (edgeClones.Count > 0) {
                // TODO:
                throw new Exception("Not implemented procedure in function FindOtherPairs");
            }


            return newPairs;
        }

        public static List<EdgePair> findNextPair(List<EdgePair> newPairs , EdgePair pair, List<Edge> edges) {

            Edge e1 = null;
            int i1 = -1;
            Edge e2 = null;
            int i2 = -1;
            /// find first edge
            for (int i = edges.Count - 1; i >= 0; i--)
            {
                if (pair.EdgeA.IsConnectedTo(edges[i]))
                {
                    e1 = edges[i].Clone();
                    i1 = i;
                    edges.RemoveAt(i);
                    break;
                }
            }

            for (int i = edges.Count - 1; i >= 0; i--)
            {
                if (pair.EdgeB.IsConnectedTo(edges[i]))
                {
                    i2 = i;
                    e2 = edges[i].Clone();
                    edges.RemoveAt(i);
                    break;
                }
            }

            if (i1 >= 0 && i2 >= 0 && (pair.EdgeA.FaceId != e1.FaceId || pair.EdgeB.FaceId != e2.FaceId))
            {
                EdgePair newpair = new EdgePair(e1, e2);

                newPairs.Add(newpair);

                //if(edges.count)
                newPairs = findNextPair(newPairs, newpair, edges);
            }
            else if (i1 >= 0 || i2 >= 0) {
                // it was not valid add
                if (i1 >= 0) edges.Add(e1);
                if (i2 >= 0) edges.Add(e2);
                return newPairs;
            }

            return newPairs;
        }

        public static List<EdgePair> FindInnerCorners(List<Edge> edges) {

            // PointPair
            List<EdgePair> pairs = new List<EdgePair>();
            for (int i = 0; i < edges.Count; i++)
            {
                bool isCorner = false;

                for (int j = i + 1; j < edges.Count; j++)
                {
                    if (edges[i].IsValidCorner(edges[j]))
                    {
                        // Here, you can either merge them or skip them (depending on your goal)
                        // edges.RemoveAt(j);
                        isCorner = true;
                        pairs.Add(new EdgePair(edges[i].Clone(), edges[j].Clone()));
                        break; // Skip this edge, it's been merged
                    }

                }
            }

             return pairs;
        
        }

   
        // Function to find the size of one cube face dynamically

        public static int FindFaceSize(char[,] map, int rows, int cols)
        {
            // Start scanning the map to find the first non-empty tile
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (map[r, c] != ' ')  // Found a face starting point
                    {
                        // Find the width of the first face (stop when we hit an empty space)
                        int width = GetFaceWidth(map, r, c, cols);

                        // Find the height of the first face (ensure the width stays constant)
                        int height = GetFaceHeight(map, r, c, width, rows);

                        // Ensure width equals height (as all cube faces are square)
                        if (width == height)
                        {
                            return width;  // Width and height are equal, return face size
                        }
                    }
                }
            }
            throw new Exception("No face found on the map.");
        }

        // Get the width of the face starting at the given coordinates
        public static int GetFaceWidth(char[,] map, int startRow, int startCol, int cols)
        {
            int width = 0;
            while (startCol + width < cols && map[startRow, startCol + width] != ' ')
            {
                width++;
            }
            return width;
        }

        // Get the height of the face starting at the given coordinates
        public static int GetFaceHeight(char[,] map, int startRow, int startCol, int width, int rows)
        {
            int height = 0;
            while (startRow + height < rows)
            {
                // Check that the width remains constant across all rows of the face
                for (int c = 0; c < width; c++)
                {
                    if (map[startRow + height, startCol + c] == ' ')
                    {
                        return height;  // We hit an empty space, end the face
                    }
                }
                height++;
            }
            return height;
        }


        // Function to find all cube faces on the map
        public static List<CubeFace> FindAllFaces(char[,] map, int faceSize, int rows, int cols)
        {
            var faces = new List<CubeFace>();
            bool[,] visited = new bool[rows, cols];  // Keep track of visited tiles
            int faceId = 1;  // Assign an ID to each face
            // Iterate through the map and detect cube faces
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    // Skip empty spaces and already visited tiles
                    if (map[r, c] != ' ' && !visited[r, c] && IsFaceStart(map, r, c, faceSize, rows, cols))
                    {
                        // Add the face with the known size and unique ID
                        faces.Add(new CubeFace(faceId++, r, c, faceSize));

                        // Mark all tiles of the current face as visited
                        MarkFaceAsVisited(visited, r, c, faceSize);

                        // Skip past the current face (move horizontally)
                        c += faceSize - 1;
                    }
                }
            }

            // Ensure exactly 6 faces are detected
            if (faces.Count != 6)
            {
                throw new Exception($"The map does not contain exactly 6 faces (found {faces.Count}).");
            }
            return faces;
        }

        // Mark all tiles of the current face as visited
        public static void MarkFaceAsVisited(bool[,] visited, int startRow, int startCol, int faceSize)
        {
            for (int r = startRow; r < startRow + faceSize; r++)
            {
                for (int c = startCol; c < startCol + faceSize; c++)
                {
                    visited[r, c] = true;
                }
            }
        }

        // Check if a face starts at the given coordinates
        public static bool IsFaceStart(char[,] map, int startRow, int startCol, int faceSize, int rows, int cols)
        {
            // Ensure the face is fully within bounds and non-empty
            for (int r = startRow; r < startRow + faceSize; r++)
            {
                for (int c = startCol; c < startCol + faceSize; c++)
                {
                    if (r >= rows || c >= cols || map[r, c] == ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        // Function to get all outer edges for each face (unchanged from previous implementation)
        // Get all outer edges for each face, with adjacent edges checked and cleaned
        public static List<Edge> GetAllOuterEdges(List<CubeFace> faces)
        {
            var edges = new List<Edge>();

            // Build the edges for each face
            foreach (var face in faces)
            {
                // Top edge
                var topEdge = new Edge(
                    face.Id,
                    "top",
                    (face.TopLeftRow, face.TopLeftCol),
                    (face.TopLeftRow, face.TopLeftCol + face.Size - 1)
                );
                for (int col = face.TopLeftCol; col < face.TopLeftCol + face.Size; col++)
                {
                    topEdge.Coordinates.Add((face.TopLeftRow, col));
                }
                edges.Add(topEdge);

                // Bottom edge
                var bottomEdge = new Edge(
                    face.Id,
                    "bottom",
                    (face.TopLeftRow + face.Size - 1, face.TopLeftCol),
                    (face.TopLeftRow + face.Size - 1, face.TopLeftCol + face.Size - 1)
                );
                for (int col = face.TopLeftCol; col < face.TopLeftCol + face.Size; col++)
                {
                    bottomEdge.Coordinates.Add((face.TopLeftRow + face.Size - 1, col));
                }
                edges.Add(bottomEdge);

                // Left edge
                var leftEdge = new Edge(
                    face.Id,
                    "left",
                    (face.TopLeftRow, face.TopLeftCol),
                    (face.TopLeftRow + face.Size - 1, face.TopLeftCol)
                );
                for (int row = face.TopLeftRow; row < face.TopLeftRow + face.Size; row++)
                {
                    leftEdge.Coordinates.Add((row, face.TopLeftCol));
                }
                edges.Add(leftEdge);

                // Right edge
                var rightEdge = new Edge(
                    face.Id,
                    "right",
                    (face.TopLeftRow, face.TopLeftCol + face.Size - 1),
                    (face.TopLeftRow + face.Size - 1, face.TopLeftCol + face.Size - 1)
                );
                for (int row = face.TopLeftRow; row < face.TopLeftRow + face.Size; row++)
                {
                    rightEdge.Coordinates.Add((row, face.TopLeftCol + face.Size - 1));
                }
                edges.Add(rightEdge);
            }

            // Clean up adjacent perpendicular edges
            return CleanAdjacentEdges(edges);
        }

        // Function to clean and merge adjacent perpendicular edges
        public static List<Edge> CleanAdjacentEdges(List<Edge> edges)
        {
            var cleanedEdges = new List<Edge>();

            for (int i = 0; i < edges.Count; i++)
            {
                bool merged = false;

                for (int j = i + 1; j < edges.Count; j++)
                {
                    // If two edges are adjacent and aligned, they can be merged or removed
                    if (edges[i].IsAdjacentAndAligned(edges[j]))
                    {
                        // Here, you can either merge them or skip them (depending on your goal)
                        edges.RemoveAt(j);
                        merged = true;
                        break; // Skip this edge, it's been merged
                    }
                }

                if (!merged)
                {
                    cleanedEdges.Add(edges[i]);
                }
            }

            return cleanedEdges;
        }



        // Helper function to find adjacent faces that share a corner with the current face
        public static List<CubeFace> FindAdjacentFaces((int Row, int Col) corner, CubeFace face, List<CubeFace> faces)
        {
            var adjacentFaces = new List<CubeFace>();
            foreach (var otherFace in faces)
            {
                if (otherFace.Id != face.Id)  // Check only other faces
                {
                    // Check if this face shares the same corner
                    if (IsFaceAtCorner(otherFace, corner))
                    {
                        adjacentFaces.Add(otherFace);
                    }
                }
            }
            return adjacentFaces;
        }

        // Check if a face has a corner at the given coordinates
        public static bool IsFaceAtCorner(CubeFace face, (int Row, int Col) corner)
        {
            var faceCorners = new (int Row, int Col)[]
            {
                (face.TopLeftRow, face.TopLeftCol),  // Top-left corner
                (face.TopLeftRow, face.TopLeftCol + face.Size - 1),  // Top-right corner
                (face.TopLeftRow + face.Size - 1, face.TopLeftCol),  // Bottom-left corner
                (face.TopLeftRow + face.Size - 1, face.TopLeftCol + face.Size - 1)  // Bottom-right corner
            };

            return faceCorners.Contains(corner);
        }

        private char[,] GetMap(int rows, int cols)
        {
            char[,] map = new char[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    map[r, c] = c < mapLines[r].Length ? mapLines[r][c] : ' ';
                }
            }
            return map;
        }
    }
}
