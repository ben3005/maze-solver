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
            //NaiveMazeImage mi = mif.CreateNaiveMaze("perfect10k.png");
            IntersectionMazeImage imi = mif.CreateIntersectionMaze("tiny.png");
            MazeSolver ms = new MazeSolver();
            MazeSolutionWriter msw = new MazeSolutionWriter();
            //Stopwatch s = new Stopwatch();
            //s.Start();
            //if (ms.NaiveSolve(mi, out Stack<Tuple<int, int>> solution))
            //{
            //    s.Stop();
            //    Console.WriteLine("Found solution in: " + s.ElapsedMilliseconds + "ms");
            //    msw.CreateSolutionImage(solution, "perfect10k.png");
            //}
            Console.ReadKey();
        }
    }
}