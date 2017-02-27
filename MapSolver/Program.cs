using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MapSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            MazeImageFactory mif = new MazeImageFactory();
            NaiveMazeImage mi = mif.CreateNaiveMaze(@"examples\tiny.png");
            IntersectionMazeImage imi = mif.CreateIntersectionMaze(@"examples\tiny.png");
            MazeSolver ms = new MazeSolver();
            MazeSolutionWriter msw = new MazeSolutionWriter();
            Stopwatch s = new Stopwatch();
            s.Start();
            if (ms.NaiveSolve(mi, out Stack<Tuple<int, int>> solution))
            {
                s.Stop();
                Console.WriteLine("Found solution in: " + s.ElapsedMilliseconds + "ms");
                msw.CreateSolutionImage(solution, @"examples\tiny.png");
            }
            Console.ReadKey();
        }
    }
}