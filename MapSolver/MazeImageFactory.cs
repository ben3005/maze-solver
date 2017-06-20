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
            var newMaze = new AStarIntersectionMazeImage();
            var img = new Bitmap(filePath);
            var hasSeenWallInJ = false;
            var hasSeenWallInI = new bool[img.Width];
            var previousIIntersection = new Tuple<int, int>[img.Width];
            var previousJIntersection = new Tuple<int, int>(0, 0);
            newMaze.MazeHeight = img.Height;
            newMaze.MazeWidth = img.Width;
            for (var i = 0; i < img.Width; i++)
            {
                newMaze.Points.Add(new List<AStarIntersectionPoint>());
                for (var j = img.Height - 1; j >= 0; j--)
                {
                    var pixel = img.GetPixel(i, j);
                    if (IsPixelSpace(pixel))
                    {
                        if (j == img.Height - 1)
                        {
                            newMaze.EndPoint = new AStarIntersectionPoint()
                            {
                                Weighting = 0,
                                Point = new Tuple<int, int>(i, j)
                            };
                        }
                        if (j == 0)
                        {
                            newMaze.StartPoint = new AStarIntersectionPoint()
                            {
                                Weighting = CalculateWeighting(i, j, newMaze.EndPoint.Point),
                                Point = new Tuple<int, int>(i, j)
                            };
                        }
                        if (IsIntersection(i, j, img))
                        {
                            var connectedIntersections = new List<Tuple<int, int>>();
                            if (!hasSeenWallInJ)
                            {
                                connectedIntersections.Add(previousJIntersection);
                                newMaze.Points[previousJIntersection.Item1].First(jCol => jCol.Point.Item2 == previousJIntersection.Item2).ConnectedIntersections.Add(new Tuple<int, int>(i, j));
                            }
                            if (!hasSeenWallInI[j])
                            {
                                connectedIntersections.Add(previousIIntersection[j]);
                                if (previousIIntersection[j] != null)
                                {
                                    newMaze.Points[previousIIntersection[j].Item1].First(jCol => jCol.Point.Item2 == previousIIntersection[j].Item2).ConnectedIntersections.Add(new Tuple<int, int>(i, j));
                                }
                            }
                            newMaze.Points[i].Add(new AStarIntersectionPoint()
                            {
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

        
        private bool IsIntersection(int i, int j, Bitmap img)
        {
            var hasUp = false;
            var hasRight = false;
            var hasDown = false;
            var hasLeft = false;

            if (j - 1 >= 0)
            {
                var pixel = img.GetPixel(i, j - 1);
                if (IsPixelSpace(pixel))
                {
                    hasUp = true;
                }
            }
            if (i + 1 < img.Width)
            {
                var pixel = img.GetPixel(i + 1, j);
                if (IsPixelSpace(pixel))
                {
                    hasRight = true;
                }
            }
            if (j + 1 < img.Height)
            {
                var pixel = img.GetPixel(i, j + 1);
                if (IsPixelSpace(pixel))
                {
                    hasDown = true;
                }
            }
            if (i - 1 >= 0)
            {
                var pixel = img.GetPixel(i - 1, j);
                if (IsPixelSpace(pixel))
                {
                    hasLeft = true;
                }
            }
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
            var newMaze = new NaiveMazeImage();
            var img = new Bitmap(filePath);
            newMaze.MazeHeight = img.Height;
            newMaze.MazeWidth = img.Width;
            for (var i = 0; i < img.Width; i++)
            {
                newMaze.Points.Add(new List<bool>());
                for (var j = 0; j < img.Height; j++)
                {
                    var pixel = img.GetPixel(i, j);
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
