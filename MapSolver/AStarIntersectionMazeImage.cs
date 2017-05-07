using System;
using System.Collections.Generic;
using System.Text;

namespace MapSolver
{
    public class AStarIntersectionMazeImage
    {
        public List<List<AStarIntersectionPoint>> Points { get; set; }
        public AStarIntersectionPoint StartPoint { get; set; }
        public AStarIntersectionPoint EndPoint { get; set; }
        public int MazeHeight { get; set; }
        public int MazeWidth { get; set; }

        public AStarIntersectionMazeImage()
        {
            Points = new List<List<AStarIntersectionPoint>>();
        }
    }

    public class AStarIntersectionPoint
    {
        public Tuple<int, int> Point { get; set; }
        public double Weighting { get; set; }
        public List<Tuple<int, int>> ConnectedIntersections { get; set; }

        public AStarIntersectionPoint()
        {
            ConnectedIntersections = new List<Tuple<int, int>>();
        }
    }
}
