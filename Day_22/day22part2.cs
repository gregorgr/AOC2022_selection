using System;
using System.Collections;
using System.Collections.Generic;
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

    // Class to represent an edge
    public class Edge
    {
        public (int Row, int Col) Start { get; set; }  // Start coordinate
        public (int Row, int Col) End { get; set; }    // End coordinate
        public List<(int Row, int Col)> Coordinates { get; set; }  // List of all coordinates on the edge
        public int StartFaceId { get; set; }  // The face ID the edge belongs to
        public string EdgeType { get; set; }  // "top", "bottom", "left", "right"

        public Edge(int startFaceId, string edgeType, (int Row, int Col) start, (int Row, int Col) end)
        {
            StartFaceId = startFaceId;
            EdgeType = edgeType;
            Start = start;
            End = end;
            Coordinates = new List<(int Row, int Col)>();  // Initialize coordinates list
        }
    }



    // Class representing a V-shaped Corner node (corner between two faces)
    public class VCornerNode
    {
        public (int Row, int Col) Position { get; set; }
        public int[] FaceIds { get; set; }  // Three face IDs that meet at this node
        public (int X, int Y) Direction { get; set; }  // Vector showing direction change

        public VCornerNode(int[] faceIds, (int Row, int Col) position, (int X, int Y) direction)
        {
            FaceIds = faceIds;
            Position = position;
            Direction = direction;
        }
    }

    public class Day22part2
    {
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

        public int Result { get; set; }

        private string[] mapLines;
        public Day22part2(string puzzleFile) {


            var input = File.ReadAllText($"../../../{puzzleFile}")
                .Replace("\r\n", "\n")// Normalize line breaks \r\n to \n
                .Split("\n\n");

            var _mapLines = input[0].Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)).ToArray();
            mapLines = _mapLines;
            var path = input[1].Trim();

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
                var faces = FindAllFaces(map, faceSize, rows, cols);

                // Output all faces with their coordinates and sizes
                foreach (var face in faces)
                {
                    Console.WriteLine($"Face  {face.Id}  at ({face.TopLeftRow}, {face.TopLeftCol}) with size {face.Size}");

                }
                Console.WriteLine("");
                // Identify the outer edges for each face
                // var edges = GetAllOuterEdges(faces, map);
                int i = 1;
                var edges = GetAllOuterEdges(faces);
                foreach (var edge in edges)
                {
                    Console.WriteLine($"{i} Edge of Face {edge.StartFaceId} ({edge.EdgeType}) starts at {edge.Start} and ends at {edge.End}");
                    i++;
                }
                Console.WriteLine("");
                // Identify V-shaped corner nodes (corners where three faces meet)
               // var vCornerNodes = FindVCornerNodes(faces, edges, faceSize);
                var vCornerNodes = FindVCornerNodes(faces, faceSize);
                foreach (var vCornerNode in vCornerNodes)
                {
                    Console.WriteLine($"V-corner node between Face {string.Join(", ", vCornerNode.FaceIds)} at position {vCornerNode.Position} with vector direction {vCornerNode.Direction}");
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message.ToString());
            }
            /*
             * - go to first V corner/node and do:
move on both edges in the direction from V node to next corner (for example one vector goes up and second vector goes left) Those 2 vectors indicate edges that connect each if faces. if edge was not jet connected, it is valid edge (like 3 and 4), but if face 6 connects to face 1 and then we again move on face 6 and 1 it is not valid move.
- when we find not valid move, we have to go to next V corner (node) and repeat process
after all 3 V nodes/corners vere visited and searched, we found all edges that connect faces and new dirrections of moving.
             * */


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


        // Get all outer edges for each face
        /*
        public static List<Edge> GetAllOuterEdges(List<CubeFace> faces, char[,] map)
        {
            var edges = new List<Edge>();

            foreach (var face in faces)
            {
                // Get the edges for each face
                edges.Add(GetEdge(face, map, "top", face.TopLeftRow, face.TopLeftCol, 0, 1));   // Top edge
                edges.Add(GetEdge(face, map, "bottom", face.TopLeftRow + face.Size - 1, face.TopLeftCol, 0, 1));  // Bottom edge
                edges.Add(GetEdge(face, map, "left", face.TopLeftRow, face.TopLeftCol, 1, 0));  // Left edge
                edges.Add(GetEdge(face, map, "right", face.TopLeftRow, face.TopLeftCol + face.Size - 1, 1, 0));  // Right edge
            }

            return edges;
        }
        */

        public static List<Edge> GetAllOuterEdges(List<CubeFace> faces)
        {
            var edges = new List<Edge>();

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

            return edges;
        }

        /*
        // Function to get an edge (either top, bottom, left, or right)
        public static Edge GetEdge(CubeFace face, char[,] map, string edgeType, int startRow, int startCol, int rowStep, int colStep)
        {
            var edge = new Edge(face.Id, edgeType);
            for (int i = 0; i < face.Size; i++)
            {
                edge.Coordinates.Add((startRow + i * rowStep, startCol + i * colStep));
            }
            return edge;
        }
        */
        // Find V-shaped corner nodes (where three edges meet and change direction)
        public static List<VCornerNode> FindVCornerNodes(List<CubeFace> faces, int faceSize)
        {
            var vCornerNodes = new List<VCornerNode>();

            // Iterate through each face
            foreach (var face in faces)
            {
                // Check each corner of the face
                var corners = new (int Row, int Col)[]
                {
                    (face.TopLeftRow, face.TopLeftCol),  // Top-left corner
                    (face.TopLeftRow, face.TopLeftCol + faceSize - 1),  // Top-right corner
                    (face.TopLeftRow + faceSize - 1, face.TopLeftCol),  // Bottom-left corner
                    (face.TopLeftRow + faceSize - 1, face.TopLeftCol + faceSize - 1)  // Bottom-right corner
                };

                // For each corner, find the two other adjacent faces
                foreach (var corner in corners)
                {
                    var adjacentFaces = FindAdjacentFaces(corner, face, faces);
                    if (adjacentFaces.Count == 2)
                    {
                        // Found a V-corner where three faces meet
                        var vCorner = new VCornerNode(new int[] { face.Id, adjacentFaces[0].Id, adjacentFaces[1].Id }, corner, (1, -1));
                        vCornerNodes.Add(vCorner);
                    }
                }
            }

            return vCornerNodes;
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
