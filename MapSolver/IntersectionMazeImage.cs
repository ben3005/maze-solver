using System;
using System.Collections.Generic;

namespace MapSolver
{
    public class IntersectionMazeImage
    {
        public List<List<IntersectionPoint>> Points { get; set; }
        public Tuple<int, int> StartPoint { get; set; }
        public Tuple<int, int> EndPoint { get; set; }
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
    }
}