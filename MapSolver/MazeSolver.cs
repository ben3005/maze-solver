using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MapSolver
{
    public class MazeSolver
    {
        //public bool IntersectionSolve(AStarIntersectionMazeImage maze, out Stack<AStarIntersectionPoint> route)
        //{
        //    bool hasFoundRoute = false;
        //    bool hasSeenAllSquares = false;
        //    AStarIntersectionPoint currentSquare = maze.StartPoint;
        //    bool[,] visitedSquares = new bool[maze.MazeHeight, maze.MazeWidth];
        //    Stack<AStarIntersectionPoint> currentRoute = new Stack<AStarIntersectionPoint>();
        //    currentRoute.Push(maze.StartPoint);
        //    int backtrackCount = 0;
        //    List<long> getNextTimes = new List<long>();
        //    Stopwatch s = new Stopwatch();
        //    while (!hasFoundRoute && !hasSeenAllSquares)
        //    {
        //        s.Start();
        //        bool hasNextSquare = GetNextSquareAStar(maze, currentSquare, visitedSquares, out AStarIntersectionPoint nextSquare);
        //        s.Stop();
        //        getNextTimes.Add(s.ElapsedMilliseconds);
        //        s.Reset();
        //        if (hasNextSquare)
        //        {
        //            if (currentRoute.Count > 0 && !currentRoute.First().Equals(currentSquare))
        //            {
        //                currentRoute.Push(currentSquare);
        //            }
        //            currentRoute.Push(nextSquare);
        //            visitedSquares[nextSquare.Point.Item1, nextSquare.Point.Item2] = true;
        //            currentSquare = nextSquare;
        //            if (nextSquare.Equals(maze.EndPoint))
        //            {
        //                hasFoundRoute = true;
        //            }
        //        }
        //        else
        //        {
        //            hasSeenAllSquares = CheckAllVisitedSquares(visitedSquares, maze);
        //            currentSquare = currentRoute.Pop();
        //            backtrackCount++;
        //        }
        //    }
        //    Console.WriteLine("AStarIntersection Backtracks: " + backtrackCount);
        //    Console.WriteLine("AStarIntersection Average Get Speed " + getNextTimes.Average() + "ms");
        //    Console.WriteLine("AStarIntersection Get Count " + getNextTimes.Count);
        //    if (hasFoundRoute)
        //    {
        //        route = currentRoute;
        //        return true;
        //    }
        //    route = null;
        //    return true;
        //}
        public bool IntersectionSolve(IntersectionMazeImage maze, out Stack<IntersectionPoint> route)
        {
            bool hasFoundRoute = false;
            bool hasSeenAllSquares = false;
            maze.StartPoint.HasVisited = true;
            IntersectionPoint currentSquare = maze.StartPoint;
            Stack<IntersectionPoint> currentRoute = new Stack<IntersectionPoint>();
            currentRoute.Push(maze.StartPoint);
            int backtrackCount = 0;
            List<long> getNextTimes = new List<long>();
            List<long> checkAllVisitedTimes = new List<long>();
            Stopwatch s = new Stopwatch();
            while (!hasFoundRoute && !hasSeenAllSquares)
            {
                s.Start();
                bool hasNextSquare = GetNextSquareDepth(maze, currentSquare, out IntersectionPoint nextSquare);
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
                    nextSquare.HasVisited = true;
                    // visitedSquares[nextSquare.Point.Item1, nextSquare.Point.Item2] = true;
                    currentSquare = nextSquare;
                    if (nextSquare.Equals(maze.EndPoint))
                    {
                        maze.EndPoint.HasVisited = true;
                        hasFoundRoute = true;
                    }
                }
                else
                {
                    s.Start();
                    hasSeenAllSquares = CheckAllVisitedSquares(maze);
                    s.Stop();
                    checkAllVisitedTimes.Add(s.ElapsedMilliseconds);
                    s.Reset();
                    currentSquare = currentRoute.Pop();
                    backtrackCount++;
                }
            }
            Console.WriteLine("Intersection Backtracks: " + backtrackCount);
            Console.WriteLine("Intersection Get Count " + getNextTimes.Count);
            Console.WriteLine("Intersection Total Get Time " + getNextTimes.Sum());
            Console.WriteLine("Intersection Check Visited Count" + checkAllVisitedTimes.Count);
            Console.WriteLine("Intersection Check Visited Total Time" + checkAllVisitedTimes.Sum());
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
            Console.WriteLine("Naive Get Count " + getNextTimes.Count);
            Console.WriteLine("Naive Total Get Time " + getNextTimes.Sum());
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

        private bool CheckAllVisitedSquares(IntersectionMazeImage maze)
        {
            IntersectionPoint point;
            for (int i = 0; i < maze.Points.Length; i++)
            {
                point = maze.Points[i];
                if (!point.HasVisited)
                {
                    return false;
                }
            }
            if (!maze.EndPoint.HasVisited)
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

        private bool GetNextSquareDepth(IntersectionMazeImage maze, IntersectionPoint currentSquare, out IntersectionPoint NextSquare)
        {
            IntersectionPoint nextLocation = null;
            NextSquare = null;
            IntersectionPoint intersection = new IntersectionPoint();
            for (int i = 0; i < currentSquare.ConnectedIntersections.Count; i++)
            {
                intersection = currentSquare.ConnectedIntersections[i];
                if (!intersection.HasVisited)
                {
                    nextLocation = intersection;
                    break;
                }
            }
            if (nextLocation != null)
            {
                if (maze.EndPoint.Equals(nextLocation))
                {
                    NextSquare = maze.EndPoint;
                    return true;
                }
                NextSquare = nextLocation;
                return true;
            }
            return false;
        }
    }
}
