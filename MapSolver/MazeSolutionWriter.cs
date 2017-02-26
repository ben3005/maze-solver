using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapSolver
{
    public class MazeSolutionWriter
    {
        public void CreateSolutionImage(Stack<Tuple<int, int>> solution, string mazeFile)
        {
            Bitmap img = new Bitmap(mazeFile);
            while (solution.Count> 0)
            {
                var step = solution.Pop();
                img.SetPixel(step.Item1, step.Item2, Color.Yellow);
            }
            img.Save("solution.png");
        }
    }
}