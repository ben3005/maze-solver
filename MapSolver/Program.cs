using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MapSolver
{
    class Program
    {
        const string NORMAL = @"examples\normal.png";
        const string BRAID200 = @"examples\braid200.png";
        const string TWOK = @"examples\perfect2k.png";
        const string FOURK = @"examples\perfect4k.png";

        static void Main(string[] args)
        {
            MazeImageFactory mif = new MazeImageFactory();
            NaiveMazeImage mi = mif.CreateNaiveMaze(BRAID200);
            IntersectionMazeImage imi = mif.CreateIntersectionMaze(BRAID200);
            MazeSolver ms = new MazeSolver();
            MazeSolutionWriter msw = new MazeSolutionWriter();
            Stopwatch s = new Stopwatch();
            s.Start();
            if (ms.NaiveSolve(mi, out Stack<Tuple<int, int>> solution))
            {
                s.Stop();
                Console.WriteLine("Found solution in: " + s.ElapsedMilliseconds + "ms");
                msw.CreateSolutionImage(solution, BRAID200);
            }
            s.Reset();
            Console.WriteLine("Created maze image");
            s.Start();
            if (ms.IntersectionSolve(imi, out Stack<IntersectionPoint> intSolution))
            {
                s.Stop();
                Console.WriteLine("Found solution in: " + s.ElapsedMilliseconds + "ms");
                msw.CreateSolutionImage(intSolution, BRAID200);
            }

            Console.ReadKey();
        }
    }
}