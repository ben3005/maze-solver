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
            while (solution.Count > 0)
            {
                var step = solution.Pop();
                img.SetPixel(step.Item1, step.Item2, Color.Yellow);
            }
            img.Save("solution.png");
        }

        public void CreateSolutionImage(Stack<IntersectionPoint> solution, string mazeFile)
        {
            Bitmap img = new Bitmap(mazeFile);
            IntersectionPoint previous = null;
            while (solution.Count > 0)
            {
                var step = solution.Pop();
                img.SetPixel(step.Point.Item1, step.Point.Item2, Color.Yellow);
                if (previous != null)
                {
                    if (previous.Point.Item1 != step.Point.Item1)
                    {
                        if (previous.Point.Item1 > step.Point.Item1)
                        {
                            for (int i = step.Point.Item1 + 1; i < previous.Point.Item1; i++)
                            {
                                img.SetPixel(i, step.Point.Item2, Color.Green);
                            }
                        }
                        else if (previous.Point.Item1 < step.Point.Item1)
                        {
                            for (int i = previous.Point.Item1 + 1; i < step.Point.Item1; i++)
                            {
                                img.SetPixel(i, step.Point.Item2, Color.Green);
                            }
                        }
                    }
                    else if (previous.Point.Item2 != step.Point.Item2)
                    {
                        if (previous.Point.Item2 > step.Point.Item2)
                        {
                            for (int j = step.Point.Item2 + 1; j < previous.Point.Item2; j++)
                            {
                                img.SetPixel(step.Point.Item1, j, Color.Green);
                            }
                        }
                        else if (previous.Point.Item2 < step.Point.Item2)
                        {
                            for (int j = previous.Point.Item2 + 1; j < step.Point.Item2; j++)
                            {
                                img.SetPixel(step.Point.Item1, j, Color.Green);
                            }
                        }
                    }
                }
                previous = step;
            }
            img.Save("solution-intersection.png");
        }
    }
}