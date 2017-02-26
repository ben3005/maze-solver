using System;
using System.Collections.Generic;
using System.Linq;

namespace MapSolver
{
    public class MazeSolver
    {
        public bool IntersectionSolve(IntersectionMazeImage maze, out Stack<Tuple<int, int>> route)
        {
            bool hasFoundRoute = false;
            bool hasSeenAllSquares = false;
            Tuple<int, int> currentSquare = maze.StartPoint;
            bool[,] visitedSquares = new bool[maze.MazeHeight, maze.MazeWidth];
            Stack<Tuple<int, int>> currentRoute = new Stack<Tuple<int, int>>();
            currentRoute.Push(maze.StartPoint);
            while (!hasFoundRoute && !hasSeenAllSquares)
            {
                bool hasNextSquare = GetNextSquare(maze, currentSquare, visitedSquares, out Tuple<int, int> nextSquare);
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
                    hasSeenAllSquares = CheckAllVisitedSquares(visitedSquares);
                    currentSquare = currentRoute.Pop();
                }
            }

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
            while (!hasFoundRoute && !hasSeenAllSquares)
            {
                bool hasNextSquare = GetNextSquare(maze, currentSquare, visitedSquares, out Tuple<int, int> nextSquare);
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
                    hasSeenAllSquares = CheckAllVisitedSquares(visitedSquares);
                    currentSquare = currentRoute.Pop();
                }
            }

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

        private bool GetNextSquare(IntersectionMazeImage maze, Tuple<int, int> currentSquare, bool[,] visitedSquares, out Tuple<int, int> NextSquare)
        {
            //Move Up
            if (maze.Points[currentSquare.Item1].Exists(j => j.Point.Item2 != currentSquare.Item2 && !visitedSquares[j.Point.Item1, j.Point.Item2]))
            {
                var nextIntersection = maze.Points[currentSquare.Item1].Where(j => j.Point.Item2 != currentSquare.Item2 && !visitedSquares[j.Point.Item1, j.Point.Item2]).First();
                NextSquare = new Tuple<int, int>(nextIntersection.Point.Item1, nextIntersection.Point.Item2);
                return true;
            }
            //Move Right
            Predicate<IntersectionPoint> sameJ = j => j.Point.Item2 == currentSquare.Item2 && j.Point.Item1 > currentSquare.Item1 && !visitedSquares[j.Point.Item1, j.Point.Item2];
            Predicate<List<IntersectionPoint>> moveRight = i => i.Exists(sameJ);
            if (maze.Points.Exists(moveRight))
            {
                var nextIntersection = maze.Points.Where(x => moveRight(x)).First();
                NextSquare = new Tuple<int, int>(nextIntersection.Point.Item1, nextIntersection.Point.Item2);
                return true;
            }
            //Move Down

            //Move Left

            NextSquare = null;
            return false;
        }
    }
}
