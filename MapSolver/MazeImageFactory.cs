using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MapSolver
{
    public class MazeImageFactory
    {
        public AStarIntersectionMazeImage CreateAStarIntersectionMaze(string filePath)
        {
            AStarIntersectionMazeImage newMaze = new AStarIntersectionMazeImage();
            Bitmap img = new Bitmap(filePath);
            bool hasSeenWallInJ = false;
            bool[] hasSeenWallInI = new bool[img.Width];
            Tuple<int, int>[] previousIIntersection = new Tuple<int, int>[img.Width];
            Tuple<int, int> previousJIntersection = null;
            newMaze.MazeHeight = img.Height;
            newMaze.MazeWidth = img.Width;
            for (int i = 0; i < img.Width; i++)
            {
                newMaze.Points.Add(new List<AStarIntersectionPoint>());
                for (int j = img.Height - 1; j >= 0; j--)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (IsPixelSpace(pixel))
                    {
                        if (j == img.Height - 1)
                        {
                            newMaze.EndPoint = new AStarIntersectionPoint()
                            {
                                HasUpward = false,
                                HasRight = false,
                                HasDownward = true,
                                HasLeft = false,
                                Weighting = 0,
                                Point = new Tuple<int, int>(i, j)
                            };
                        }
                        if (j == 0)
                        {
                            newMaze.StartPoint = new AStarIntersectionPoint()
                            {
                                HasUpward = false,
                                HasRight = false,
                                HasDownward = true,
                                HasLeft = false,
                                Weighting = CalculateWeighting(i, j, newMaze.EndPoint.Point),
                                Point = new Tuple<int, int>(i, j)
                            };
                        }
                        if (IsIntersection(i, j, img, out Tuple<bool, bool, bool, bool> directions))
                        {
                            List<Tuple<int, int>> connectedIntersections = new List<Tuple<int, int>>();
                            if (!hasSeenWallInJ)
                            {
                                connectedIntersections.Add(previousJIntersection);
                                newMaze.Points[previousJIntersection.Item1].Where(jCol => jCol.Point.Item2 == previousJIntersection.Item2).First().ConnectedIntersections.Add(new Tuple<int, int>(i, j));
                            }
                            if (!hasSeenWallInI[j])
                            {
                                connectedIntersections.Add(previousIIntersection[j]);
                                if (previousIIntersection[j] != null)
                                {
                                    newMaze.Points[previousIIntersection[j].Item1].Where(jCol => jCol.Point.Item2 == previousIIntersection[j].Item2).First().ConnectedIntersections.Add(new Tuple<int, int>(i, j));
                                }
                            }
                            newMaze.Points[i].Add(new AStarIntersectionPoint()
                            {
                                HasUpward = directions.Item1,
                                HasRight = directions.Item2,
                                HasDownward = directions.Item3,
                                HasLeft = directions.Item4,
                                Point = new Tuple<int, int>(i, j),
                                Weighting = CalculateWeighting(i, j, newMaze.EndPoint.Point),
                                ConnectedIntersections = connectedIntersections
                            });
                            hasSeenWallInJ = false;
                            hasSeenWallInI[j] = false;
                            previousJIntersection = new Tuple<int, int>(i, j);
                            previousIIntersection[j] = new Tuple<int, int>(i, j);
                        }
                    }
                    else
                    {
                        hasSeenWallInJ = true;
                        hasSeenWallInI[j] = true;
                    }
                }
            }
            var startConnector = newMaze.Points[newMaze.StartPoint.Point.Item1].Where(j => j.Point.Item2 > newMaze.StartPoint.Point.Item2).OrderBy(p => p.Point.Item2).First();
            startConnector.ConnectedIntersections.Add(newMaze.StartPoint.Point);
            newMaze.StartPoint.ConnectedIntersections.Add(startConnector.Point);
            var endConnector = newMaze.Points[newMaze.EndPoint.Point.Item1].Where(j => j.Point.Item2 < newMaze.EndPoint.Point.Item2).OrderBy(p => p.Point.Item2).Last();
            endConnector.ConnectedIntersections.Add(newMaze.EndPoint.Point);
            newMaze.EndPoint.ConnectedIntersections.Add(endConnector.Point);
            return newMaze;
        }

        private double CalculateWeighting(double i, double j, Tuple<int, int> endPoint)
        {
            return Math.Sqrt((Math.Abs(endPoint.Item1 - i) * Math.Abs(endPoint.Item1 - i)) + (Math.Abs(endPoint.Item2 - j) * Math.Abs(endPoint.Item2 - j)));
        }

        public IntersectionMazeImage CreateIntersectionMaze(string filePath)
        {
            Bitmap img = new Bitmap(filePath);
            IntersectionMazeImage newMaze = new IntersectionMazeImage(img.Width, img.Height);
            bool hasSeenWallInJ = false;
            bool[] hasSeenWallInI = new bool[img.Width];
            IntersectionPoint[] previousIIntersection = new IntersectionPoint[img.Width];
            IntersectionPoint previousJIntersection = null;
            List<IntersectionPoint> newMazePoints = new List<IntersectionPoint>();
            for (int i = 0; i < img.Width; i++)
            {
                newMaze.ISections[i] = newMazePoints.Count;
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (IsPixelSpace(pixel))
                    {
                        if (j == 0)
                        {
                            newMaze.StartPoint = new IntersectionPoint()
                            {
                                HasUpward = false,
                                HasRight = false,
                                HasDownward = true,
                                HasLeft = false,
                                Point = new Tuple<int, int>(i, j)
                            };
                        }
                        if (j == img.Height - 1)
                        {
                            newMaze.EndPoint = new IntersectionPoint()
                            {
                                HasUpward = false,
                                HasRight = false,
                                HasDownward = true,
                                HasLeft = false,
                                Point = new Tuple<int, int>(i, j)
                            };
                        }
                        if (IsIntersection(i, j, img, out Tuple<bool, bool, bool, bool> directions))
                        {
                            List<IntersectionPoint> connectedIntersections = new List<IntersectionPoint>();
                            if (!hasSeenWallInJ)
                            {
                                connectedIntersections.Add(previousJIntersection);
                                var startI = newMaze.ISections[previousJIntersection.Point.Item1];
                                IntersectionPoint cachedPoint = new IntersectionPoint();
                                for (int localI = startI; localI < newMazePoints.Count; localI++)
                                {
                                    cachedPoint = newMazePoints[localI];
                                    if (cachedPoint.Point.Item1 != previousJIntersection.Point.Item1)
                                    {
                                        break;
                                    }
                                    if (cachedPoint.Point.Item2 == previousJIntersection.Point.Item2)
                                    {
                                        cachedPoint.ConnectedIntersections.Add(cachedPoint);
                                        break;
                                    }
                                }
                            }
                            if (!hasSeenWallInI[j])
                            {
                                connectedIntersections.Add(previousIIntersection[j]);
                                if (previousIIntersection[j] != null)
                                {
                                    var startI = newMaze.ISections[previousIIntersection[j].Point.Item1];
                                    IntersectionPoint cachedPoint = new IntersectionPoint();
                                    for (int localI = startI; localI < newMazePoints.Count; localI++)
                                    {
                                        cachedPoint = newMazePoints[localI];
                                        if (cachedPoint.Point.Item1 != previousIIntersection[j].Point.Item1)
                                        {
                                            break;
                                        }
                                        if (cachedPoint.Point.Item2 == previousIIntersection[j].Point.Item2)
                                        {
                                            cachedPoint.ConnectedIntersections.Add(cachedPoint);
                                            break;
                                        }
                                    }
                                }
                            }
                            var currentPoint = new IntersectionPoint()
                            {
                                HasUpward = directions.Item1,
                                HasRight = directions.Item2,
                                HasDownward = directions.Item3,
                                HasLeft = directions.Item4,
                                HasVisited = false,
                                Point = new Tuple<int, int>(i, j),
                                ConnectedIntersections = connectedIntersections
                            };
                            newMazePoints.Add(currentPoint);
                            hasSeenWallInJ = false;
                            hasSeenWallInI[j] = false;
                            previousJIntersection = currentPoint;
                            previousIIntersection[j] = currentPoint;
                        }
                    }
                    else
                    {
                        hasSeenWallInJ = true;
                        hasSeenWallInI[j] = true;
                    }
                }
            }
            IntersectionPoint startConnector = new IntersectionPoint();
            IntersectionPoint endConnector = new IntersectionPoint();
            var startPointI = newMaze.ISections[newMaze.StartPoint.Point.Item1];
            var endPointI = newMaze.ISections[newMaze.EndPoint.Point.Item1];

            for (int i = startPointI; i < newMazePoints.Count; i++)
            {
                if (newMazePoints[i].Point.Item1 != newMaze.StartPoint.Point.Item1)
                {
                    break;
                }
                startConnector = newMazePoints[i];
                break;
            }
            for (int i = newMazePoints.Count - 1; i >= endPointI; i--)
            {
                if (newMazePoints[i].Point.Item1 == newMaze.EndPoint.Point.Item1)
                {
                    endConnector = newMazePoints[i];
                    break;
                }
            }
            startConnector.ConnectedIntersections.Add(newMaze.StartPoint);
            newMaze.StartPoint.ConnectedIntersections.Add(startConnector);
            endConnector.ConnectedIntersections.Add(newMaze.EndPoint);
            newMaze.EndPoint.ConnectedIntersections.Add(endConnector);
            newMaze.Points = newMazePoints.ToArray();
            return newMaze;
        }

        private bool IsIntersection(int i, int j, Bitmap img, out Tuple<bool, bool, bool, bool> directions)
        {
            bool hasUp = false;
            bool hasRight = false;
            bool hasDown = false;
            bool hasLeft = false;

            if (j - 1 >= 0)
            {
                Color pixel = img.GetPixel(i, j - 1);
                if (IsPixelSpace(pixel))
                {
                    hasUp = true;
                }
            }
            if (i + 1 < img.Width)
            {
                Color pixel = img.GetPixel(i + 1, j);
                if (IsPixelSpace(pixel))
                {
                    hasRight = true;
                }
            }
            if (j + 1 < img.Height)
            {
                Color pixel = img.GetPixel(i, j + 1);
                if (IsPixelSpace(pixel))
                {
                    hasDown = true;
                }
            }
            if (i - 1 >= 0)
            {
                Color pixel = img.GetPixel(i - 1, j);
                if (IsPixelSpace(pixel))
                {
                    hasLeft = true;
                }
            }
            directions = new Tuple<bool, bool, bool, bool>(hasUp, hasRight, hasDown, hasLeft);
            return AtLeastThree(hasUp, hasRight, hasDown, hasLeft) || ChangeInDirection(hasUp, hasRight, hasDown, hasLeft);
        }

        private bool ChangeInDirection(bool hasUp, bool hasRight, bool hasDown, bool hasLeft)
        {
            return (hasUp && (hasLeft || hasRight)) || (hasDown && (hasLeft || hasRight));
        }

        private bool AtLeastThree(bool hasUp, bool hasRight, bool hasDown, bool hasLeft)
        {
            return (hasUp ? 1 : 0) + (hasRight ? 1 : 0) + (hasDown ? 1 : 0) + (hasLeft ? 1 : 0) >= 3;
        }

        public NaiveMazeImage CreateNaiveMaze(string filePath)
        {
            NaiveMazeImage newMaze = new NaiveMazeImage();
            Bitmap img = new Bitmap(filePath);
            newMaze.MazeHeight = img.Height;
            newMaze.MazeWidth = img.Width;
            for (int i = 0; i < img.Width; i++)
            {
                newMaze.Points.Add(new List<bool>());
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (IsPixelSpace(pixel))
                    {
                        if (j == 0)
                        {
                            newMaze.StartPoint = new Tuple<int, int>(i, j);
                        }
                        if (j == img.Height - 1)
                        {
                            newMaze.EndPoint = new Tuple<int, int>(i, j);
                        }
                        newMaze.Points[i].Add(true);
                    }
                    else
                    {
                        newMaze.Points[i].Add(false);
                    }
                }
            }
            return newMaze;
        }

        public bool IsPixelSpace(Color pixel)
        {
            return pixel.R == 255 && pixel.G == 255 & pixel.B == 255;
        }
    }
}
