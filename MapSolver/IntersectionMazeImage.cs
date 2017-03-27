using System;
using System.Collections.Generic;

namespace MapSolver
{
    public class IntersectionMazeImage
    {
        public IntersectionPoint[] Points { get; set; }
        public int[] ISections { get; set; }
        public IntersectionPoint StartPoint { get; set; }
        public IntersectionPoint EndPoint { get; set; }
        public int MazeHeight { get; set; }
        public int MazeWidth { get; set; }

        public IntersectionMazeImage(int width, int height)
        {
            MazeHeight = height;
            MazeWidth = width;
            ISections = new int[width];
        }
    }

    public class IntersectionPoint
    {
        public bool HasUpward { get; set; }
        public bool HasDownward { get; set; }
        public bool HasLeft { get; set; }
        public bool HasRight { get; set; }
        public bool HasVisited { get; set; }
        public Tuple<int, int> Point { get; set; }
        public List<IntersectionPoint> ConnectedIntersections { get; set; }

        public IntersectionPoint()
        {
            ConnectedIntersections = new List<IntersectionPoint>();
        }
    }
}