using System;
using System.Collections.Generic;

namespace MapSolver
{
    public class IntersectionMazeImage
    {
        public List<List<IntersectionPoint>> Points { get; set; }
        public IntersectionPoint StartPoint { get; set; }
        public IntersectionPoint EndPoint { get; set; }
        public int MazeHeight { get; set; }
        public int MazeWidth { get; set; }

        public IntersectionMazeImage()
        {
            Points = new List<List<IntersectionPoint>>();
        }
    }

    public class IntersectionPoint
    {
        public bool HasUpward { get; set; }
        public bool HasDownward { get; set; }
        public bool HasLeft { get; set; }
        public bool HasRight { get; set; }
        public Tuple<int, int> Point { get; set; }
        public List<Tuple<int, int>> ConnectedIntersections { get; set; }

        public IntersectionPoint()
        {
            ConnectedIntersections = new List<Tuple<int, int>>();
        }
    }
}