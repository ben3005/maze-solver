using System.Collections.Generic;

namespace MapSolver
{
    public class IntersectionMazeImage
    {
        public IntersectionPoint[] Points { get; set; }
        // ReSharper disable once InconsistentNaming
        public IntersectionPoint StartPoint { get; set; }
        public IntersectionPoint EndPoint { get; set; }
        public int TotalVisited { get; set; }

        public IntersectionMazeImage()
        {
            TotalVisited = 0;
        }
    }

    public class IntersectionPoint
    {
        public bool HasVisited { get; set; }
        // ReSharper disable once InconsistentNaming
        public int ICoord { get; set; }
        public int JCoord { get; set; }
        public List<IntersectionPoint> ConnectedIntersections { get; }

        public IntersectionPoint()
        {
            ConnectedIntersections = new List<IntersectionPoint>();
        }
    }
}