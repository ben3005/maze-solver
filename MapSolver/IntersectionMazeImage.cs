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
        public int TotalVisited { get; set; }

        public IntersectionMazeImage(int width, int height)
        {
            MazeHeight = height;
            MazeWidth = width;
            ISections = new int[width];
            TotalVisited = 0;
        }
    }

    public class IntersectionPoint
    {
        public bool HasVisited { get; set; }
        public int ICoord { get; set; }
        public int JCoord { get; set; }
        public List<IntersectionPoint> ConnectedIntersections { get; set; }

        public IntersectionPoint()
        {
            ConnectedIntersections = new List<IntersectionPoint>();
        }
    }
}