using System;
using System.Collections.Generic;
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
            while (!hasFoundRoute && !hasSeenAllSquares)
            {
                bool hasNextSquare = GetNextSquare(maze, currentSquare, visitedSquares, out IntersectionPoint nextSquare);
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

        private bool CheckAllVisitedSquares(bool[,] visitedSquares, IntersectionMazeImage maze)
        {
            bool hasFoundUnvisited = false;
            maze.Points.ForEach(i =>
            {
                i.ForEach(j =>
                {
                    if (!visitedSquares[j.Point.Item1, j.Point.Item2]) {
                        hasFoundUnvisited = true;
                    }
                });
            });
            if (!visitedSquares[maze.EndPoint.Point.Item1, maze.EndPoint.Point.Item2])
            {
                hasFoundUnvisited = true;
            }
            return !hasFoundUnvisited;
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

        private bool GetNextSquare(IntersectionMazeImage maze, IntersectionPoint currentSquare, bool[,] visitedSquares, out IntersectionPoint NextSquare)
        {
            if (currentSquare.ConnectedIntersections.Exists(point => !visitedSquares[point.Item1, point.Item2]))
            {
                var nextLocation = currentSquare.ConnectedIntersections.Where(point => !visitedSquares[point.Item1, point.Item2]).First();
                if (maze.Points.Exists(i => i.Exists(j => j.Point.Equals(nextLocation))))
                {
                    NextSquare = maze.Points.Where(i => i.Exists(j => j.Point.Equals(nextLocation))).First().Where(j => j.Point.Equals(nextLocation)).First();
                    return true;
                }
                if (maze.EndPoint.Point.Equals(nextLocation))
                {
                    NextSquare = maze.EndPoint;
                    return true;
                }
            }
            NextSquare = null;
            return false;
        }
    }
}
