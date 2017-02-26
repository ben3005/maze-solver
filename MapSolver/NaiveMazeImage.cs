using System;
using System.Collections.Generic;

namespace MapSolver
{
    public class NaiveMazeImage
    {
        public List<List<bool>> Points { get; set; }
        public Tuple<int, int> StartPoint { get; set; }
        public Tuple<int, int> EndPoint { get; set; }
        public int MazeHeight { get; set; }
        public int MazeWidth { get; set; }

        public NaiveMazeImage()
        {
            Points = new List<List<bool>>();
        }
    }
}