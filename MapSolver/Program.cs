using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MapSolver
{
    class Program
    {
        const string TINY = @"examples\tiny.png";
        const string SMALL = @"examples\small.png";
        const string NORMAL = @"examples\normal.png";
        const string BRAID200 = @"examples\braid200.png";
        const string TWOK = @"examples\perfect2k.png";
        const string FOURK = @"examples\perfect4k.png";
        const string COMBO400 = @"examples\combo400.png";
        static readonly string mazeToRun = FOURK;
        static MazeImageFactory mif = new MazeImageFactory();
        static Stopwatch s = new Stopwatch();
        static MazeSolutionWriter msw = new MazeSolutionWriter();
        static MazeSolver ms = new MazeSolver();

        static void Main(string[] args)
        {
            SolveNaive();
            SolveIntersection();
            Console.ReadKey();
        }

        static void SolveNaive()
        {
            s.Start();
            NaiveMazeImage mi = mif.CreateNaiveMaze(mazeToRun);
            s.Stop();
            Console.WriteLine("Created maze representation in: " + s.ElapsedMilliseconds + "ms");
            s.Reset();
            s.Start();
            if (ms.NaiveSolve(mi, out Stack<Tuple<int, int>> solution))
            {
                s.Stop();
                Console.WriteLine("Found solution in: " + s.ElapsedMilliseconds + "ms");
                msw.CreateSolutionImage(solution, mazeToRun);
            }
            if (s.IsRunning)
            {
                s.Stop();
            }
            s.Reset();
        }

        static void SolveIntersection()
        {
            s.Start();
            IntersectionMazeImage imi = mif.CreateIntersectionMaze(mazeToRun);
            s.Stop();
            Console.WriteLine("Created maze representation in: " + s.ElapsedMilliseconds + "ms");
            s.Reset();
            s.Start();
            if (ms.IntersectionSolve(imi, out Stack<IntersectionPoint> intSolution))
            {
                s.Stop();
                Console.WriteLine("Found solution in: " + s.ElapsedMilliseconds + "ms");
                msw.CreateSolutionImage(intSolution, mazeToRun);
            }
            s.Stop();
        }
    }
}