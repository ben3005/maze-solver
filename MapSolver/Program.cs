using System;
using System.Collections.Generic;
using System.Diagnostics;
// ReSharper disable UnusedMember.Local

namespace MapSolver
{
    class Program
    {
        private const string TINY = @"examples\tiny.png";
        private const string SMALL = @"examples\small.png";
        private const string NORMAL = @"examples\normal.png";
        private const string BRAID200 = @"examples\braid200.png";
        private const string TWOK = @"examples\perfect2k.png";
        private const string FOURK = @"examples\perfect4k.png";
        private const string COMBO400 = @"examples\combo400.png";
        private const string MAZE_TO_RUN = FOURK;
        static MazeImageFactory mif = new MazeImageFactory();
        static IntersectionMazeImageFactory imif = new IntersectionMazeImageFactory();
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
            var mi = mif.CreateNaiveMaze(MAZE_TO_RUN);
            s.Stop();
            Console.WriteLine("Created maze representation in: " + s.ElapsedMilliseconds + "ms");
            s.Reset();
            s.Start();
            if (ms.NaiveSolve(mi, out Stack<Tuple<int, int>> solution))
            {
                s.Stop();
                Console.WriteLine("Found solution in: " + s.ElapsedMilliseconds + "ms");
                msw.CreateSolutionImage(solution, MAZE_TO_RUN);
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
            var imi = imif.CreateIntersectionMaze(MAZE_TO_RUN);
            s.Stop();
            Console.WriteLine("Created maze representation in: " + s.ElapsedMilliseconds + "ms");
            s.Reset();
            s.Start();
            if (ms.IntersectionSolve(imi, out Stack<IntersectionPoint> intSolution))
            {
                s.Stop();
                Console.WriteLine("Found solution in: " + s.ElapsedMilliseconds + "ms");
                msw.CreateSolutionImage(intSolution, MAZE_TO_RUN);
            }
            s.Stop();
        }
    }
}