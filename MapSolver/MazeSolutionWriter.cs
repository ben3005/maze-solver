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
                img.SetPixel(step.ICoord, step.JCoord, Color.Yellow);
                if (previous != null)
                {
                    if (previous.ICoord != step.ICoord)
                    {
                        if (previous.ICoord > step.ICoord)
                        {
                            for (int i = step.ICoord + 1; i < previous.ICoord; i++)
                            {
                                img.SetPixel(i, step.JCoord, Color.Green);
                            }
                        }
                        else if (previous.ICoord < step.ICoord)
                        {
                            for (int i = previous.ICoord + 1; i < step.ICoord; i++)
                            {
                                img.SetPixel(i, step.JCoord, Color.Green);
                            }
                        }
                    }
                    else if (previous.JCoord != step.JCoord)
                    {
                        if (previous.JCoord > step.JCoord)
                        {
                            for (int j = step.JCoord + 1; j < previous.JCoord; j++)
                            {
                                img.SetPixel(step.ICoord, j, Color.Green);
                            }
                        }
                        else if (previous.JCoord < step.JCoord)
                        {
                            for (int j = previous.JCoord + 1; j < step.JCoord; j++)
                            {
                                img.SetPixel(step.ICoord, j, Color.Green);
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