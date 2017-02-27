using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MapSolver
{
    public class MazeImageFactory
    {
        public IntersectionMazeImage CreateIntersectionMaze(string filePath)
        {
            IntersectionMazeImage newMaze = new IntersectionMazeImage();
            Bitmap img = new Bitmap(filePath);
            bool hasSeenWallInJ = false;
            bool[] hasSeenWallInI = new bool[img.Width];
            Tuple<int, int>[] previousIIntersection = new Tuple<int, int>[img.Width];
            Tuple<int, int> previousJIntersection = null;
            newMaze.MazeHeight = img.Height;
            newMaze.MazeWidth = img.Width;
            for (int i = 0; i < img.Width; i++)
            {
                newMaze.Points.Add(new List<IntersectionPoint>());
                for (int j = 0; j < img.Height; j++)
                {
                    Color pixel = img.GetPixel(i, j);
                    if (IsPixelSpace(pixel))
                    {
                        if (j == 0)
                        {
                            newMaze.StartPoint = new Tuple<int, int>(i, j);
                            newMaze.Points[i].Add(new IntersectionPoint()
                            {
                                HasUpward = false,
                                HasRight = false,
                                HasDownward = true,
                                HasLeft = false,
                                Point = new Tuple<int, int>(i, j)
                            });
                        }
                        if (j == img.Height - 1)
                        {
                            newMaze.EndPoint = new Tuple<int, int>(i, j);
                            newMaze.Points[i].Add(new IntersectionPoint()
                            {
                                HasUpward = true,
                                HasRight = false,
                                HasDownward = false,
                                HasLeft = false,
                                Point = new Tuple<int, int>(i, j)
                            });
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
                            newMaze.Points[i].Add(new IntersectionPoint()
                            {
                                HasUpward = directions.Item1,
                                HasRight = directions.Item2,
                                HasDownward = directions.Item3,
                                HasLeft = directions.Item4,
                                Point = new Tuple<int, int>(i, j),
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
            var startConnector = newMaze.Points[newMaze.StartPoint.Item1].Where(j => j.Point.Item2 == newMaze.StartPoint.Item2 + 1).First();
            startConnector.ConnectedIntersections.Add(newMaze.StartPoint);
            newMaze.Points[newMaze.StartPoint.Item1].Where(j => j.Point.Equals(newMaze.StartPoint)).First().ConnectedIntersections.Add(startConnector.Point);
            var endConnector = newMaze.Points[newMaze.EndPoint.Item1].Where(j => j.Point.Item2 == newMaze.EndPoint.Item2 - 1).First();
            endConnector.ConnectedIntersections.Add(newMaze.EndPoint);
            newMaze.Points[newMaze.EndPoint.Item1].Where(j => j.Point.Equals(newMaze.EndPoint)).First().ConnectedIntersections.Add(endConnector.Point);
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
