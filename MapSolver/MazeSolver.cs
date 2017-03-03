using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MapSolver
{
    public class MazeSolver
    {
        public bool IntersectionSolve(IntersectionMazeImage maze, out Stack<IntersectionPoint> route)
        {
            bool hasFoundRoute = false;
            bool hasSeenAllSquares = false;
            IntersectionPoint currentSquare = maze.StartPoint;
            bool[,] visitedSquares = new bool[maze.MazeHeight, maze.MazeWidth];
            Stack<IntersectionPoint> currentRoute = new Stack<IntersectionPoint>();
            currentRoute.Push(maze.StartPoint);
            int backtrackCount = 0;
            List<long> getNextTimes = new List<long>();
            Stopwatch s = new Stopwatch();
            while (!hasFoundRoute && !hasSeenAllSquares)
            {
                s.Start();
                bool hasNextSquare = GetNextSquareDepth(maze, currentSquare, visitedSquares, out IntersectionPoint nextSquare);
                s.Stop();
                getNextTimes.Add(s.ElapsedMilliseconds);
                s.Reset();
                if (hasNextSquare)
                {
                    if (currentRoute.Count > 0 && !currentRoute.First().Equals(currentSquare))
                    {
                        currentRoute.Push(currentSquare);
                    }
                    currentRoute.Push(nextSquare);
                    visitedSquares[nextSquare.Point.Item1, nextSquare.Point.Item2] = true;
                    currentSquare = nextSquare;
                    if (nextSquare.Equals(maze.EndPoint))
                    {
                        hasFoundRoute = true;
                    }
                }
                else
                {
                    hasSeenAllSquares = CheckAllVisitedSquares(visitedSquares, maze);
                    currentSquare = currentRoute.Pop();
                    backtrackCount++;
                }
            }
            Console.WriteLine("Intersection Backtracks: " + backtrackCount);
            Console.WriteLine("Intersection Average Get Speed " + getNextTimes.Average() + "ms");
            Console.WriteLine("Intersection Get Count " + getNextTimes.Count);
            if (hasFoundRoute)
            {
                route = currentRoute;
                return true;
            }
            route = null;
            return true;
        }

        public bool NaiveSolve(NaiveMazeImage maze, out Stack<Tuple<int, int>> route)
        {
            bool hasFoundRoute = false;
            bool hasSeenAllSquares = false;
            Tuple<int, int> currentSquare = maze.StartPoint;
            bool[,] visitedSquares = new bool[maze.MazeHeight, maze.MazeWidth];
            Stack<Tuple<int, int>> currentRoute = new Stack<Tuple<int, int>>();
            currentRoute.Push(maze.StartPoint);
            int backtrackCount = 0;
            Stopwatch s = new Stopwatch();
            List<long> getNextTimes = new List<long>();
            while (!hasFoundRoute && !hasSeenAllSquares)
            {
                s.Start();
                bool hasNextSquare = GetNextSquare(maze, currentSquare, visitedSquares, out Tuple<int, int> nextSquare);
                s.Stop();
                getNextTimes.Add(s.ElapsedMilliseconds);
                s.Reset();
                if (hasNextSquare)
                {
                    if (currentRoute.Count > 0 && !currentRoute.First().Equals(currentSquare))
                    {
                        currentRoute.Push(currentSquare);
                    }
                    currentRoute.Push(nextSquare);
                    visitedSquares[nextSquare.Item1, nextSquare.Item2] = true;
                    currentSquare = nextSquare;
                    if (nextSquare.Equals(maze.EndPoint))
                    {
                        hasFoundRoute = true;
                    }
                }
                else
                {
                    backtrackCount++;
                    hasSeenAllSquares = CheckAllVisitedSquares(visitedSquares);
                    currentSquare = currentRoute.Pop();
                }
            }
            Console.WriteLine("Naive Backtracks: " + backtrackCount);
            Console.WriteLine("Naive Average Get Speed " + getNextTimes.Average() + "ms");
            Console.WriteLine("Naive Get Count " + getNextTimes.Count);
            if (hasFoundRoute)
            {
                route = currentRoute;
                return true;
            }
            route = null;
            return true;
        }

        private bool CheckAllVisitedSquares(bool[,] visitedSquares)
        {
            for (int i = 0; i < visitedSquares.GetLength(0); i++)
            {
                for (int j = 0; j < visitedSquares.GetLength(1); j++)
                {
                    if (!visitedSquares[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool CheckAllVisitedSquares(bool[,] visitedSquares, IntersectionMazeImage maze)
        {
            foreach (var i in maze.Points)
            {
                foreach (var j in i)
                {
                    if (!visitedSquares[j.Point.Item1, j.Point.Item2])
                    {
                        return false;
                    }
                }
            }
            if (!visitedSquares[maze.EndPoint.Point.Item1, maze.EndPoint.Point.Item2])
            {
                return false;
            }
            return true;
        }

        private bool GetNextSquare(NaiveMazeImage maze, Tuple<int, int> currentSquare, bool[,] visitedSquares, out Tuple<int, int> NextSquare)
        {
            //Move Up
            if (currentSquare.Item2 - 1 > 0 && maze.Points[currentSquare.Item1][currentSquare.Item2 - 1] && !visitedSquares[currentSquare.Item1, currentSquare.Item2 - 1])
            {
                NextSquare = new Tuple<int, int>(currentSquare.Item1, currentSquare.Item2 - 1);
                return true;
            }
            //Move Right
            if (currentSquare.Item1 + 1 < maze.MazeWidth && maze.Points[currentSquare.Item1 + 1][currentSquare.Item2] && !visitedSquares[currentSquare.Item1 + 1, currentSquare.Item2])
            {
                NextSquare = new Tuple<int, int>(currentSquare.Item1 + 1, currentSquare.Item2);
                return true;
            }
            //Move Down
            if (currentSquare.Item2 + 1 < maze.MazeHeight && maze.Points[currentSquare.Item1][currentSquare.Item2 + 1] && !visitedSquares[currentSquare.Item1, currentSquare.Item2 + 1])
            {
                NextSquare = new Tuple<int, int>(currentSquare.Item1, currentSquare.Item2 + 1);
                return true;
            }
            //Move Left
            if (currentSquare.Item1 - 1 > 0 && maze.Points[currentSquare.Item1 - 1][currentSquare.Item2] && !visitedSquares[currentSquare.Item1 - 1, currentSquare.Item2])
            {
                NextSquare = new Tuple<int, int>(currentSquare.Item1 - 1, currentSquare.Item2);
                return true;
            }
            NextSquare = null;
            return false;
        }

        private bool GetNextSquareDepth(IntersectionMazeImage maze, IntersectionPoint currentSquare, bool[,] visitedSquares, out IntersectionPoint NextSquare)
        {
            Tuple<int, int> nextLocation = null;
            NextSquare = null;
            foreach (var intersection in currentSquare.ConnectedIntersections)
            {
                if (intersection.Item2 > currentSquare.Point.Item2 && !visitedSquares[intersection.Item1, intersection.Item2])
                {
                    nextLocation = intersection;
                    break;
                }
                if (!visitedSquares[intersection.Item1, intersection.Item2])
                {
                    nextLocation = intersection;
                    break;
                }
            }
            if (nextLocation != null)
            {
                if (maze.EndPoint.Point.Equals(nextLocation))
                {
                    NextSquare = maze.EndPoint;
                    return true;
                }
                foreach (var j in maze.Points[nextLocation.Item1])
                {
                    if (j.Point.Equals(nextLocation))
                    {
                        NextSquare = j;
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
